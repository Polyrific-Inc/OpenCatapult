// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Engine.Core.JobTasks
{
    public class PublishArtifactTask : BaseJobTask<PublishArtifactTaskConfig>, IPublishArtifactTask
    {
        public PublishArtifactTask(IProjectService projectService, ILogger logger) : base(projectService, logger)
        {
        }

        public override string Type => JobTaskDefinitionType.PublishArtifact;

        [ImportMany(typeof(IStorageProvider))]
        public IEnumerable<IStorageProvider> StorageProvider;

        public override async Task<TaskRunnerResult> RunPreprocessingTask()
        {
            var provider = StorageProvider?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Storage provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var error = await provider.BeforePublishArtifact(TaskConfig, AdditionalConfigs, Logger);
            if (!string.IsNullOrEmpty(error))
                return new TaskRunnerResult(error, TaskConfig.PreProcessMustSucceed);

            return new TaskRunnerResult(true, "");
        }

        public override async Task<TaskRunnerResult> RunMainTask(Dictionary<string, string> previousTasksOutputValues)
        {
            var provider = StorageProvider?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Storage provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var result = await provider.PublishArtifact(TaskConfig, AdditionalConfigs, Logger);
            if (!string.IsNullOrEmpty(result.errorMessage))
                return new TaskRunnerResult(result.errorMessage, !TaskConfig.ContinueWhenError);

            return new TaskRunnerResult(true, result.storageLocation, result.outputValues);
        }

        public override async Task<TaskRunnerResult> RunPostprocessingTask()
        {
            var provider = StorageProvider?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Storage provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var error = await provider.AfterPublishArtifact(TaskConfig, AdditionalConfigs, Logger);
            if (!string.IsNullOrEmpty(error))
                return new TaskRunnerResult(error, TaskConfig.PostProcessMustSucceed);

            return new TaskRunnerResult(true, "");
        }
    }
}
