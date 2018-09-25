// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreMvc.Helpers;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace AspNetCoreMvc.ProjectGenerators
{
    internal class CoreProjectGenerator
    {
        private string _projectName;
        private readonly ProjectHelper _projectHelper;
        private readonly List<ProjectDataModelDto> _models;

        public const string CoreProject = "Core";
        
        public CoreProjectGenerator(string projectName, ProjectHelper projectHelper, List<ProjectDataModelDto> models)
        {
            _projectName = projectName;
            _projectHelper = projectHelper;
            _models = models;
        }

        public async Task<string> Initialize()
        {
            return await _projectHelper.CreateProject($"{_projectName}.{CoreProject}", "classlib");
        }

        public Task<string> GenerateRepositoryInterface()
        {
            return Task.FromResult("");
        }

        public Task<string> GenerateServiceInterface()
        {
            return Task.FromResult("");
        }

        public Task<string> GenerateServiceClass()
        {
            return Task.FromResult("");
        }

        public Task<string> GenerateModels()
        {
            return Task.FromResult("");
        }
    }
}
