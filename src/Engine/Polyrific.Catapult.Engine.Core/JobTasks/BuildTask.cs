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
    public class BuildTask : BaseJobTask<BuildTaskConfig>, IBuildTask
    {
        /// <summary>
        /// Instantiate <see cref="BuildTask"/>
        /// </summary>
        /// <param name="logger">Logger</param>
        public BuildTask(ILogger<BuildTask> logger) : base(logger)
        {
            
        }

        public override string Type => JobTaskDefinitionType.Build;

        [ImportMany(typeof(IBuildProvider))]
        public IEnumerable<IBuildProvider> BuildProviders;
        
        public override async Task<TaskRunnerResult> RunMainTask()
        {
            var provider = BuildProviders?.FirstOrDefault(p => p.Name == TaskConfig.ProviderName);
            if (provider == null)
                return new TaskRunnerResult($"Build provider \"{TaskConfig.ProviderName}\" could not be found.");

            var result = await provider.Build(TaskConfig);
            if (!string.IsNullOrEmpty(result.errorMessage))
                return new TaskRunnerResult(result.errorMessage, !TaskConfig.ContinueWhenError);

            return new TaskRunnerResult(true, result.returnValue);
        }
    }
}