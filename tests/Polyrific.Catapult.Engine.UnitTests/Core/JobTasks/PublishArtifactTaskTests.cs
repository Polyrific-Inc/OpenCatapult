// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Engine.Core;
using Polyrific.Catapult.Engine.Core.JobTasks;
using Polyrific.Catapult.Shared.Dto.Project;
using Polyrific.Catapult.Shared.Service;
using Xunit;

namespace Polyrific.Catapult.Engine.UnitTests.Core.JobTasks
{
    public class PublishArtifactTaskTests
    {
        private readonly Mock<ILogger<PublishArtifactTask>> _logger;
        private readonly Mock<IProjectService> _projectService;
        private readonly Mock<IExternalServiceService> _externalServiceService;
        private readonly Mock<IPluginManager> _pluginManager;

        public PublishArtifactTaskTests()
        {
            _logger = new Mock<ILogger<PublishArtifactTask>>();

            _projectService = new Mock<IProjectService>();
            _externalServiceService = new Mock<IExternalServiceService>();
            _projectService.Setup(s => s.GetProject(It.IsAny<int>()))
                .ReturnsAsync((int id) => new ProjectDto { Id = id, Name = $"Project {id}" });

            _pluginManager = new Mock<IPluginManager>();
            _pluginManager.Setup(p => p.GetPlugins(It.IsAny<string>())).Returns(new List<PluginItem>
            {
                new PluginItem("FakeStorageProvider", "path/to/FakeStorageProvider.dll", new string[] { })
            });
        }

        [Fact]
        public async void RunMainTask_Success()
        {
            _pluginManager.Setup(p => p.InvokeTaskProvider(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string pluginDll, string pluginArgs) => new Dictionary<string, object>
                {
                    {"storageLocation", "good-result"}
                });

            var config = new Dictionary<string, string>();
            
            var task = new PublishArtifactTask(_projectService.Object, _externalServiceService.Object, _pluginManager.Object, _logger.Object);
            task.SetConfig(config, "working");
            task.Provider = "FakeStorageProvider";

            var result = await task.RunMainTask(new Dictionary<string, string>());

            Assert.True(result.IsSuccess);
            Assert.Equal("good-result", result.ReturnValue);
        }

        [Fact]
        public async void RunMainTask_Failed()
        {
            _pluginManager.Setup(p => p.InvokeTaskProvider(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string pluginDll, string pluginArgs) => new Dictionary<string, object>
                {
                    {"errorMessage", "error-message"}
                });

            var config = new Dictionary<string, string>();
            
            var task = new PublishArtifactTask(_projectService.Object, _externalServiceService.Object, _pluginManager.Object, _logger.Object);
            task.SetConfig(config, "working");
            task.Provider = "FakeStorageProvider";

            var result = await task.RunMainTask(new Dictionary<string, string>());

            Assert.False(result.IsSuccess);
            Assert.Equal("error-message", result.ErrorMessage);
        }

        [Fact]
        public async void RunMainTask_NoProvider()
        {
            var config = new Dictionary<string, string>();
            
            var task = new PublishArtifactTask(_projectService.Object, _externalServiceService.Object, _pluginManager.Object, _logger.Object);
            task.SetConfig(config, "working");
            task.Provider = "NotExistStorageProvider";

            var result = await task.RunMainTask(new Dictionary<string, string>());

            Assert.False(result.IsSuccess);
            Assert.Equal("Storage provider \"NotExistStorageProvider\" could not be found.", result.ErrorMessage);
        }
    }
}
