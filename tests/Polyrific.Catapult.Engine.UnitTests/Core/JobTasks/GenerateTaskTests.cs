// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Polyrific.Catapult.Engine.Core.JobTasks;
using Polyrific.Catapult.Engine.UnitTests.Core.JobTasks.Utilities;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using Polyrific.Catapult.Shared.Dto.Project;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;
using Polyrific.Catapult.Shared.Service;
using System.Collections.Generic;
using Xunit;

namespace Polyrific.Catapult.Engine.UnitTests.Core.JobTasks
{
    public class GenerateTaskTests
    {
        private readonly Mock<IProjectService> _projectService;
        private readonly Mock<IProjectDataModelService> _dataModelService;
        private readonly Mock<ILogger<GenerateTask>> _logger;

        public GenerateTaskTests()
        {
            var dataModels = new List<ProjectDataModelDto>
            {
                new ProjectDataModelDto{Id = 1, ProjectId = 1}
            };

            _projectService = new Mock<IProjectService>();
            _projectService.Setup(s => s.GetProject(It.IsAny<int>()))
                .ReturnsAsync((int id) => new ProjectDto {Id = id, Name = $"Project {id}"});

            _dataModelService = new Mock<IProjectDataModelService>();
            _dataModelService.Setup(s => s.GetProjectDataModels(It.IsAny<int>())).ReturnsAsync(dataModels);

            _logger = new Mock<ILogger<GenerateTask>>();
        }

        [Fact]
        public async void RunMainTask_Success()
        {
            var config = new GenerateTaskConfig();
            var configString = JsonConvert.SerializeObject(config);

            var providers = new List<ICodeGeneratorProvider>
            {
                new FakeCodeGeneratorProvider("good-result", "")
            };

            var task = new GenerateTask(_projectService.Object, _dataModelService.Object, _logger.Object) {GeneratorProviders = providers};
            task.SetConfig(configString);
            task.Provider = "FakeCodeGeneratorProvider";

            var result = await task.RunMainTask();

            Assert.True(result.IsSuccess);
            Assert.Equal("good-result", result.ReturnValue);
        }

        [Fact]
        public async void RunMainTask_Failed()
        {
            var config = new GenerateTaskConfig();
            var configString = JsonConvert.SerializeObject(config);

            var providers = new List<ICodeGeneratorProvider>
            {
                new FakeCodeGeneratorProvider("", "error-message")
            };

            var task = new GenerateTask(_projectService.Object, _dataModelService.Object, _logger.Object) {GeneratorProviders = providers};
            task.SetConfig(configString);
            task.Provider = "FakeCodeGeneratorProvider";

            var result = await task.RunMainTask();

            Assert.False(result.IsSuccess);
            Assert.Equal("error-message", result.ErrorMessage);
        }

        [Fact]
        public async void RunMainTask_NoProvider()
        {
            var config = new GenerateTaskConfig();
            var configString = JsonConvert.SerializeObject(config);

            var task = new GenerateTask(_projectService.Object, _dataModelService.Object, _logger.Object);
            task.SetConfig(configString);
            task.Provider = "FakeCodeGeneratorProvider";

            var result = await task.RunMainTask();

            Assert.False(result.IsSuccess);
            Assert.Equal("Code generator provider \"FakeCodeGeneratorProvider\" could not be found.", result.ErrorMessage);
        }
    }
}
