// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;
using Xunit;

namespace AspNetCoreMvc.Tests
{
    public class CodeGeneratorProviderTests
    {
        private readonly Mock<ILogger> _logger;

        public CodeGeneratorProviderTests()
        {
            _logger = new Mock<ILogger>();
        }

        [Fact]
        public async void Generate_Success()
        {
            var models = GetSampleDataModels();
            var outputFolderName = "20180817.1";
            var workingLocation = Path.Combine(AppContext.BaseDirectory, "working");
            var outputLocation = Path.Combine(workingLocation, outputFolderName);

            var config = new GenerateTaskConfig
            {
                WorkingLocation = workingLocation,
                OutputLocation = outputLocation
            };
            
            if (Directory.Exists(outputLocation))
                Directory.Delete(outputLocation, true);

            var provider = new CodeGeneratorProvider();
            var result = await provider.Generate("MyProject", models, config, new Dictionary<string, string>(), _logger.Object);

            Assert.Equal(outputLocation, result.outputLocation);
            Assert.True(Directory.Exists(outputLocation));
        }

        private List<ProjectDataModelDto> GetSampleDataModels()
        {
            var models = new List<ProjectDataModelDto>();

            return models;
        }
    }
}
