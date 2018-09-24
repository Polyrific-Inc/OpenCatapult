// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;

namespace DotNetCore
{
    [Export(typeof(IBuildProvider))]
    public class BuildProvider : IBuildProvider
    {
        private IBuilder _builder;

        public BuildProvider()
        {
            
        }

        public BuildProvider(IBuilder builder)
        {
            _builder = builder;
        }

        public string Name => "DotNetCoreBuildProvider";

        public string[] RequiredServices => new string[0];

        public Task<string> BeforeBuild(string projectName, BuildTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult("");
        }

        public async Task<(string outputLocation, Dictionary<string, string> outputValues, string errorMessage)> Build(string projectName, BuildTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            var csprojLocation = Path.Combine(config.SourceLocation, $"{projectName}.csproj");
            if (additionalConfigs.ContainsKey("CsprojLocation"))
                csprojLocation = additionalConfigs["CsprojLocation"];

            var buildConfiguration = "Release";
            if (additionalConfigs.ContainsKey("Configuration"))
                buildConfiguration = additionalConfigs["Configuration"];

            var buildOutputLocation = Path.Combine(csprojLocation, $"bin/{buildConfiguration}");

            var artifactLocation = Path.Combine(csprojLocation, $"bin/{buildConfiguration}", "artifact");
            if (!string.IsNullOrEmpty(config.OutputArtifactLocation))
                artifactLocation = config.OutputArtifactLocation;

            if (_builder == null)
                _builder = new Builder();

            var error = await _builder.Build(csprojLocation, buildOutputLocation);
            if (!string.IsNullOrEmpty(error))
                return ("", null, error);
            
            error = await _builder.CreateArtifact(buildOutputLocation, artifactLocation);
            if (!string.IsNullOrEmpty(error))
                return ("", null, error);

            return (artifactLocation, null, "");
        }

        public Task<string> AfterBuild(string projectName, BuildTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult("");
        }
    }
}
