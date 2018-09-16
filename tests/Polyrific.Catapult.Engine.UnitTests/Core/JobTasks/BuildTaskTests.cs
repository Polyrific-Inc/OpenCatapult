// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Polyrific.Catapult.Engine.Core.JobTasks;
using Polyrific.Catapult.Engine.UnitTests.Core.JobTasks.Utilities;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using Polyrific.Catapult.Shared.Dto.Project;
using Polyrific.Catapult.Shared.Service;
using System.Collections.Generic;
using Xunit;

namespace Polyrific.Catapult.Engine.UnitTests.Core.JobTasks
{
    public class BuildTaskTests
    {
        private readonly Mock<ILogger<BuildTask>> _logger;
        private readonly Mock<IProjectService> _projectService;

        public BuildTaskTests()
        {
            _logger = new Mock<ILogger<BuildTask>>();

            _projectService = new Mock<IProjectService>();
            _projectService.Setup(s => s.GetProject(It.IsAny<int>()))
                .ReturnsAsync((int id) => new ProjectDto { Id = id, Name = $"Project {id}" });
        }

        [Fact]
        public async void RunMainTask_Success()
        {
            var config = new BuildTaskConfig
            {
                ProviderName = "FakeBuildProvider"
            };
            var configString = JsonConvert.SerializeObject(config);

            var providers = new List<IBuildProvider>
            {
                new FakeBuildProvider("good-result", "")
            };

            var task = new BuildTask(_projectService.Object, _logger.Object) {BuildProviders = providers};
            task.SetConfig(configString);

            var result = await task.RunMainTask();

            Assert.True(result.IsSuccess);
            Assert.Equal("good-result", result.ReturnValue);
        }

        [Fact]
        public async void RunMainTask_Failed()
        {
            var config = new BuildTaskConfig
            {
                ProviderName = "FakeBuildProvider"
            };
            var configString = JsonConvert.SerializeObject(config);

            var providers = new List<IBuildProvider>
            {
                new FakeBuildProvider("", "error-message")
            };

            var task = new BuildTask(_projectService.Object, _logger.Object) {BuildProviders = providers};
            task.SetConfig(configString);

            var result = await task.RunMainTask();

            Assert.False(result.IsSuccess);
            Assert.Equal("error-message", result.ErrorMessage);
        }

        [Fact]
        public async void RunMainTask_NoProvider()
        {
            var config = new BuildTaskConfig
            {
                ProviderName = "FakeBuildProvider"
            };
            var configString = JsonConvert.SerializeObject(config);

            var task = new BuildTask(_projectService.Object, _logger.Object);
            task.SetConfig(configString);

            var result = await task.RunMainTask();

            Assert.False(result.IsSuccess);
            Assert.Equal("Build provider \"FakeBuildProvider\" could not be found.", result.ErrorMessage);
        }
    }
}
