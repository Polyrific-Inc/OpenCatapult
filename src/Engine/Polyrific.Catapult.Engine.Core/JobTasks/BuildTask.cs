// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polyrific.Catapult.Plugins.Core.Configs;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Engine.Core.JobTasks
{
    public class BuildTask : BaseJobTask<BuildTaskConfig>, IBuildTask
    {
        /// <inheritdoc />
        public BuildTask(IProjectService projectService, IExternalServiceService externalServiceService, IExternalServiceTypeService externalServiceTypeService, IProviderService providerService, IPluginManager pluginManager, ILogger<BuildTask> logger) 
            : base(projectService, externalServiceService, externalServiceTypeService, providerService, pluginManager, logger)
        {
            
        }

        public override string Type => JobTaskDefinitionType.Build;

        public List<PluginItem> BuildProviders => PluginManager.GetPlugins(PluginType.BuildProvider);

        public override async Task<TaskRunnerResult> RunPreprocessingTask()
        {
            var provider = BuildProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Build provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var arg = GetArgString("pre");
            var result = await PluginManager.InvokeTaskProvider(provider.StartFilePath, arg.argString, arg.securedArgString);
            if (result.ContainsKey("errorMessage"))
                return new TaskRunnerResult(result["errorMessage"].ToString(), TaskConfig.PreProcessMustSucceed);

            return new TaskRunnerResult(true, "");
        }

        public override async Task<TaskRunnerResult> RunMainTask(Dictionary<string, string> previousTasksOutputValues)
        {
            var provider = BuildProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Build provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var arg = GetArgString("main");
            var result = await PluginManager.InvokeTaskProvider(provider.StartFilePath, arg.argString, arg.securedArgString);
            if (result.ContainsKey("errorMessage") && !string.IsNullOrEmpty(result["errorMessage"].ToString()))
                return new TaskRunnerResult(result["errorMessage"].ToString(), !TaskConfig.ContinueWhenError);

            var outputArtifact = "";
            var taskRemarks = "";
            if (result.ContainsKey("outputArtifact"))
            {
                outputArtifact = result["outputArtifact"].ToString();
                taskRemarks = $"The build artifact is located at {outputArtifact}";
            }

            var outputValues = new Dictionary<string, string>();
            if (result.ContainsKey("outputValues") && !string.IsNullOrEmpty(result["outputValues"]?.ToString()))
                outputValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(result["outputValues"].ToString());
            
            return new TaskRunnerResult(true, outputArtifact, outputValues)
            {
                TaskRemarks = taskRemarks
            };
        }

        public override async Task<TaskRunnerResult> RunPostprocessingTask()
        {
            var provider = BuildProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Build provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var arg = GetArgString("post");
            var result = await PluginManager.InvokeTaskProvider(provider.StartFilePath, arg.argString, arg.securedArgString);
            if (result.ContainsKey("errorMessage"))
                return new TaskRunnerResult(result["errorMessage"].ToString(), TaskConfig.PostProcessMustSucceed);

            return new TaskRunnerResult(true, "");
        }

        private (string argString, string securedArgString) GetArgString(string process)
        {
            var dict = new Dictionary<string, object>
            {
                {"process", process},
                {"project", Project.Name},
                {"config", TaskConfig},
                {"additional", AdditionalConfigs}
            };

            var argString = JsonConvert.SerializeObject(dict);

            dict["additional"] = SecuredAdditionalConfigs;
            var securedArgString = JsonConvert.SerializeObject(dict);

            return (argString, securedArgString);
        }
    }
}
