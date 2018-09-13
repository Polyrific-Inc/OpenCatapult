// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polyrific.Catapult.Engine.Core.Exceptions;
using Polyrific.Catapult.Engine.Core.JobTasks;
using Polyrific.Catapult.Shared.Common;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.JobDefinition;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Engine.Core
{
    public class TaskRunner : ITaskRunner
    {
        private readonly JobTaskService _jobTaskService;
        private readonly ILogger _logger;

        public TaskRunner(JobTaskService jobTaskService, ILogger<TaskRunner> logger)
        {
            _jobTaskService = jobTaskService;
            _logger = logger;
        }

        public async Task<Dictionary<int, TaskRunnerResult>> Run(int projectId, string queueCode, List<JobTaskDefinitionDto> jobTasks, string pluginsLocation)
        {
            var orderedJobTasks = jobTasks.OrderBy(t => t.Sequence).ToList();

            _logger.LogInformation($"[Queue \"{queueCode}\"] Attempting to run {orderedJobTasks.Count} job tasks");

            var results = orderedJobTasks.Select(t => (t.Id, new TaskRunnerResult())).ToDictionary(r => r.Item1, r => r.Item2);

            RefreshPlugins(pluginsLocation, orderedJobTasks.Select(t => t.Type).ToArray());

            foreach (var jobTask in orderedJobTasks)
            {
                var taskObj = GetJobTaskInstance(projectId, queueCode, jobTask);

                _logger.LogInformation($"[Queue \"{queueCode}\"] Running {jobTask.Type} pre-processing task");
                await taskObj.RunPreprocessingTask();

                _logger.LogInformation($"[Queue \"{queueCode}\"] Running {jobTask.Type} task");
                var result = await taskObj.RunMainTask();
                results[jobTask.Id] = result;

                _logger.LogInformation($"[Queue \"{queueCode}\"] Running {jobTask.Type} post-processing task");
                await taskObj.RunPostprocessingTask();
                
                if (!result.IsSuccess && result.StopTheProcess)
                    break;
            }

            _logger.LogInformation($"[Queue \"{queueCode}\"] Job tasks execution complete with the following result: {results}");

            return results;
        }

        private IJobTask GetJobTaskInstance(int projectId, string queueCode, JobTaskDefinitionDto jobTask)
        {
            IJobTask task;
            switch (jobTask.Type)
            {
                case JobTaskDefinitionType.Build:
                    task = _jobTaskService.BuildTask;
                    break;
                case JobTaskDefinitionType.Deploy:
                    task = _jobTaskService.DeployTask;
                    break;
                case JobTaskDefinitionType.Generate:
                    task = _jobTaskService.GenerateTask;
                    break;
                case JobTaskDefinitionType.Push:
                    task = _jobTaskService.PushTask;
                    break;
                default:
                    throw new InvalidJobTaskTypeException(jobTask.Type);
            }

            task.ProjectId = projectId;
            task.JobTaskId = jobTask.Id;
            task.JobQueueCode = queueCode;
            task.SetConfig(JsonConvert.SerializeObject(jobTask.Config));

            return task;
        }

        private void RefreshPlugins(string pluginsLocation, string[] jobTaskTypes)
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
                    case JobTaskDefinitionType.Deploy:
                        providerType = PluginType.DeployProvider;
                        break;
                    case JobTaskDefinitionType.Generate:
                        providerType = PluginType.GeneratorProvider;
                        break;
                    case JobTaskDefinitionType.Push:
                        providerType = PluginType.RepositoryProvider;
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

            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }
    }
}
