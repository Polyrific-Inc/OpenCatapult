// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polyrific.Catapult.Plugins.Core.Configs;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Engine.Core.JobTasks
{
    public class GenerateTask : BaseJobTask<GenerateTaskConfig>, IGenerateTask
    {
        private readonly IProjectDataModelService _dataModelService;

        /// <summary>
        /// Instantiate <see cref="GenerateTask"/>
        /// </summary>
        /// <param name="projectService">Instance of <see cref="IProjectService"/></param>
        /// <param name="externalServiceService">Instance of <see cref="IExternalServiceService"/></param>
        /// <param name="externalServiceTypeService">Instance of <see cref="IExternalServiceTypeService"/></param>
        /// <param name="IProviderService">Instance of <see cref="IProviderService"/></param>
        /// <param name="dataModelService">Instance of <see cref="IProjectDataModelService"/></param>
        /// <param name="pluginManager">Instance of <see cref="IPluginManager"/></param>
        /// <param name="logger">Logger</param>
        public GenerateTask(IProjectService projectService, IExternalServiceService externalServiceService, IExternalServiceTypeService externalServiceTypeService, IProviderService providerService, IProjectDataModelService dataModelService, IPluginManager pluginManager, ILogger<GenerateTask> logger) 
            : base(projectService, externalServiceService, externalServiceTypeService, providerService, pluginManager, logger)
        {
            _dataModelService = dataModelService;
        }

        public override string Type => JobTaskDefinitionType.Generate;

        private List<ProjectDataModelDto> _dataModels;
        protected List<ProjectDataModelDto> DataModels => _dataModels ?? (_dataModels = _dataModelService.GetProjectDataModels(ProjectId, true).Result);

        public List<PluginItem> GeneratorProviders => PluginManager.GetPlugins(PluginType.GeneratorProvider);

        public override async Task<TaskRunnerResult> RunPreprocessingTask()
        {
            var provider = GeneratorProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Code generator provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var arg = GetArgString("pre");
            var result = await PluginManager.InvokeTaskProvider(provider.StartFilePath, arg.argString, arg.securedArgString);
            if (result.ContainsKey("errorMessage"))
                return new TaskRunnerResult(result["errorMessage"].ToString(), TaskConfig.PreProcessMustSucceed);
            
            return new TaskRunnerResult(true, "");
        }

        public override async Task<TaskRunnerResult> RunMainTask(Dictionary<string, string> previousTasksOutputValues)
        {
            var provider = GeneratorProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Code generator provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var arg = GetArgString("main");
            var result = await PluginManager.InvokeTaskProvider(provider.StartFilePath, arg.argString, arg.securedArgString);
            if (result.ContainsKey("errorMessage") && !string.IsNullOrEmpty(result["errorMessage"].ToString()))
                return new TaskRunnerResult(result["errorMessage"].ToString(), !TaskConfig.ContinueWhenError);

            var outputLocation = "";
            var taskRemarks = "";
            if (result.ContainsKey("outputLocation"))
            {
                outputLocation = result["outputLocation"].ToString();
                taskRemarks = $"The generated code is located at {outputLocation}";
            }
            
            var outputValues = new Dictionary<string, string>();
            if (result.ContainsKey("outputValues"))
                outputValues = result["outputValues"] as Dictionary<string, string>;
            
            return new TaskRunnerResult(true, outputLocation, outputValues)
            {
                TaskRemarks = taskRemarks
            };
        }

        public override async Task<TaskRunnerResult> RunPostprocessingTask()
        {
            var provider = GeneratorProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Code generator provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var arg = GetArgString("post");
            var result = await PluginManager.InvokeTaskProvider(provider.StartFilePath, arg.argString, arg.securedArgString);
            if (result.ContainsKey("errorMessage"))
                return new TaskRunnerResult(result["errorMessage"].ToString(), TaskConfig.PostProcessMustSucceed);

            return new TaskRunnerResult(true, "");
        }

        public override void ReloadProperties()
        {
            base.ReloadProperties();
            _dataModels = null;
        }

        private (string argString, string securedArgString) GetArgString(string process)
        {
            var dict = new Dictionary<string, object>
            {
                {"process", process},
                {"project", Project.Name},
                {"models", DataModels},
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
