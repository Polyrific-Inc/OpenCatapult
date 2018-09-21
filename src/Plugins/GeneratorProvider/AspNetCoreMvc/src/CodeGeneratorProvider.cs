// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
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

        public Task<string> BeforeGenerate(string projectName, List<ProjectDataModelDto> models, string outputFolderName, GenerateTaskConfig config)
        {
            return Task.FromResult("");
        }

        public async Task<(string outputLocation, string errorMessage)> Generate(string projectName, List<ProjectDataModelDto> models, string outputFolderName, GenerateTaskConfig config)
        {
            var outputLocation = Path.Combine(config.WorkingLocation, outputFolderName);

            var generator = new CodeGenerator(projectName, outputLocation, models);

            await generator.InitProject();

            await generator.GenerateModels();

            await generator.GenerateDbContext();

            await generator.GenerateControllers();

            await generator.GenerateViews();
            
            return (outputLocation, "");
        }

        public Task<string> AfterGenerate(string projectName, List<ProjectDataModelDto> models, string outputFolderName, GenerateTaskConfig config)
        {
            return Task.FromResult("");
        }
    }
}
