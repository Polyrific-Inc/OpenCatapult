// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.TaskProviders.Core;

namespace Polyrific.Catapult.Plugins.AspNetCoreMvc
{
    public class Program : CodeGeneratorProvider
    {
        private const string TaskProviderName = "Polyrific.Catapult.Plugins.AspNetCoreMvc";

        public override string Name => TaskProviderName;

        public Program(string[] args) : base(args)
        {
        }
        
        public override async Task<(string outputLocation, Dictionary<string, string> outputValues, string errorMessage)> Generate()
        {
            AdditionalConfigs.TryGetValue("AdminEmail", out var adminEmail);

            Config.OutputLocation = Config.OutputLocation ?? Config.WorkingLocation;

            var generator = new CodeGenerator(ProjectName, Config.OutputLocation, Models, adminEmail, Logger);

            await generator.InitSolution();

            await generator.InitProjects();

            await generator.GenerateModels();

            await generator.GenerateDbContext();

            await generator.GenerateRepositories();

            await generator.GenerateServices();

            await generator.GenerateControllers();

            await generator.GenerateViews();

            await generator.UpdateMigrationScript();

            await generator.GenerateRepositoryFiles();

            return (Config.OutputLocation, null, "");
        }

        private static async Task Main(string[] args)
        {
            var app = new Program(args);
            
            var result = await app.Execute();
            app.ReturnOutput(result);
        }
    }
}
