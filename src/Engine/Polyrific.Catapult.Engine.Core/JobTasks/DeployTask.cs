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
    public class DeployTask : BaseJobTask<DeployTaskConfig>, IDeployTask
    {
        /// <summary>
        /// Instantiate <see cref="DeployTask"/>
        /// </summary>
        /// <param name="projectService">Project service</param>
        /// <param name="logger">Logger</param>
        public DeployTask(IProjectService projectService, ILogger<DeployTask> logger) 
            : base(projectService, logger)
        {
        }

        public override string Type => JobTaskDefinitionType.Deploy;

        [ImportMany(typeof(IDeployProvider))]
        public IEnumerable<IDeployProvider> DeployProviders;

        public override async Task<TaskRunnerResult> RunPreprocessingTask()
        {
            var provider = DeployProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Deploy provider \"{Provider}\" could not be found.");

            var serviceProperties = GetServiceProperties(provider.RequiredServices);

            var error = await provider.BeforeDeploy(TaskConfig, serviceProperties, Logger);
            if (!string.IsNullOrEmpty(error))
                return new TaskRunnerResult(error, TaskConfig.PreProcessMustSucceed);

            return new TaskRunnerResult(true, "");
        }

        public override async Task<TaskRunnerResult> RunMainTask()
        {
            var provider = DeployProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Deploy provider \"{Provider}\" could not be found.");

            var serviceProperties = GetServiceProperties(provider.RequiredServices);

            var result = await provider.Deploy("artifact", TaskConfig, serviceProperties, Logger);
            if (!string.IsNullOrEmpty(result.errorMessage))
                return new TaskRunnerResult(result.errorMessage, !TaskConfig.ContinueWhenError);

            return new TaskRunnerResult(true, result.returnValue);
        }

        public override async Task<TaskRunnerResult> RunPostprocessingTask()
        {
            var provider = DeployProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Deploy provider \"{Provider}\" could not be found.");

            var serviceProperties = GetServiceProperties(provider.RequiredServices);

            var error = await provider.AfterDeploy(TaskConfig, serviceProperties, Logger);
            if (!string.IsNullOrEmpty(error))
                return new TaskRunnerResult(error, TaskConfig.PostProcessMustSucceed);

            return new TaskRunnerResult(true, "");
        }

        private Dictionary<string, string> GetServiceProperties(string[] serviceNames)
        {
            //TODO: retrieve properties from service manager
            return new Dictionary<string, string>();
        }
    }
}
