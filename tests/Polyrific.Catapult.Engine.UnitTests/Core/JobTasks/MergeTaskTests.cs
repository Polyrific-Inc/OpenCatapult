﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

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
    public class MergeTaskTests
    {
        private readonly Mock<IProjectService> _projectService;
        private readonly Mock<IExternalServiceService> _externalServiceService;
        private readonly Mock<IPluginManager> _pluginManager;
        private readonly Mock<ILogger<MergeTask>> _logger;

        public MergeTaskTests()
        {
            _projectService = new Mock<IProjectService>();
            _projectService.Setup(s => s.GetProject(It.IsAny<int>()))
                .ReturnsAsync((int id) => new ProjectDto { Id = id, Name = $"Project {id}" });

            _externalServiceService = new Mock<IExternalServiceService>();

            _pluginManager = new Mock<IPluginManager>();
            _pluginManager.Setup(p => p.GetPlugins(It.IsAny<string>())).Returns(new List<PluginItem>
            {
                new PluginItem("FakeCodeRepositoryProvider", "path/to/FakeCodeRepositoryProvider.dll", new string[] { })
            });

            _logger = new Mock<ILogger<MergeTask>>();
        }

        [Fact]
        public async void RunMainTask_Success()
        {
            _pluginManager.Setup(p => p.InvokeTaskProvider(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string pluginDll, string pluginArgs) => new Dictionary<string, object>
                {
                    {"remoteUrl", "good-result"}
                });

            var config = new Dictionary<string, string>();
                        
            var task = new MergeTask(_projectService.Object, _externalServiceService.Object, _pluginManager.Object, _logger.Object);
            task.SetConfig(config, "working");
            task.Provider = "FakeCodeRepositoryProvider";

            var result = await task.RunMainTask(new Dictionary<string, string> {{"PRNumber", "1"}});

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

            var task = new MergeTask(_projectService.Object, _externalServiceService.Object, _pluginManager.Object, _logger.Object);
            task.SetConfig(config, "working");
            task.Provider = "FakeCodeRepositoryProvider";

            var result = await task.RunMainTask(new Dictionary<string, string> {{"PRNumber", "1"}});

            Assert.False(result.IsSuccess);
            Assert.Equal("error-message", result.ErrorMessage);
        }

        [Fact]
        public async void RunMainTask_NoPRNumber()
        {
            var config = new Dictionary<string, string>();
                        
            var task = new MergeTask(_projectService.Object, _externalServiceService.Object, _pluginManager.Object, _logger.Object);
            task.SetConfig(config, "working");
            task.Provider = "FakeCodeRepositoryProvider";

            var result = await task.RunMainTask(new Dictionary<string, string>());

            Assert.False(result.IsSuccess);
            Assert.Equal("PR Number was undefined.", result.ErrorMessage);
        }
        
        [Fact]
        public async void RunMainTask_NoProvider()
        {
            var config = new Dictionary<string, string>();

            var task = new MergeTask(_projectService.Object, _externalServiceService.Object, _pluginManager.Object, _logger.Object);
            task.SetConfig(config, "working");
            task.Provider = "NotExistRepositoryProvider";

            var result = await task.RunMainTask(new Dictionary<string, string>());

            Assert.False(result.IsSuccess);
            Assert.Equal("Code repository provider \"NotExistRepositoryProvider\" could not be found.", result.ErrorMessage);
        }
    }
}
