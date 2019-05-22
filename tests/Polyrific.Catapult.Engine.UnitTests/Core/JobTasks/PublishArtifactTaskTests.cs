// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Engine.Core;
using Polyrific.Catapult.Engine.Core.JobTasks;
using Polyrific.Catapult.Shared.Dto.ExternalService;
using Polyrific.Catapult.Shared.Dto.ExternalServiceType;
using Polyrific.Catapult.Shared.Dto.Provider;
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
        private readonly Mock<IExternalServiceTypeService> _externalServiceTypeService;
        private readonly Mock<IProviderService> _providerService;
        private readonly Mock<ITaskProviderManager> _taskProviderManager;

        public PublishArtifactTaskTests()
        {
            _logger = new Mock<ILogger<PublishArtifactTask>>();

            _projectService = new Mock<IProjectService>();
            _externalServiceService = new Mock<IExternalServiceService>();
            _projectService.Setup(s => s.GetProject(It.IsAny<int>()))
                .ReturnsAsync((int id) => new ProjectDto { Id = id, Name = $"Project {id}" });

            _taskProviderManager = new Mock<ITaskProviderManager>();
            _taskProviderManager.Setup(p => p.GetTaskProviders(It.IsAny<string>())).Returns(new List<TaskProviderItem>
            {
                new TaskProviderItem("FakeStorageProvider", "path/to/FakeStorageProvider.dll", new string[] { })
            });

            _externalServiceTypeService = new Mock<IExternalServiceTypeService>();
            _externalServiceTypeService.Setup(s => s.GetExternalServiceTypes(It.IsAny<bool>()))
                .ReturnsAsync(new List<ExternalServiceTypeDto>
                {
                    new ExternalServiceTypeDto
                    {
                        Name = "GitHub",
                        ExternalServiceProperties = new List<ExternalServicePropertyDto>
                        {
                            new ExternalServicePropertyDto
                            {
                                Name = "AuthToken",
                                IsSecret = true
                            }
                        }
                    }
                });
            _providerService = new Mock<IProviderService>();
            _providerService.Setup(s => s.GetProviderAdditionalConfigByProviderName(It.IsAny<string>()))
                .ReturnsAsync(new List<TaskProviderAdditionalConfigDto>
                {
                    new TaskProviderAdditionalConfigDto
                    {
                        Name = "ConnectionString",
                        IsSecret = true
                    }
                });
        }

        [Fact]
        public async void RunMainTask_Success()
        {
            _taskProviderManager.Setup(p => p.InvokeTaskProvider(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string taskProviderDll, string taskProviderArgs, string secretTaskProviderArgs) => new Dictionary<string, object>
                {
                    {"storageLocation", "c:\\test"}
                });

            var config = new Dictionary<string, string>();
            
            var task = new PublishArtifactTask(_projectService.Object, _externalServiceService.Object, _externalServiceTypeService.Object, _providerService.Object, _taskProviderManager.Object, _logger.Object);
            task.SetConfig(config, "working");
            task.Provider = "FakeStorageProvider";

            var result = await task.RunMainTask(new Dictionary<string, string>());

            Assert.True(result.IsSuccess);
            Assert.Equal("c:\\test", result.ReturnValue);
            Assert.Equal("The downloaded build artifact is located at c:\\test", result.TaskRemarks);
        }

        [Fact]
        public async void RunMainTask_Failed()
        {
            _taskProviderManager.Setup(p => p.InvokeTaskProvider(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string taskProviderDll, string taskProviderArgs, string secretTaskProviderArgs) => new Dictionary<string, object>
                {
                    {"errorMessage", "error-message"}
                });

            var config = new Dictionary<string, string>();
            
            var task = new PublishArtifactTask(_projectService.Object, _externalServiceService.Object, _externalServiceTypeService.Object, _providerService.Object, _taskProviderManager.Object, _logger.Object);
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
            
            var task = new PublishArtifactTask(_projectService.Object, _externalServiceService.Object, _externalServiceTypeService.Object, _providerService.Object, _taskProviderManager.Object, _logger.Object);
            task.SetConfig(config, "working");
            task.Provider = "NotExistStorageProvider";

            var result = await task.RunMainTask(new Dictionary<string, string>());

            Assert.False(result.IsSuccess);
            Assert.Equal("Storage provider \"NotExistStorageProvider\" could not be found.", result.ErrorMessage);
        }

        [Fact]
        public async void RunMainTask_AdditionalConfigSecured()
        {
            _taskProviderManager.Setup(p => p.InvokeTaskProvider(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string taskProviderDll, string taskProviderArgs, string secretTaskProviderArgs) => new Dictionary<string, object>
                {
                    {"storageLocation", "c:\\test"}
                });
            _taskProviderManager.Setup(p => p.GetTaskProviders(It.IsAny<string>())).Returns(new List<TaskProviderItem>
            {
                new TaskProviderItem("FakeStorageProvider", "path/to/FakeStorageProvider.dll", new string[] { "GitHub" })
            });
            _externalServiceService.Setup(p => p.GetExternalServiceByName(It.IsAny<string>())).ReturnsAsync((string name) => new ExternalServiceDto
            {
                Name = name,
                Config = new Dictionary<string, string>
                {
                    { "AuthToken", "123" }
                }
            });

            var config = new Dictionary<string, string>
            {
                { "GitHubExternalService", "github-test" }
            };

            var task = new PublishArtifactTask(_projectService.Object, _externalServiceService.Object, _externalServiceTypeService.Object, _providerService.Object, _taskProviderManager.Object, _logger.Object);
            task.SetConfig(config, "working");
            task.Provider = "FakeStorageProvider";
            task.AdditionalConfigs = new Dictionary<string, string>
            {
                { "ConnectionString", "Server=localhost;Database=TestProject;User ID=sa;Password=samprod;" }
            };

            var result = await task.RunMainTask(new Dictionary<string, string>());

            Assert.True(result.IsSuccess);
            Assert.Equal("c:\\test", result.ReturnValue);
            Assert.Equal("The downloaded build artifact is located at c:\\test", result.TaskRemarks);

            Assert.Equal(2, task.AdditionalConfigs.Count);
            Assert.Equal(2, task.SecuredAdditionalConfigs.Count);
            Assert.Equal("***", task.SecuredAdditionalConfigs["AuthToken"]);
            Assert.Equal("***", task.SecuredAdditionalConfigs["ConnectionString"]);
            Assert.Equal("123", task.AdditionalConfigs["AuthToken"]);
            Assert.Equal("Server=localhost;Database=TestProject;User ID=sa;Password=samprod;", task.AdditionalConfigs["ConnectionString"]);
        }
    }
}
