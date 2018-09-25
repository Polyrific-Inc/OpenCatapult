// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreMvc.Helpers;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace AspNetCoreMvc.ProjectGenerators
{
    internal class InfrastructureProjectGenerator
    {
        private string _projectName;
        private readonly ProjectHelper _projectHelper;
        private readonly List<ProjectDataModelDto> _models;

        public const string InfrastructureProject = "Infrastructure";

        public InfrastructureProjectGenerator(string projectName, ProjectHelper projectHelper, List<ProjectDataModelDto> models)
        {
            _projectName = projectName;
            _projectHelper = projectHelper;
            _models = models;
        }

        public async Task<string> Initialize()
        {
            var infrastructureProjectReferences = new string[]
            {
                _projectHelper.GetProjectFullPath($"{_projectName}.{CoreProjectGenerator.CoreProject}"),
                _projectHelper.GetProjectFullPath($"{_projectName}.{DataProjectGenerator.DataProject}")
            };

            return await _projectHelper.CreateProject($"{_projectName}.{InfrastructureProject}", "classlib", infrastructureProjectReferences);
        }

        public Task<string> GenerateDbContextInjection()
        {
            return Task.FromResult("");
        }

        public Task<string> GenerateRepositoryInjection()
        {
            return Task.FromResult("");
        }
    }
}
