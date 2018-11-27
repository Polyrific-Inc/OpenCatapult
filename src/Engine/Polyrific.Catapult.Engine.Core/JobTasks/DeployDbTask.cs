﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

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
    public class DeployDbTask : BaseJobTask<DeployDbTaskConfig>, IDeployDbTask
    {
        public DeployDbTask(IProjectService projectService, IExternalServiceService externalServiceService, IPluginManager pluginManager, ILogger<DeployDbTask> logger) 
            : base(projectService, externalServiceService, pluginManager, logger)
        {
        }

        public override string Type => JobTaskDefinitionType.DeployDb;

        public List<PluginItem> DatabaseProviders => PluginManager.GetPlugins(PluginType.DatabaseProvider);

        public override async Task<TaskRunnerResult> RunPreprocessingTask()
        {
            var provider = DatabaseProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Database provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var result = await PluginManager.InvokeTaskProvider(provider.DllPath, GetArgString("pre"));
            if (result.ContainsKey("error"))
                return new TaskRunnerResult(result["error"].ToString(), TaskConfig.PreProcessMustSucceed);
            
            return new TaskRunnerResult(true, "");
        }

        public override async Task<TaskRunnerResult> RunMainTask(Dictionary<string, string> previousTasksOutputValues)
        {
            var provider = DatabaseProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Database provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var result = await PluginManager.InvokeTaskProvider(provider.DllPath, GetArgString("main"));
            if (result.ContainsKey("errorMessage") && !string.IsNullOrEmpty(result["errorMessage"].ToString()))
                return new TaskRunnerResult(result["errorMessage"].ToString(), !TaskConfig.ContinueWhenError);

            var databaseLocation = "";
            if (result.ContainsKey("databaseLocation"))
                databaseLocation = result["databaseLocation"].ToString();

            var outputValues = new Dictionary<string, string>();
            if (result.ContainsKey("outputValues") && !string.IsNullOrEmpty(result["outputValues"]?.ToString()))
                outputValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(result["outputValues"].ToString());

            return new TaskRunnerResult(true, databaseLocation, outputValues);
        }

        public override async Task<TaskRunnerResult> RunPostprocessingTask()
        {
            var provider = DatabaseProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Database provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var result = await PluginManager.InvokeTaskProvider(provider.DllPath, GetArgString("post"));
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
