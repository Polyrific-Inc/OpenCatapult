// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.TaskProviders.AspNetCoreMvc.Helpers;
using Polyrific.Catapult.TaskProviders.AspNetCoreMvc.ProjectGenerators;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace Polyrific.Catapult.TaskProviders.AspNetCoreMvc
{
    public class CodeGenerator
    {
        private readonly string _projectName;
        private readonly string _outputLocation;
        private readonly string _adminEmail;
        private readonly List<ProjectDataModelDto> _models;
        private readonly ProjectHelper _projectHelper;
        private readonly CoreProjectGenerator _coreProjectGenerator;
        private readonly DataProjectGenerator _dataProjectGenerator;
        private readonly InfrastructureProjectGenerator _infrastructureProjectGenerator;
        private readonly MainProjectGenerator _mainProjectGenerator;
        private readonly ILogger _logger;
               
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public CodeGenerator(string projectName, string outputLocation, List<ProjectDataModelDto> models,
            string adminEmail, ILogger logger = null)
        {
            _projectName = TextHelper.Pascalize(projectName.Replace("-", "_"));
            _outputLocation = outputLocation;
            _adminEmail = adminEmail;
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
            if (File.Exists(Path.Combine(_outputLocation, $"{_projectName}.sln")))
                return "";

            var args = $"new sln -n {_projectName} -o \"{_outputLocation}\"";
            return await CommandHelper.RunDotnet(args);
        }

        public async Task<string> InitProjects()
        {
            var sb = new StringBuilder();

            // add main project first so it set as start up project
            sb.AppendLine(await _mainProjectGenerator.Initialize());
            sb.AppendLine(await _coreProjectGenerator.Initialize());
            sb.AppendLine(await _dataProjectGenerator.Initialize());
            sb.AppendLine(await _infrastructureProjectGenerator.Initialize());
            await _mainProjectGenerator.AddProjectReferences();

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

        public async Task GenerateRepositoryFiles()
        {
            var readmeFile = Path.Combine(_outputLocation, "README.md");

            // generate readme if not exist
            if (!File.Exists(readmeFile))
            {
                var sourceFile = Path.Combine(AssemblyDirectory, "Resources/README.md");
                var readmeContent = await File.ReadAllTextAsync(sourceFile);
                readmeContent = readmeContent.Replace("{{ProjectName}}", _projectName);
                readmeContent = readmeContent.Replace("{{AdminEmail}}", _adminEmail);
                await File.WriteAllTextAsync(readmeFile, readmeContent);
            }

            var gitignoreFile = Path.Combine(_outputLocation, ".gitignore");

            if (!File.Exists(gitignoreFile))
            {
                var sourceFile = Path.Combine(AssemblyDirectory, "Resources/.gitignore");
                File.Copy(sourceFile, gitignoreFile);
            }
                
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
