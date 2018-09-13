// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using AspNetCoreMvc.Helpers;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace AspNetCoreMvc
{
    [Export(typeof(ICodeGeneratorProvider))]
    public class CodeGeneratorProvider : ICodeGeneratorProvider
    {
        public string Name => "AspNetCoreMvc";

        public async Task<(string outputLocation, string errorMessage)> Generate(string projectName, List<ProjectDataModelDto> models, string outputFolderName, GenerateTaskConfig config)
        {
            if (config.ProviderName != Name)
                return ("", "Provider name doesn't match.");

            var outputLocation = Path.Combine(config.WorkingLocation, outputFolderName);

            var args = $"new mvc -n {projectName} -o {outputLocation}";
            var result = await CommandHelper.Execute("dotnet", args);

            return (outputLocation, "");
        }
    }
}
