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
    public class PublishArtifactTask : BaseJobTask<PublishArtifactTaskConfig>, IPublishArtifactTask
    {
        /// <inheritdoc />
        public PublishArtifactTask(IProjectService projectService, IExternalServiceService externalServiceService, IExternalServiceTypeService externalServiceTypeService, IPluginService pluginService, IPluginManager pluginManager, ILogger<PublishArtifactTask> logger)
            : base(projectService, externalServiceService, externalServiceTypeService, pluginService, pluginManager, logger)
        {
        }

        public override string Type => JobTaskDefinitionType.PublishArtifact;

        public List<PluginItem> StorageProviders => PluginManager.GetPlugins(PluginType.StorageProvider);

        public override async Task<TaskRunnerResult> RunPreprocessingTask()
        {
            var provider = StorageProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Storage provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var arg = GetArgString("pre");
            var result = await PluginManager.InvokeTaskProvider(provider.DllPath, arg.argString, arg.securedArgString);
            if (result.ContainsKey("error"))
                return new TaskRunnerResult(result["error"].ToString(), TaskConfig.PreProcessMustSucceed);
            
            return new TaskRunnerResult(true, "");
        }

        public override async Task<TaskRunnerResult> RunMainTask(Dictionary<string, string> previousTasksOutputValues)
        {
            var provider = StorageProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Storage provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var arg = GetArgString("main");
            var result = await PluginManager.InvokeTaskProvider(provider.DllPath, arg.argString, arg.securedArgString);
            if (result.ContainsKey("errorMessage") && !string.IsNullOrEmpty(result["errorMessage"].ToString()))
                return new TaskRunnerResult(result["errorMessage"].ToString(), !TaskConfig.ContinueWhenError);

            var storageLocation = "";
            if (result.ContainsKey("storageLocation"))
                storageLocation = result["storageLocation"].ToString();

            var outputValues = new Dictionary<string, string>();
            if (result.ContainsKey("outputValues") && !string.IsNullOrEmpty(result["outputValues"]?.ToString()))
                outputValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(result["outputValues"].ToString());

            return new TaskRunnerResult(true, storageLocation, outputValues);
        }

        public override async Task<TaskRunnerResult> RunPostprocessingTask()
        {
            var provider = StorageProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Storage provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var arg = GetArgString("post");
            var result = await PluginManager.InvokeTaskProvider(provider.DllPath, arg.argString, arg.securedArgString);
            if (result.ContainsKey("error"))
                return new TaskRunnerResult(result["error"].ToString(), TaskConfig.PostProcessMustSucceed);
            
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
