// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreMvc.Helpers;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace AspNetCoreMvc
{
    public class CodeGenerator
    {
        private readonly string _projectName;
        private readonly string _outputLocation;
        private readonly List<ProjectDataModelDto> _models;

        public CodeGenerator(string projectName, string outputLocation, List<ProjectDataModelDto> models)
        {
            _projectName = projectName;
            _outputLocation = outputLocation;
            _models = models;
        }

        public async Task<string> InitProject()
        {
            var args = $"new mvc -n {_projectName} -o {_outputLocation}";
            return await RunDotnet(args);
        }

        public Task<string> GenerateModels()
        {
            return Task.FromResult("");
        }

        public Task<string> GenerateDbContext()
        {
            return Task.FromResult("");
        }

        public Task<string> GenerateControllers()
        {
            return Task.FromResult("");
        }

        public Task<string> GenerateViews()
        {
            return Task.FromResult("");
        }

        private async Task<string> RunDotnet(string args)
        {
            return await CommandHelper.Execute("dotnet", args);
        }
    }
}
