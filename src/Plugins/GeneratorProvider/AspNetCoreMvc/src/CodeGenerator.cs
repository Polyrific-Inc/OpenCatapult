// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreMvc.Helpers;
using AspNetCoreMvc.ProjectGenerators;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace AspNetCoreMvc
{
    public class CodeGenerator
    {
        private readonly string _projectName;
        private readonly string _outputLocation;
        private readonly List<ProjectDataModelDto> _models;
        private readonly ProjectHelper _projectHelper;
        private readonly CoreProjectGenerator _coreProjectGenerator;
        private readonly DataProjectGenerator _dataProjectGenerator;
        private readonly InfrastructureProjectGenerator _infrastructureProjectGenerator;
        private readonly MainProjectGenerator _mainProjectGenerator;
        
        public CodeGenerator(string projectName, string outputLocation, List<ProjectDataModelDto> models, string connectionString)
        {
            _projectName = projectName;
            _outputLocation = outputLocation;
            _models = models;
            _projectHelper = new ProjectHelper(projectName, outputLocation);
            _coreProjectGenerator = new CoreProjectGenerator(projectName, _projectHelper, models);
            _dataProjectGenerator = new DataProjectGenerator(projectName, _projectHelper, models);
            _infrastructureProjectGenerator = new InfrastructureProjectGenerator(projectName, _projectHelper, models);
            _mainProjectGenerator = new MainProjectGenerator(projectName, _projectHelper, models, connectionString);
        }

        public async Task<string> InitSolution()
        {
            var args = $"new sln -n {_projectName} -o {_outputLocation}";
            return await CommandHelper.RunDotnet(args);
        }

        public async Task<string> InitProjects()
        {
            var sb = new StringBuilder();

            sb.AppendLine(await _coreProjectGenerator.Initialize());
            sb.AppendLine(await _dataProjectGenerator.Initialize());
            sb.AppendLine(await _infrastructureProjectGenerator.Initialize());
            sb.AppendLine(await _mainProjectGenerator.Initialize());

            return sb.ToString();
        }

        public async Task<string> GenerateModels()
        {
            var sb = new StringBuilder();

            sb.AppendLine(await _coreProjectGenerator.GenerateModels());
            sb.AppendLine(await _mainProjectGenerator.GenerateViewModels());

            return sb.ToString();
        }

        public async Task<string> GenerateDbContext()
        {
            var sb = new StringBuilder();

            sb.AppendLine(await _dataProjectGenerator.GenerateEntityConfigs());
            sb.AppendLine(await _dataProjectGenerator.GenerateDbContext());
            sb.AppendLine(await _infrastructureProjectGenerator.GenerateDbContextInjection());

            return sb.ToString();
        }

        public async Task<string> GenerateRepositories()
        {
            var sb = new StringBuilder();

            sb.AppendLine(await _coreProjectGenerator.GenerateRepositoryInterface());
            sb.AppendLine(await _dataProjectGenerator.GenerateRepositoryClass());
            sb.AppendLine(await _infrastructureProjectGenerator.GenerateRepositoryInjection());

            return sb.ToString();
        }

        public async Task<string> GenerateServices()
        {
            var sb = new StringBuilder();

            sb.AppendLine(await _coreProjectGenerator.GenerateServices());
            sb.AppendLine(await _mainProjectGenerator.GenerateServiceInjection());
            sb.AppendLine(await _mainProjectGenerator.GenerateStartupClass());
            sb.AppendLine(await _mainProjectGenerator.GenerateProgramClass());

            return sb.ToString();
        }

        public Task<string> GenerateControllers()
        {
            return _mainProjectGenerator.GenerateControllers();
        }

        public Task<string> GenerateViews()
        {
            return _mainProjectGenerator.GenerateViews();
        }

        public Task<string> UpdateMigrationScript()
        {
            return _mainProjectGenerator.UpdateMigrationScript();
        }
    }
}
