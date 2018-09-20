// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Plugins.Abstraction.Configs;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace AspNetCoreMvc.Tests
{
    public class CodeGeneratorProviderTests
    {
        [Fact]
        public async void Generate_Success()
        {
            var models = GetSampleDataModels();
            var outputFolderName = "20180817.1";
            var config = new GenerateTaskConfig
            {
                ProviderName = "AspNetCoreMvc",
                WorkingLocation = Path.Combine(AppContext.BaseDirectory, "working")
            };

            var outputLocation = Path.Combine(config.WorkingLocation, outputFolderName);
            if (Directory.Exists(outputLocation))
                Directory.Delete(outputLocation, true);

            var provider = new CodeGeneratorProvider();
            var result = await provider.Generate("MyProject", models, outputFolderName, config);

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
