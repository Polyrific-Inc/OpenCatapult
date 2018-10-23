// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polyrific.Catapult.Engine.Core.Exceptions;
using Polyrific.Catapult.Engine.Core.JobLogger;
using Polyrific.Catapult.Engine.Core.JobTasks;
using Polyrific.Catapult.Shared.Common;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.JobDefinition;
using Polyrific.Catapult.Shared.Dto.JobQueue;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Engine.Core
{
    public class TaskRunner : ITaskRunner
    {
        private readonly JobTaskService _jobTaskService;
        private readonly IJobQueueService _jobQueueService;
        private readonly ILogger _logger;
        private CompositionContainer _compositionContainer;

        public TaskRunner(JobTaskService jobTaskService, IJobQueueService jobQueueService, ILogger<TaskRunner> logger)
        {
            _jobTaskService = jobTaskService;
            _jobQueueService = jobQueueService;
            _logger = logger;
        }

        public async Task<Dictionary<int, TaskRunnerResult>> Run(int projectId, JobDto job, List<JobTaskDefinitionDto> jobTasks, string pluginsLocation, string workingLocation)
        {
            // update the status to processing so the clients can start listening for logs
            job.Status = JobStatus.Processing;
            await _jobQueueService.UpdateJobQueue(job.Id, new UpdateJobDto
            {
                Id = job.Id,
                CatapultEngineId = job.CatapultEngineId,
                CatapultEngineIPAddress = job.CatapultEngineIPAddress,
                CatapultEngineMachineName = job.CatapultEngineMachineName,
                CatapultEngineVersion = job.CatapultEngineVersion,
                JobTasksStatus = job.JobTasksStatus,
                JobType = job.JobType,
                Status = job.Status
            });

            var orderedJobTasks = jobTasks.OrderBy(t => t.Sequence).ToList();

            _logger.LogInformation($"[Queue \"{job.Code}\"] Attempting to run {orderedJobTasks.Count} job tasks");

            var results = orderedJobTasks.Select(t => (t.Id, new TaskRunnerResult())).ToDictionary(r => r.Item1, r => r.Item2);

            _compositionContainer = GetPluginsCompositionContainer(pluginsLocation, orderedJobTasks.Select(t => t.Type).ToArray());

            var outputValues = new Dictionary<string, string>();
            var jobTaskStatuses = orderedJobTasks.Select((t, idx) => new JobTaskStatusDto
            {
                TaskName = t.Name,
                Sequence = idx + 1,
                Status = JobTaskStatusType.NotExecuted
            }).ToList();
            foreach (var jobTask in orderedJobTasks)
            {
                using (_logger.BeginScope(new TaskScope(jobTask.Name)))
                {
                    var jobTaskStatus = jobTaskStatuses.First(t => t.TaskName == jobTask.Name);
                    var taskObj = GetJobTaskInstance(projectId, job.Code, jobTask, workingLocation);

                    // pre-processing
                    _logger.LogInformation($"[Queue \"{job.Code}\"] Running {jobTask.Type} pre-processing task");
                    var preResult = await taskObj.RunPreprocessingTask();
                    if (!preResult.IsSuccess && preResult.StopTheProcess)
                    {
                        _logger.LogError($"[Queue \"{job.Code}\"] Execution of {jobTask.Type} pre-processing task was failed, stopping the next task execution.");
                        jobTaskStatus.Status = JobTaskStatusType.Failed;
                        jobTaskStatus.Remarks = preResult.ErrorMessage;
                        job.JobTasksStatus = JsonConvert.SerializeObject(jobTaskStatuses);
                        break;
                    }

                    // main process
                    _logger.LogInformation($"[Queue \"{job.Code}\"] Running {jobTask.Type} task");
                    var result = await taskObj.RunMainTask(outputValues);
                    results[jobTask.Id] = result;
                    if (!result.IsSuccess && result.StopTheProcess)
                    {
                        _logger.LogError($"[Queue \"{job.Code}\"] Execution of {jobTask.Type} task was failed, stopping the next task execution.");
                        jobTaskStatus.Status = JobTaskStatusType.Failed;
                        jobTaskStatus.Remarks = preResult.ErrorMessage;
                        job.JobTasksStatus = JsonConvert.SerializeObject(jobTaskStatuses);
                        break;
                    }

                    // save output values to be used as the input for the next tasks
                    if (result.OutputValues != null)
                    {
                        foreach (var key in result.OutputValues.Keys)
                        {
                            if (outputValues.ContainsKey(key))
                                outputValues[key] = result.OutputValues[key];
                            else
                                outputValues.Add(key, result.OutputValues[key]);
                        }
                    }

                    // post-processing
                    _logger.LogInformation($"[Queue \"{job.Code}\"] Running {jobTask.Type} post-processing task");
                    var postResult = await taskObj.RunPostprocessingTask();
                    if (!postResult.IsSuccess && postResult.StopTheProcess)
                    {
                        _logger.LogError($"[Queue \"{job.Code}\"] Execution of {jobTask.Type} post-processing task was failed, stopping the next task execution.");
                        jobTaskStatus.Status = JobTaskStatusType.Failed;
                        jobTaskStatus.Remarks = preResult.ErrorMessage;
                        job.JobTasksStatus = JsonConvert.SerializeObject(jobTaskStatuses);
                        break;
                    }

                    // check if there's a need to postpone the next task
                    if (result.IsSuccess && result.StopTheProcess)
                    {
                        _logger.LogInformation($"[Queue \"{job.Code}\"] Execution of {jobTask.Type} require the job to be halted. Run the restart command in order to restart the job");
                        jobTaskStatus.Status = JobTaskStatusType.Success;

                        int currentTaskStatusIdx = jobTaskStatuses.IndexOf(jobTaskStatus);
                        var nextTaskStatus = currentTaskStatusIdx == jobTaskStatuses.Count - 1 ? null : jobTaskStatuses[currentTaskStatusIdx + 1];
                        nextTaskStatus.Status = JobTaskStatusType.Pending;

                        job.JobTasksStatus = JsonConvert.SerializeObject(jobTaskStatuses);
                        break;
                    }

                    jobTaskStatus.Status = JobTaskStatusType.Success;
                    job.JobTasksStatus = JsonConvert.SerializeObject(jobTaskStatuses);
                    await _jobQueueService.UpdateJobQueue(job.Id, new UpdateJobDto
                    {
                        Id = job.Id,
                        CatapultEngineId = job.CatapultEngineId,
                        CatapultEngineIPAddress = job.CatapultEngineIPAddress,
                        CatapultEngineMachineName = job.CatapultEngineMachineName,
                        CatapultEngineVersion = job.CatapultEngineVersion,
                        JobTasksStatus = job.JobTasksStatus,
                        JobType = job.JobType,
                        Status = job.Status
                    });
                }
            }

            _logger.LogInformation($"[Queue \"{job.Code}\"] Job tasks execution complete with the following result: {results}");

            return results;
        }

        private IJobTask GetJobTaskInstance(int projectId, string queueCode, JobTaskDefinitionDto jobTask, string workingLocation)
        {
            IJobTask task;
            switch (jobTask.Type)
            {
                case JobTaskDefinitionType.Build:
                    task = _jobTaskService.BuildTask;
                    break;
                case JobTaskDefinitionType.Clone:
                    task = _jobTaskService.CloneTask;
                    break;
                case JobTaskDefinitionType.Deploy:
                    task = _jobTaskService.DeployTask;
                    break;
                case JobTaskDefinitionType.DeployDb:
                    task = _jobTaskService.DeployDbTask;
                    break;
                case JobTaskDefinitionType.Generate:
                    task = _jobTaskService.GenerateTask;
                    break;
                case JobTaskDefinitionType.Merge:
                    task = _jobTaskService.MergeTask;
                    break;
                case JobTaskDefinitionType.PublishArtifact:
                    task = _jobTaskService.PublishArtifactTask;
                    break;
                case JobTaskDefinitionType.Push:
                    task = _jobTaskService.PushTask;
                    break;
                case JobTaskDefinitionType.Test:
                    task = _jobTaskService.TestTask;
                    break;
                default:
                    throw new InvalidJobTaskTypeException(jobTask.Type);
            }

            task.ReloadProject();
            task.ProjectId = projectId;
            task.JobTaskId = jobTask.Id;
            task.Provider = jobTask.Provider;
            task.JobQueueCode = queueCode;
            task.SetConfig(jobTask.Configs, workingLocation);
            task.AdditionalConfigs = jobTask.AdditionalConfigs;
            _compositionContainer.ComposeParts(task);

            return task;
        }

        private CompositionContainer GetPluginsCompositionContainer(string pluginsLocation, string[] jobTaskTypes)
        {
            var catalog = new AggregateCatalog();

            foreach (var jobTaskType in jobTaskTypes)
            {
                string providerType;
                switch (jobTaskType)
                {
                    case JobTaskDefinitionType.Build:
                        providerType = PluginType.BuildProvider;
                        break;
                    case JobTaskDefinitionType.Clone:
                        providerType = PluginType.RepositoryProvider;
                        break;
                    case JobTaskDefinitionType.Deploy:
                        providerType = PluginType.HostingProvider;
                        break;
                    case JobTaskDefinitionType.DeployDb:
                        providerType = PluginType.DatabaseProvider;
                        break;
                    case JobTaskDefinitionType.Generate:
                        providerType = PluginType.GeneratorProvider;
                        break;
                    case JobTaskDefinitionType.Merge:
                        providerType = PluginType.RepositoryProvider;
                        break;
                    case JobTaskDefinitionType.PublishArtifact:
                        providerType = PluginType.StorageProvider;
                        break;
                    case JobTaskDefinitionType.Push:
                        providerType = PluginType.RepositoryProvider;
                        break;
                    case JobTaskDefinitionType.Test:
                        providerType = PluginType.TestProvider;
                        break;
                    default:
                        providerType = jobTaskType;
                        break;
                }

                var taskPluginsLocation = Path.Combine(pluginsLocation, providerType);
                var plugins = DirectoryHelper.GetChildFolders(taskPluginsLocation);

                foreach (var plugin in plugins)
                {
                    catalog.Catalogs.Add(new DirectoryCatalog(plugin));
                }
            }

            return new CompositionContainer(catalog);
        }
    }
}
