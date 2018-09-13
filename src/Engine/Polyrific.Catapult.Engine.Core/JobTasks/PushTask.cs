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
    public class PushTask : BaseJobTask<PushTaskConfig>, IPushTask
    {
        /// <summary>
        /// Instantiate <see cref="PushTask"/>
        /// </summary>
        /// <param name="logger">Logger</param>
        public PushTask(ILogger<PushTask> logger) : base(logger)
        {
        }

        public override string Type => JobTaskDefinitionType.Push;

        [ImportMany(typeof(ICodeRepositoryProvider))]
        public IEnumerable<ICodeRepositoryProvider> CodeRepositoryProviders;

        public override async Task<TaskRunnerResult> RunMainTask()
        {
            var provider = CodeRepositoryProviders?.FirstOrDefault(p => p.Name == TaskConfig.ProviderName);
            if (provider == null)
                return new TaskRunnerResult($"Code repository provider \"{TaskConfig.ProviderName}\" could not be found.");

            var result = await provider.Push(TaskConfig);
            if (!string.IsNullOrEmpty(result.errorMessage))
                return new TaskRunnerResult(result.errorMessage, !TaskConfig.ContinueWhenError);

            return new TaskRunnerResult(true, result.returnValue);
        }
    }
}