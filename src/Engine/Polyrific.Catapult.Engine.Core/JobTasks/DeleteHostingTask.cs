// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Service;
using Polyrific.Catapult.TaskProviders.Core.Configs;

namespace Polyrific.Catapult.Engine.Core.JobTasks
{
    public class DeleteHostingTask : BaseJobTask<BaseJobTaskConfig>, IDeleteHostingTask
    {
        /// <inheritdoc />
        public DeleteHostingTask(
            IProjectService projectService, IExternalServiceService externalServiceService, 
            IExternalServiceTypeService externalServiceTypeService, IProviderService providerService, 
            IPluginManager pluginManager, ILogger<DeleteHostingTask> logger)
            : base(projectService, externalServiceService, externalServiceTypeService, providerService, pluginManager, logger)
        {
        }

        public override string Type => JobTaskDefinitionType.DeleteHosting;

        public List<PluginItem> HostingProviders => PluginManager.GetPlugins(TaskProviderType.HostingProvider);

        public override async Task<TaskRunnerResult> RunMainTask(Dictionary<string, string> previousTasksOutputValues)
        {
            var provider = HostingProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Deploy provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var arg = GetArgString();
            var result = await PluginManager.InvokeTaskProvider(provider.StartFilePath, arg.argString, arg.securedArgString);
            if (result.ContainsKey("errorMessage") && !string.IsNullOrEmpty(result["errorMessage"].ToString()))
                return new TaskRunnerResult(result["errorMessage"].ToString(), !TaskConfig.ContinueWhenError);

            var hostLocation = "";
            var taskRemarks = "";
            if (result.ContainsKey("deletedHostLocation"))
            {
                hostLocation = result["deletedHostLocation"].ToString();
                taskRemarks = $"The hosting url {hostLocation} has been deleted";
            }

            var outputValues = new Dictionary<string, string>();
            if (result.ContainsKey("outputValues") && !string.IsNullOrEmpty(result["outputValues"]?.ToString()))
                outputValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(result["outputValues"].ToString());

            return new TaskRunnerResult(true, hostLocation, outputValues)
            {
                TaskRemarks = taskRemarks
            };
        }

        private (string argString, string securedArgString) GetArgString()
        {
            var dict = new Dictionary<string, object>
            {
                {"process", "delete"},
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
