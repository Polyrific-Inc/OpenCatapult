// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using Polyrific.Catapult.Shared.Dto.Constants;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Engine.Core.JobTasks
{
    public class DeployTask : BaseJobTask<DeployTaskConfig>, IDeployTask
    {
        /// <summary>
        /// Instantiate <see cref="DeployTask"/>
        /// </summary>
        /// <param name="logger">Logger</param>
        public DeployTask(ILogger<DeployTask> logger) : base(logger)
        {
        }

        public override string Type => JobTaskDefinitionType.Deploy;

        [ImportMany(typeof(IDeployProvider))]
        public IEnumerable<IDeployProvider> DeployProviders;

        public override async Task<TaskRunnerResult> RunMainTask()
        {
            var provider = DeployProviders?.FirstOrDefault(p => p.Name == TaskConfig.ProviderName);
            if (provider == null)
                return new TaskRunnerResult($"Deploy provider \"{TaskConfig.ProviderName}\" could not be found.");

            var result = await provider.Deploy(TaskConfig);
            if (!string.IsNullOrEmpty(result.errorMessage))
                return new TaskRunnerResult(result.errorMessage, !TaskConfig.ContinueWhenError);

            return new TaskRunnerResult(true, result.returnValue);
        }
    }
}