﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Engine.Core;
using Polyrific.Catapult.Engine.Core.JobTasks;
using Polyrific.Catapult.Engine.UnitTests.Core.JobTasks.Utilities;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Shared.Dto.Project;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;
using Polyrific.Catapult.Shared.Service;
using Xunit;

namespace Polyrific.Catapult.Engine.UnitTests.Core.JobTasks
{
    public class GenerateTaskTests
    {
        private readonly Mock<IProjectService> _projectService;
        private readonly Mock<IExternalServiceService> _externalServiceService;
        private readonly Mock<IProjectDataModelService> _dataModelService;
        private readonly Mock<ILogger<GenerateTask>> _logger;
        private readonly Mock<IPluginManager> _pluginManager;

        public GenerateTaskTests()
        {
            var dataModels = new List<ProjectDataModelDto>
            {
                new ProjectDataModelDto{Id = 1, ProjectId = 1}
            };

            _projectService = new Mock<IProjectService>();
            _externalServiceService = new Mock<IExternalServiceService>();
            _projectService.Setup(s => s.GetProject(It.IsAny<int>()))
                .ReturnsAsync((int id) => new ProjectDto {Id = id, Name = $"Project {id}"});

            _dataModelService = new Mock<IProjectDataModelService>();
            _dataModelService.Setup(s => s.GetProjectDataModels(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(dataModels);

            _logger = new Mock<ILogger<GenerateTask>>();
            _pluginManager = new Mock<IPluginManager>();
        }

        [Fact]
        public async void RunMainTask_Success()
        {
            var config = new Dictionary<string, string>();


            var providers = new List<ICodeGeneratorProvider>
            {
                new FakeCodeGeneratorProvider("good-result", null, "")
            };

            var task = new GenerateTask(_projectService.Object, _externalServiceService.Object, _dataModelService.Object, _pluginManager.Object, _logger.Object);
            task.SetConfig(config, "working");
            task.Provider = "FakeCodeGeneratorProvider";

            var result = await task.RunMainTask(new Dictionary<string, string>());

            Assert.True(result.IsSuccess);
            Assert.Equal("good-result", result.ReturnValue);
        }

        [Fact]
        public async void RunMainTask_Failed()
        {
            var config = new Dictionary<string, string>();


            var providers = new List<ICodeGeneratorProvider>
            {
                new FakeCodeGeneratorProvider("", null, "error-message")
            };

            var task = new GenerateTask(_projectService.Object, _externalServiceService.Object, _dataModelService.Object, _pluginManager.Object, _logger.Object);
            task.SetConfig(config, "working");
            task.Provider = "FakeCodeGeneratorProvider";

            var result = await task.RunMainTask(new Dictionary<string, string>());

            Assert.False(result.IsSuccess);
            Assert.Equal("error-message", result.ErrorMessage);
        }

        [Fact]
        public async void RunMainTask_NoProvider()
        {
            var config = new Dictionary<string, string>();


            var task = new GenerateTask(_projectService.Object, _externalServiceService.Object, _dataModelService.Object, _pluginManager.Object, _logger.Object);
            task.SetConfig(config, "working");
            task.Provider = "FakeCodeGeneratorProvider";

            var result = await task.RunMainTask(new Dictionary<string, string>());

            Assert.False(result.IsSuccess);
            Assert.Equal("Code generator provider \"FakeCodeGeneratorProvider\" could not be found.", result.ErrorMessage);
        }
    }
}
