// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;

namespace DotNetCoreBuildProvider
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

        public Task<string> BeforeBuild(BuildTaskConfig config)
        {
            return Task.FromResult("");
        }

        public async Task<(string returnValue, string errorMessage)> Build(string projectName, string jobQueueCode, BuildTaskConfig config)
        {
            var csprojLocation = Path.Combine(config.WorkingLocation, config.CsprojLocation);
            var testCsprojLocation = Path.Combine(config.WorkingLocation, config.TestCsprojLocation);
            var buildOutputLocation = Path.Combine(config.WorkingLocation, projectName, jobQueueCode, "publish");
            var artifactLocation = Path.Combine(config.WorkingLocation, projectName, jobQueueCode, "artifact");

            if (_builder == null)
                _builder = new Builder();

            var error = await _builder.Build(csprojLocation, buildOutputLocation);
            if (!string.IsNullOrEmpty(error))
                return ("", error);

            if (!string.IsNullOrEmpty(testCsprojLocation))
            {
                error = await _builder.Test(testCsprojLocation);
                if (!string.IsNullOrEmpty(error))
                    return ("", error);
            }

            error = await _builder.CreateArtifact(buildOutputLocation, artifactLocation);
            if (!string.IsNullOrEmpty(error))
                return ("", error);

            return (artifactLocation, "");
        }

        public Task<string> AfterBuild(BuildTaskConfig config)
        {
            return Task.FromResult("");
        }
    }
}
