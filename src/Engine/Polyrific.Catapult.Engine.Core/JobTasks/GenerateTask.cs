// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Service;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Engine.Core.JobTasks
{
    public class GenerateTask : BaseJobTask<GenerateTaskConfig>, IGenerateTask
    {
        private readonly IProjectService _projectService;
        private readonly IProjectDataModelService _dataModelService;
        
        /// <summary>
        /// Instantiate <see cref="GenerateTask"/>
        /// </summary>
        /// <param name="projectService">Instance of <see cref="IProjectService"/></param>
        /// <param name="dataModelService">Instance of <see cref="IProjectDataModelService"/></param>
        /// <param name="logger">Logger</param>
        public GenerateTask(IProjectService projectService, IProjectDataModelService dataModelService, ILogger<GenerateTask> logger) 
            : base(logger)
        {
            _projectService = projectService;
            _dataModelService = dataModelService;
        }

        public override string Type => JobTaskDefinitionType.Generate;

        [ImportMany(typeof(ICodeGeneratorProvider))]
        public IEnumerable<ICodeGeneratorProvider> GeneratorProviders;
        
        public override async Task<TaskRunnerResult> RunMainTask()
        {
            var provider = GeneratorProviders?.FirstOrDefault(p => p.Name == TaskConfig.ProviderName);
            if (provider == null)
                return new TaskRunnerResult($"Code generator provider \"{TaskConfig.ProviderName}\" could not be found.");

            var project = await _projectService.GetProject(ProjectId);
            var models = await _dataModelService.GetProjectDataModels(ProjectId);

            var (outputLocation, errorMessage) = await provider.Generate(project.Name, models, JobQueueCode, TaskConfig);
            if (!string.IsNullOrEmpty(errorMessage))
                return new TaskRunnerResult(errorMessage, !TaskConfig.ContinueWhenError);

            return new TaskRunnerResult(true, outputLocation);
        }
    }
}