// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Service;
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
        /// <param name="projectService">Instance of <see cref="IProjectService"/></param>
        /// <param name="logger">Logger</param>
        public PushTask(IProjectService projectService, ILogger<PushTask> logger) 
            : base(projectService, logger)
        {
        }

        public override string Type => JobTaskDefinitionType.Push;

        [ImportMany(typeof(ICodeRepositoryProvider))]
        public IEnumerable<ICodeRepositoryProvider> CodeRepositoryProviders;

        public override async Task<TaskRunnerResult> RunPreprocessingTask()
        {
            var provider = CodeRepositoryProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Code repository provider \"{Provider}\" could not be found.");

            var serviceProperties = GetServiceProperties(provider.RequiredServices);

            var error = await provider.BeforePush(JobQueueCode, TaskConfig, serviceProperties);
            if (!string.IsNullOrEmpty(error))
                return new TaskRunnerResult(error, TaskConfig.PreProcessMustSucceed);

            return new TaskRunnerResult(true, "");
        }

        public override async Task<TaskRunnerResult> RunMainTask()
        {
            var provider = CodeRepositoryProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Code repository provider \"{Provider}\" could not be found.");

            var serviceProperties = GetServiceProperties(provider.RequiredServices);

            var result = await provider.Push(JobQueueCode, TaskConfig, serviceProperties, Logger);
            if (!string.IsNullOrEmpty(result.errorMessage))
                return new TaskRunnerResult(result.errorMessage, !TaskConfig.ContinueWhenError);

            return new TaskRunnerResult(true, result.returnValue);
        }

        public override async Task<TaskRunnerResult> RunPostprocessingTask()
        {
            var provider = CodeRepositoryProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Code repository provider \"{Provider}\" could not be found.");

            var serviceProperties = GetServiceProperties(provider.RequiredServices);

            var error = await provider.AfterPush(JobQueueCode, TaskConfig, serviceProperties);
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
