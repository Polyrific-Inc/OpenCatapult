// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreMvc.Helpers;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace AspNetCoreMvc.ProjectGenerators
{
    internal class DataProjectGenerator
    {
        private string _projectName;
        private readonly ProjectHelper _projectHelper;
        private readonly List<ProjectDataModelDto> _models;

        public const string DataProject = "Data";

        public DataProjectGenerator(string projectName, ProjectHelper projectHelper, List<ProjectDataModelDto> models)
        {
            _projectName = projectName;
            _projectHelper = projectHelper;
            _models = models;
        }

        public async Task<string> Initialize()
        {
            var dataProjectReferences = new string[]
            {
                _projectHelper.GetProjectFullPath($"{_projectName}.{CoreProjectGenerator.CoreProject}")
            };
            var dataProjectPackages = new (string, string)[]
            {
                ("Microsoft.EntityFrameworkCore.SqlServer", "2.1.1"),
                ("Microsoft.EntityFrameworkCore.Tools", "2.1.1")
            };

            return await _projectHelper.CreateProject($"{_projectName}.{DataProject}", "classlib", dataProjectReferences, dataProjectPackages);
        }

        public Task<string> GenerateDbContext()
        {
            return Task.FromResult("");
        }

        public Task<string> GenerateRepositoryClass()
        {
            return Task.FromResult("");
        }
    }
}
