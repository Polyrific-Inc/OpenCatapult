// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polyrific.Catapult.Engine.Core.Exceptions;
using Polyrific.Catapult.Engine.Core.JobLogger;
using Polyrific.Catapult.Engine.Core.JobTasks;
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
        private readonly IPluginManager _pluginManager;
        private readonly ILogger _logger;

        public TaskRunner(JobTaskService jobTaskService, IJobQueueService jobQueueService, IPluginManager pluginManager, ILogger<TaskRunner> logger)
        {
            _jobTaskService = jobTaskService;
            _jobQueueService = jobQueueService;
            _pluginManager = pluginManager;
            _logger = logger;
        }

        public async Task<Dictionary<int, TaskRunnerResult>> Run(int projectId, JobDto job, List<JobTaskDefinitionDto> jobTasks, string pluginsLocation, string workingLocation)
        {
            _pluginManager.RefreshPlugins();

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
                Status = job.Status,
                OutputValues = job.OutputValues
            });

            var orderedJobTasks = jobTasks.OrderBy(t => t.Sequence).ToList();

            _logger.LogInformation("[Queue {Code}] Attempting to run {Count} job tasks", job.Code, orderedJobTasks.Count);

            var results = orderedJobTasks.ToDictionary(t => t.Id, t => new TaskRunnerResult());

            var outputValues = job.OutputValues ?? new Dictionary<string, string>();
            job.JobTasksStatus = job.JobTasksStatus?.Count > 0 ? job.JobTasksStatus :
                orderedJobTasks.Select((t, idx) => new JobTaskStatusDto
                {
                    TaskName = t.Name,
                    Sequence = idx + 1,
                    Status = JobTaskStatusType.NotExecuted
                }).ToList();

            foreach (var jobTask in orderedJobTasks)
            {
                using (_logger.BeginScope(new TaskScope(jobTask.Name)))
                {
                    // skip previously success task
                    var jobTaskStatus = job.JobTasksStatus.First(t => t.TaskName == jobTask.Name);
                    if (jobTaskStatus.Status == JobTaskStatusType.Success)
                    {
                        results[jobTask.Id] = new TaskRunnerResult(true, "");
                        continue;
                    }

                    var taskObj = GetJobTaskInstance(projectId, job.Code, jobTask, workingLocation);

                    // pre-processing
                    _logger.LogInformation("[Queue {Code}] Running {Type} pre-processing task", job.Code, jobTask.Type);
                    var preResult = await taskObj.RunPreprocessingTask();
                    if (!preResult.IsSuccess && preResult.StopTheProcess)
                    {
                        _logger.LogError("[Queue {Code}]  Execution of {Type} pre-processing task was failed, stopping the next task execution. Error: {ErrorMessage}", job.Code, jobTask.Type, preResult.ErrorMessage);
                        jobTaskStatus.Status = JobTaskStatusType.Failed;
                        jobTaskStatus.Remarks = preResult.ErrorMessage;
                        break;
                    }

                    // main process
                    _logger.LogInformation("[Queue {Code}] Running {jobTask.Type} task", job.Code, jobTask.Type);
                    var result = await taskObj.RunMainTask(outputValues);
                    results[jobTask.Id] = result;
                    if (!result.IsSuccess && result.StopTheProcess)
                    {
                        _logger.LogError("[Queue {Code}] Execution of {Type} task was failed, stopping the next task execution. Error: {ErrorMessage}", job.Code, jobTask.Type, result.ErrorMessage);
                        jobTaskStatus.Status = JobTaskStatusType.Failed;
                        jobTaskStatus.Remarks = result.ErrorMessage;
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
                    _logger.LogInformation("[Queue {Code}] Running {Type} post-processing task", job.Code, jobTask.Type);
                    var postResult = await taskObj.RunPostprocessingTask();
                    if (!postResult.IsSuccess && postResult.StopTheProcess)
                    {
                        _logger.LogError("[Queue {Code}] Execution of {Type} post-processing task was failed, stopping the next task execution. Error: {ErrorMessage}", job.Code, jobTask.Type, postResult.ErrorMessage);
                        jobTaskStatus.Status = JobTaskStatusType.Failed;
                        jobTaskStatus.Remarks = postResult.ErrorMessage;
                        break;
                    }

                    // check if there's a need to postpone the next task
                    if (result.IsSuccess && result.StopTheProcess)
                    {
                        _logger.LogInformation("[Queue {Code}] Execution of {Type} require the job to be halted. Run the restart command in order to restart the job", job.Code, jobTask.Type);
                        jobTaskStatus.Status = JobTaskStatusType.Success;
                        jobTaskStatus.Remarks = result.ReturnValue;

                        int currentTaskStatusIdx = job.JobTasksStatus.IndexOf(jobTaskStatus);
                        var nextTaskStatus = currentTaskStatusIdx == job.JobTasksStatus.Count - 1 ? null : job.JobTasksStatus[currentTaskStatusIdx + 1];
                        nextTaskStatus.Status = JobTaskStatusType.Pending;
                        nextTaskStatus.Remarks = result.StopRemarks;
                        break;
                    }

                    jobTaskStatus.Status = JobTaskStatusType.Success;
                    jobTaskStatus.Remarks = result.ReturnValue;
                    await _jobQueueService.UpdateJobQueue(job.Id, new UpdateJobDto
                    {
                        Id = job.Id,
                        CatapultEngineId = job.CatapultEngineId,
                        CatapultEngineIPAddress = job.CatapultEngineIPAddress,
                        CatapultEngineMachineName = job.CatapultEngineMachineName,
                        CatapultEngineVersion = job.CatapultEngineVersion,
                        JobTasksStatus = job.JobTasksStatus,
                        JobType = job.JobType,
                        Status = job.Status,
                        OutputValues = outputValues
                    });
                }
            }

            job.OutputValues = outputValues;

            var serializedResult = JsonConvert.SerializeObject(results);
            _logger.LogInformation("[Queue {Code}] Job tasks execution complete with the following result: {serializedResult}", job.Code, serializedResult);

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

            task.ReloadProperties();
            task.ProjectId = projectId;
            task.JobTaskId = jobTask.Id;
            task.Provider = jobTask.Provider;
            task.JobQueueCode = queueCode;
            task.SetConfig(jobTask.Configs, workingLocation);
            task.AdditionalConfigs = jobTask.AdditionalConfigs;

            return task;
        }
    }
}
