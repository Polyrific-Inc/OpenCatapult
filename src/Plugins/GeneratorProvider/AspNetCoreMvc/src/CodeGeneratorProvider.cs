// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace AspNetCoreMvc
{
    [Export(typeof(ICodeGeneratorProvider))]
    public class CodeGeneratorProvider : ICodeGeneratorProvider
    {
        public string Name => "AspNetCoreMvc";

        public string[] RequiredServices => new string[0];

        public Task<string> BeforeGenerate(string projectName, List<ProjectDataModelDto> models, GenerateTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult("");
        }

        public async Task<(string outputLocation, Dictionary<string, string> outputValues, string errorMessage)> Generate(string projectName, List<ProjectDataModelDto> models, GenerateTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            additionalConfigs.TryGetValue("AdminEmail", out var adminEmail);

            config.OutputLocation = config.OutputLocation ?? config.WorkingLocation;

            var generator = new CodeGenerator(projectName, config.OutputLocation, models, adminEmail, logger);

            await generator.InitSolution();

            await generator.InitProjects();

            await generator.GenerateModels();

            await generator.GenerateDbContext();

            await generator.GenerateRepositories();

            await generator.GenerateServices();

            await generator.GenerateControllers();

            await generator.GenerateViews();

            await generator.UpdateMigrationScript();

            return (config.OutputLocation, null, "");
        }

        public Task<string> AfterGenerate(string projectName, List<ProjectDataModelDto> models, GenerateTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult("");
        }
    }
}
