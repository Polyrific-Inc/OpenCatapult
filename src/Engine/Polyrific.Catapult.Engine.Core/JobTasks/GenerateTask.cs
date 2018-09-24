﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
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
        /// <param name="dataModelService">Instance of <see cref="IProjectDataModelService"/></param>
        /// <param name="logger">Logger</param>
        public GenerateTask(IProjectService projectService, IProjectDataModelService dataModelService, ILogger<GenerateTask> logger) 
            : base(projectService, logger)
        {
            _dataModelService = dataModelService;
        }

        public override string Type => JobTaskDefinitionType.Generate;

        private List<ProjectDataModelDto> _dataModels;
        protected List<ProjectDataModelDto> DataModels => _dataModels ?? (_dataModels = _dataModelService.GetProjectDataModels(ProjectId).Result);

        [ImportMany(typeof(ICodeGeneratorProvider))]
        public IEnumerable<ICodeGeneratorProvider> GeneratorProviders;

        public override async Task<TaskRunnerResult> RunPreprocessingTask()
        {
            var provider = GeneratorProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Code generator provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var error = await provider.BeforeGenerate(Project.Name, DataModels, TaskConfig, AdditionalConfigs, Logger);
            if (!string.IsNullOrEmpty(error))
                return new TaskRunnerResult(error, TaskConfig.PreProcessMustSucceed);

            return new TaskRunnerResult(true, "");
        }

        public override async Task<TaskRunnerResult> RunMainTask()
        {
            var provider = GeneratorProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Code generator provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);
            
            var (outputLocation, errorMessage) = await provider.Generate(Project.Name, DataModels, TaskConfig, AdditionalConfigs, Logger);
            if (!string.IsNullOrEmpty(errorMessage))
                return new TaskRunnerResult(errorMessage, !TaskConfig.ContinueWhenError);

            return new TaskRunnerResult(true, outputLocation);
        }

        public override async Task<TaskRunnerResult> RunPostprocessingTask()
        {
            var provider = GeneratorProviders?.FirstOrDefault(p => p.Name == Provider);
            if (provider == null)
                return new TaskRunnerResult($"Code generator provider \"{Provider}\" could not be found.");

            await LoadRequiredServicesToAdditionalConfigs(provider.RequiredServices);

            var error = await provider.AfterGenerate(Project.Name, DataModels, TaskConfig, AdditionalConfigs, Logger);
            if (!string.IsNullOrEmpty(error))
                return new TaskRunnerResult(error, TaskConfig.PostProcessMustSucceed);

            return new TaskRunnerResult(true, "");
        }
    }
}
