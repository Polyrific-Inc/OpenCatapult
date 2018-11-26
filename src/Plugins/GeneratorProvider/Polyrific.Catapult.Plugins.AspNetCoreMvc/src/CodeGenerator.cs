// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.AspNetCoreMvc.Helpers;
using Polyrific.Catapult.Plugins.AspNetCoreMvc.ProjectGenerators;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace Polyrific.Catapult.Plugins.AspNetCoreMvc
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
        private readonly ILogger _logger;
        
        public CodeGenerator(string projectName, string outputLocation, List<ProjectDataModelDto> models,
            string adminEmail, ILogger logger = null)
        {
            _projectName = TextHelper.Pascalize(projectName.Replace("-", "_"));
            _outputLocation = outputLocation;
            _models = NormalizeModels(models);
            _projectHelper = new ProjectHelper(_projectName, outputLocation, logger);
            _coreProjectGenerator = new CoreProjectGenerator(_projectName, _projectHelper, _models, logger);
            _dataProjectGenerator = new DataProjectGenerator(_projectName, _projectHelper, _models, adminEmail, logger);
            _infrastructureProjectGenerator = new InfrastructureProjectGenerator(_projectName, _projectHelper, _models, logger);
            _mainProjectGenerator = new MainProjectGenerator(_projectName, _projectHelper, _models, logger);
            _logger = logger;
        }

        public async Task<string> InitSolution()
        {
            var args = $"new sln -n {_projectName} -o \"{_outputLocation}\"";
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
            sb.AppendLine(await _dataProjectGenerator.GenerateIdentityClasses());
            sb.AppendLine(await _infrastructureProjectGenerator.GenerateRepositoryInjection());
            sb.AppendLine(await _infrastructureProjectGenerator.GenerateIdentityIjection());

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

        public async Task<string> GenerateControllers()
        {
            var sb = new StringBuilder();

            sb.AppendLine(await _mainProjectGenerator.GenerateControllers());
            sb.AppendLine(await _mainProjectGenerator.AddApplicationIdentity());

            return sb.ToString();
        }

        public Task<string> GenerateViews()
        {
            return _mainProjectGenerator.GenerateViews();
        }

        public Task<string> UpdateMigrationScript()
        {
            return _mainProjectGenerator.UpdateMigrationScript();
        }

        public List<ProjectDataModelDto> NormalizeModels(List<ProjectDataModelDto> models)
        {
            var baseProperties = new string[] { "id", "created", "updated", "concurrencystamp" };
            foreach (var model in models)
            {
                model.Properties = model.Properties.Where(p => !baseProperties.Contains(p.Name.ToLower())).ToList();

                foreach (var property in model.Properties)
                {
                    switch (property.DataType)
                    {
                        case PropertyDataType.DateTime:
                            property.DataType = "DateTime";
                            break;
                        case PropertyDataType.DbGeography:
                            property.DataType = "DbGeography";
                            break;
                    }
                }
            }

            return models;
        }
    }
}
