// Copyright (c) Polyrific, Inc 2018. All rights reserved.

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
    public class PushTask : BaseJobTask<PushTaskConfig>, IPushTask
    {
        /// <summary>
        /// Instantiate <see cref="PushTask"/>
        /// </summary>
        /// <param name="projectService">Instance of <see cref="IProjectService"/></param>
        /// <param name="externalServiceService">Instance of <see cref="IExternalServiceService"/></param>
        /// <param name="pluginManager"></param>
        /// <param name="logger">Logger</param>
        public PushTask(IProjectService projectService, IExternalServiceService externalServiceService, IPluginManager pluginManager, ILogger<PushTask> logger) 
            : base(projectService, externalServiceService, pluginManager, logger)
        {
        }

        public override string Type => JobTaskDefinitionType.Push;

        public List<PluginItem> CodeRepositoryProviders => PluginManager.GetPlugins(PluginType.RepositoryProvider);

        public override async Task<TaskRunnerResult> RunPreprocessingTask()
        {
            var provider = CodeRepositoryProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Code repository provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var result = await PluginManager.InvokeTaskProvider(provider.DllPath, GetArgString("pre"));
            if (result.ContainsKey("error") && !string.IsNullOrEmpty(result["error"].ToString()))
                return new TaskRunnerResult(result["error"].ToString(), TaskConfig.PreProcessMustSucceed);

            return new TaskRunnerResult(true, "");
        }

        public override async Task<TaskRunnerResult> RunMainTask(Dictionary<string, string> previousTasksOutputValues)
        {
            var provider = CodeRepositoryProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Code repository provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var result = await PluginManager.InvokeTaskProvider(provider.DllPath, GetArgString("main"));
            if (result.ContainsKey("errorMessage") && !string.IsNullOrEmpty(result["errorMessage"].ToString()))
                return new TaskRunnerResult(result["errorMessage"].ToString(), !TaskConfig.ContinueWhenError);

            var remoteUrl = "";
            if (result.ContainsKey("remoteUrl"))
                remoteUrl = result["remoteUrl"].ToString();

            var outputValues = new Dictionary<string, string>();
            if (result.ContainsKey("outputValues"))
                outputValues = result["outputValues"] as Dictionary<string, string>;

            // stop the next process if we're creating a pull request
            return new TaskRunnerResult(true, remoteUrl, outputValues, TaskConfig.CreatePullRequest);
        }

        public override async Task<TaskRunnerResult> RunPostprocessingTask()
        {
            var provider = CodeRepositoryProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Code repository provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var result = await PluginManager.InvokeTaskProvider(provider.DllPath, GetArgString("post"));
            if (result.ContainsKey("error") && !string.IsNullOrEmpty(result["error"].ToString()))
                return new TaskRunnerResult(result["error"].ToString(), TaskConfig.PostProcessMustSucceed);

            return new TaskRunnerResult(true, "");
        }

        private string GetArgString(string process)
        {
            var dict = new Dictionary<string, object>
            {
                {"process", process},
                {"project", Project.Name},
                {"pushconfig", TaskConfig},
                {"additional", AdditionalConfigs}
            };

            return JsonConvert.SerializeObject(dict);
        }
    }
}
