using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Polyrific.Catapult.TaskProviders.Core.Configs;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;
using Xunit;

namespace Polyrific.Catapult.Plugins.AspNetCoreMvc.UnitTests
{
    public class CodeGeneratorProviderTests
    {
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

            var additionalConfig = new Dictionary<string, string>();

            if (Directory.Exists(outputLocation))
                Directory.Delete(outputLocation, true);

            var provider = new Program(new string[] { GetArgString("main", "TestProject", models, config, additionalConfig) });
            var result = await provider.Generate();

            Assert.Equal(outputLocation, result.outputLocation);
            Assert.True(Directory.Exists(outputLocation));
        }

        private List<ProjectDataModelDto> GetSampleDataModels()
        {
            var models = new List<ProjectDataModelDto>
            {
                new ProjectDataModelDto
                {
                    Name = "Product",
                    Description = "Product",
                    Label = "Product",
                    Properties = new List<ProjectDataModelPropertyDto>
                    {
                        new ProjectDataModelPropertyDto
                        {
                            Name = "Title",
                            Label = "Title",
                            DataType = PropertyDataType.String,
                            ControlType = PropertyControlType.InputText
                        },
                        new ProjectDataModelPropertyDto
                        {
                            Name = "Price",
                            Label = "Price",
                            DataType = PropertyDataType.Integer,
                            ControlType = PropertyControlType.InputNumber
                        }
                    }
                }
            };

            return models;
        }

        private string GetArgString(string process, string projectName, List<ProjectDataModelDto> dataModels, GenerateTaskConfig taskConfig, Dictionary<string, string> additionalConfigs)
        {
            var dict = new Dictionary<string, object>
            {
                {"process", process},
                {"project", projectName},
                {"models", dataModels},
                {"config", taskConfig},
                {"additional", additionalConfigs}
            };

            return JsonConvert.SerializeObject(dict);
        }
    }
}
