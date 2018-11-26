﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Engine.Core.JobTasks
{
    public class BuildTask : BaseJobTask<BuildTaskConfig>, IBuildTask
    {
        /// <inheritdoc />
        public BuildTask(IProjectService projectService, IExternalServiceService externalServiceService, IPluginManager pluginManager, ILogger<BuildTask> logger) 
            : base(projectService, externalServiceService, pluginManager, logger)
        {
            
        }

        public override string Type => JobTaskDefinitionType.Build;

        public List<PluginItem> BuildProviders => PluginManager.GetPlugins(Type);

        public override async Task<TaskRunnerResult> RunPreprocessingTask()
        {
            var provider = BuildProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Build provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var result = await InvokeTaskProvider(provider.DllPath, GetArgString("pre"));
            if (result.ContainsKey("error"))
                return new TaskRunnerResult(result["error"].ToString(), TaskConfig.PreProcessMustSucceed);

            return new TaskRunnerResult(true, "");
        }

        public override async Task<TaskRunnerResult> RunMainTask(Dictionary<string, string> previousTasksOutputValues)
        {
            var provider = BuildProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Build provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var result = await InvokeTaskProvider(provider.DllPath, GetArgString("main"));
            if (result.ContainsKey("errorMessage"))
                return new TaskRunnerResult(result["errorMessage"].ToString(), !TaskConfig.ContinueWhenError);

            var outputArtifact = "";
            if (result.ContainsKey("outputArtifact"))
                outputArtifact = result["outputArtifact"].ToString();
            
            var outputValues = new Dictionary<string, string>();
            if (result.ContainsKey("outputValues"))
                outputValues = result["outputValues"] as Dictionary<string, string>;

            return new TaskRunnerResult(true, outputArtifact, outputValues);
        }

        public override async Task<TaskRunnerResult> RunPostprocessingTask()
        {
            var provider = BuildProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Build provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var result = await InvokeTaskProvider(provider.DllPath, GetArgString("post"));
            if (result.ContainsKey("error"))
                return new TaskRunnerResult(result["error"].ToString(), TaskConfig.PostProcessMustSucceed);

            return new TaskRunnerResult(true, "");
        }

        private string GetArgString(string process)
        {
            var dict = new Dictionary<string, object>
            {
                {"process", process},
                {"project", Project.Name},
                {"config", TaskConfig},
                {"additional", AdditionalConfigs}
            };

            return JsonConvert.SerializeObject(dict);
        }
    }
}
