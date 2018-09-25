// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.IO;
using System.Threading.Tasks;
using DotNetCore.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using Xunit;

namespace DotNetCore.Tests
{
    public class BuildProviderTests
    {
        private readonly Mock<ILogger> _logger;

        public BuildProviderTests()
        {
            _logger = new Mock<ILogger>();
        }

        [Fact]
        public async void Build_Success()
        {
            var workingLocation = Path.Combine(AppContext.BaseDirectory, "working", "20180817.1");
            
            if (Directory.Exists(workingLocation))
                Directory.Delete(workingLocation, true);

            Directory.CreateDirectory(workingLocation);

            await GenerateTestProject("MyProject", Path.Combine(workingLocation, "src"));

            var provider = new BuildProvider();

            var taskConfig = new BuildTaskConfig
            {
                SourceLocation = "src",
                OutputArtifactLocation = "artifact",
                WorkingLocation = workingLocation
            };

            var result = await provider.Build("MyProject", taskConfig, null, _logger.Object);

            Assert.Equal("", result.errorMessage);
            Assert.True(File.Exists(result.outputArtifact));
        }

        private async Task GenerateTestProject(string projectName, string outputLocation)
        {
            await CommandHelper.Execute("dotnet", $"new console -n {projectName} -o {outputLocation}");
        }
    }
}
