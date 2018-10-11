// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Polyrific.Catapult.Engine.Core.JobTasks;
using Polyrific.Catapult.Engine.UnitTests.Core.JobTasks.Utilities;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using Polyrific.Catapult.Shared.Dto.Project;
using Polyrific.Catapult.Shared.Service;
using Xunit;

namespace Polyrific.Catapult.Engine.UnitTests.Core.JobTasks
{
    public class DeployTaskTests
    {
        private readonly Mock<ILogger<DeployTask>> _logger;
        private readonly Mock<IProjectService> _projectService;
        private readonly Mock<IExternalServiceService> _externalServiceService;

        public DeployTaskTests()
        {
            _logger = new Mock<ILogger<DeployTask>>();

            _projectService = new Mock<IProjectService>();
            _externalServiceService = new Mock<IExternalServiceService>();
            _projectService.Setup(s => s.GetProject(It.IsAny<int>()))
                .ReturnsAsync((int id) => new ProjectDto { Id = id, Name = $"Project {id}" });
        }

        [Fact]
        public async void RunMainTask_Success()
        {
            var config = new Dictionary<string, string>();


            var providers = new List<IHostingProvider>
            {
                new FakeHostingProvider("good-result", null, "")
            };

            var task = new DeployTask(_projectService.Object, _externalServiceService.Object , _logger.Object) {HostingProviders = providers};
            task.SetConfig(config, "working");
            task.Provider = "FakeHostingProvider";

            var result = await task.RunMainTask(new Dictionary<string, string>());

            Assert.True(result.IsSuccess);
            Assert.Equal("good-result", result.ReturnValue);
        }

        [Fact]
        public async void RunMainTask_Failed()
        {
            var config = new Dictionary<string, string>();


            var providers = new List<IHostingProvider>
            {
                new FakeHostingProvider("", null, "error-message")
            };

            var task = new DeployTask(_projectService.Object, _externalServiceService.Object , _logger.Object) {HostingProviders = providers};
            task.SetConfig(config, "working");
            task.Provider = "FakeHostingProvider";

            var result = await task.RunMainTask(new Dictionary<string, string>());

            Assert.False(result.IsSuccess);
            Assert.Equal("error-message", result.ErrorMessage);
        }

        [Fact]
        public async void RunMainTask_NoProvider()
        {
            var config = new Dictionary<string, string>();


            var task = new DeployTask(_projectService.Object, _externalServiceService.Object , _logger.Object);
            task.SetConfig(config, "working");
            task.Provider = "FakeHostingProvider";

            var result = await task.RunMainTask(new Dictionary<string, string>());

            Assert.False(result.IsSuccess);
            Assert.Equal("Deploy provider \"FakeHostingProvider\" could not be found.", result.ErrorMessage);
        }
    }
}
