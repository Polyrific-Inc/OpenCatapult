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
    public class DeleteRepositoryTask : BaseJobTask<DeleteRepositoryTaskConfig>, IDeleteRepositoryTask
    {
        /// <inheritdoc />
        public DeleteRepositoryTask(
            IProjectService projectService, IExternalServiceService externalServiceService, 
            IExternalServiceTypeService externalServiceTypeService, IProviderService providerService, 
            ITaskProviderManager taskProviderManager, ILogger<DeleteRepositoryTask> logger)
            : base(projectService, externalServiceService, externalServiceTypeService, providerService, taskProviderManager, logger)
        {
        }

        public override string Type => JobTaskDefinitionType.DeleteRepository;

        public List<TaskProviderItem> CodeRepositoryProviders => TaskProviderManager.GetTaskProviders(TaskProviderType.RepositoryProvider);
        
        public override async Task<TaskRunnerResult> RunMainTask(Dictionary<string, string> previousTasksOutputValues)
        {
            var provider = CodeRepositoryProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Code repository provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var arg = GetArgString();
            var result = await TaskProviderManager.InvokeTaskProvider(provider.StartFilePath, arg.argString, arg.securedArgString);
            if (result.ContainsKey("errorMessage") && !string.IsNullOrEmpty(result["errorMessage"].ToString()))
                return new TaskRunnerResult(result["errorMessage"].ToString(), !TaskConfig.ContinueWhenError);

            var outputValues = new Dictionary<string, string>();
            if (result.ContainsKey("outputValues") && !string.IsNullOrEmpty(result["outputValues"]?.ToString()))
                outputValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(result["outputValues"].ToString());

            return new TaskRunnerResult(true, "", outputValues)
            {
                TaskRemarks = $"The remote repository \"{TaskConfig.Repository}\" has been deleted"
            };
        }

        private (string argString, string securedArgString) GetArgString()
        {
            var dict = new Dictionary<string, object>
            {
                {"process", "delete"},
                {"project", Project.Name},
                {"deleteconfig", TaskConfig},
                {"additional", AdditionalConfigs}
            };

            var argString = JsonConvert.SerializeObject(dict);

            dict["additional"] = SecuredAdditionalConfigs;
            var securedArgString = JsonConvert.SerializeObject(dict);

            return (argString, securedArgString);
        }
    }
}
