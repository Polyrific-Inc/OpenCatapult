﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

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
    public class TestTask : BaseJobTask<TestTaskConfig>, ITestTask
    {
        public TestTask(IProjectService projectService, IExternalServiceService externalServiceService, IPluginManager pluginManager, ILogger<TestTask> logger) 
            : base(projectService, externalServiceService, pluginManager, logger)
        {
        }

        public override string Type => JobTaskDefinitionType.Test;

        [ImportMany(typeof(ITestProvider))]
        public IEnumerable<ITestProvider> TestProvider;

        public override async Task<TaskRunnerResult> RunPreprocessingTask()
        {
            var provider = TestProvider?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Test provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);
            
            var error = await provider.BeforeTest(TaskConfig, AdditionalConfigs, Logger);
            if (!string.IsNullOrEmpty(error))
                return new TaskRunnerResult(error, TaskConfig.PreProcessMustSucceed);

            return new TaskRunnerResult(true, "");
        }

        public override async Task<TaskRunnerResult> RunMainTask(Dictionary<string, string> previousTasksOutputValues)
        {
            var provider = TestProvider?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Test provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var result = await provider.Test(TaskConfig, AdditionalConfigs, Logger);
            if (!string.IsNullOrEmpty(result.errorMessage))
                return new TaskRunnerResult(result.errorMessage, !TaskConfig.ContinueWhenError);

            return new TaskRunnerResult(true, result.testResultLocation, result.outputValues);
        }

        public override async Task<TaskRunnerResult> RunPostprocessingTask()
        {
            var provider = TestProvider?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Test provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var error = await provider.AfterTest(TaskConfig, AdditionalConfigs, Logger);
            if (!string.IsNullOrEmpty(error))
                return new TaskRunnerResult(error, TaskConfig.PostProcessMustSucceed);

            return new TaskRunnerResult(true, "");
        }
    }
}
