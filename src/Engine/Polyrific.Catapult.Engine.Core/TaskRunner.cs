// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Engine.Core.Exceptions;
using Polyrific.Catapult.Engine.Core.JobTasks;
using Polyrific.Catapult.Shared.Common;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.JobDefinition;

namespace Polyrific.Catapult.Engine.Core
{
    public class TaskRunner : ITaskRunner
    {
        private readonly JobTaskService _jobTaskService;
        private readonly ILogger _logger;
        private CompositionContainer _compositionContainer;

        public TaskRunner(JobTaskService jobTaskService, ILogger<TaskRunner> logger)
        {
            _jobTaskService = jobTaskService;
            _logger = logger;
        }
        
        public async Task<Dictionary<int, TaskRunnerResult>> Run(int projectId, string queueCode, List<JobTaskDefinitionDto> jobTasks, string pluginsLocation, string workingLocation)
        {
            var orderedJobTasks = jobTasks.OrderBy(t => t.Sequence).ToList();

            _logger.LogInformation($"[Queue \"{queueCode}\"] Attempting to run {orderedJobTasks.Count} job tasks");

            var results = orderedJobTasks.Select(t => (t.Id, new TaskRunnerResult())).ToDictionary(r => r.Item1, r => r.Item2);

            _compositionContainer = GetPluginsCompositionContainer(pluginsLocation, orderedJobTasks.Select(t => t.Type).ToArray());

            var outputValues = new Dictionary<string, string>();
            foreach (var jobTask in orderedJobTasks)
            {
                var taskObj = GetJobTaskInstance(projectId, queueCode, jobTask, workingLocation);

                // pre-processing
                _logger.LogInformation($"[Queue \"{queueCode}\"] Running {jobTask.Type} pre-processing task");
                var preResult = await taskObj.RunPreprocessingTask();
                if (!preResult.IsSuccess && preResult.StopTheProcess)
                {
                    _logger.LogError($"[Queue \"{queueCode}\"] Execution of {jobTask.Type} pre-processing task was failed, stopping the next task execution.");
                    break;
                }

                // main process
                _logger.LogInformation($"[Queue \"{queueCode}\"] Running {jobTask.Type} task");
                var result = await taskObj.RunMainTask(outputValues);
                results[jobTask.Id] = result;
                if (!result.IsSuccess && result.StopTheProcess)
                {
                    _logger.LogError($"[Queue \"{queueCode}\"] Execution of {jobTask.Type} task was failed, stopping the next task execution.");
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
                _logger.LogInformation($"[Queue \"{queueCode}\"] Running {jobTask.Type} post-processing task");
                var postResult = await taskObj.RunPostprocessingTask();
                if (!postResult.IsSuccess && postResult.StopTheProcess)
                {
                    _logger.LogError($"[Queue \"{queueCode}\"] Execution of {jobTask.Type} post-processing task was failed, stopping the next task execution.");
                    break;
                }
            }

            _logger.LogInformation($"[Queue \"{queueCode}\"] Job tasks execution complete with the following result: {results}");

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
