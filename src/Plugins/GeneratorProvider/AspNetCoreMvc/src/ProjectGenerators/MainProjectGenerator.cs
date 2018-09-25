// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreMvc.Helpers;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace AspNetCoreMvc.ProjectGenerators
{
    internal class MainProjectGenerator
    {
        private string _projectName;
        private readonly ProjectHelper _projectHelper;
        private readonly List<ProjectDataModelDto> _models;

        public const string MainProject = "Main";

        public MainProjectGenerator(string projectName, ProjectHelper projectHelper, List<ProjectDataModelDto> models)
        {
            _projectName = projectName;
            _projectHelper = projectHelper;
            _models = models;
        }

        public async Task<string> Initialize()
        {
            var mainProjectReferences = new string[]
            {
                _projectHelper.GetProjectFullPath($"{_projectName}.{CoreProjectGenerator.CoreProject}"),
                _projectHelper.GetProjectFullPath($"{_projectName}.{InfrastructureProjectGenerator.InfrastructureProject}")
            };
            return await _projectHelper.CreateProject($"{_projectName}", "mvc", mainProjectReferences);
        }

        public Task<string> GenerateControllers()
        {
            return Task.FromResult("");
        }

        public Task<string> GenerateViews()
        {
            return Task.FromResult("");
        }

        public Task<string> GenerateServiceInjection()
        {
            return Task.FromResult("");
        }

        public Task<string> GenerateRepositoryInjection()
        {
            return Task.FromResult("");
        }
    }
}
