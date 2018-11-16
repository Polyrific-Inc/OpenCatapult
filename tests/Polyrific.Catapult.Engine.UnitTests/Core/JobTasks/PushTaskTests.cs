﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Engine.Core.JobTasks;
using Polyrific.Catapult.Engine.UnitTests.Core.JobTasks.Utilities;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Shared.Dto.Project;
using Polyrific.Catapult.Shared.Service;
using Xunit;

namespace Polyrific.Catapult.Engine.UnitTests.Core.JobTasks
{
    public class PushTaskTests
    {
        private readonly Mock<ILogger<PushTask>> _logger;
        private readonly Mock<IProjectService> _projectService;
        private readonly Mock<IProjectDataModelService> _projectDataModelService;
        private readonly Mock<IExternalServiceService> _externalServiceService;

        public PushTaskTests()
        {
            _logger = new Mock<ILogger<PushTask>>();

            _projectService = new Mock<IProjectService>();
            _projectDataModelService = new Mock<IProjectDataModelService>();
            _externalServiceService = new Mock<IExternalServiceService>();
            _projectService.Setup(s => s.GetProject(It.IsAny<int>()))
                .ReturnsAsync((int id) => new ProjectDto { Id = id, Name = $"Project {id}" });
        }

        [Fact]
        public async void RunMainTask_Success()
        {
            var config = new Dictionary<string, string>();


            var providers = new List<ICodeRepositoryProvider>
            {
                new FakeCodeRepositoryProvider("good-result", null, "")
            };

            var task = new PushTask(_projectService.Object, _projectDataModelService.Object, _externalServiceService.Object , _logger.Object) {CodeRepositoryProviders = providers};
            task.SetConfig(config, "working");
            task.Provider = "FakeCodeRepositoryProvider";

            var result = await task.RunMainTask(new Dictionary<string, string>());

            Assert.True(result.IsSuccess);
            Assert.Equal("good-result", result.ReturnValue);
        }

        [Fact]
        public async void RunMainTask_Failed()
        {
            var config = new Dictionary<string, string>();


            var providers = new List<ICodeRepositoryProvider>
            {
                new FakeCodeRepositoryProvider("", null, "error-message")
            };

            var task = new PushTask(_projectService.Object, _projectDataModelService.Object, _externalServiceService.Object , _logger.Object) {CodeRepositoryProviders = providers};
            task.SetConfig(config, "working");
            task.Provider = "FakeCodeRepositoryProvider";

            var result = await task.RunMainTask(new Dictionary<string, string>());

            Assert.False(result.IsSuccess);
            Assert.Equal("error-message", result.ErrorMessage);
        }

        [Fact]
        public async void RunMainTask_NoProvider()
        {
            var config = new Dictionary<string, string>();


            var task = new PushTask(_projectService.Object, _projectDataModelService.Object, _externalServiceService.Object , _logger.Object);
            task.SetConfig(config, "working");
            task.Provider = "FakeCodeRepositoryProvider";

            var result = await task.RunMainTask(new Dictionary<string, string>());

            Assert.False(result.IsSuccess);
            Assert.Equal("Code repository provider \"FakeCodeRepositoryProvider\" could not be found.", result.ErrorMessage);
        }
    }
}
