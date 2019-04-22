// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Api.Controllers;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.UnitTests.Utilities;
using Polyrific.Catapult.Shared.Dto.Provider;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Controllers
{
    public class ProviderControllerTests
    {
        private readonly Mock<ITaskProviderService> _providerService;
        private readonly Mock<ITaskProviderAdditionalConfigService> _providerAdditionalConfigService;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<TaskProviderController>> _logger;

        public ProviderControllerTests()
        {
            _providerService = new Mock<ITaskProviderService>();

            _providerAdditionalConfigService = new Mock<ITaskProviderAdditionalConfigService>();

            _mapper = AutoMapperUtils.GetMapper();

            _logger = LoggerMock.GetLogger<TaskProviderController>();
        }

        [Fact]
        public async void GetProviders_ReturnsProviderList()
        {
            _providerService.Setup(s => s.GetTaskProviders(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TaskProvider>
                {
                    new TaskProvider
                    {
                        Id = 1,
                        Name = "AspMvcNet"
                    }
                });

            var controller = new TaskProviderController(_providerService.Object, _providerAdditionalConfigService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetTaskProviders();

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<TaskProviderDto>>(okActionResult.Value);
            Assert.NotEmpty(returnValue);
        }

        [Fact]
        public async void GetProvidersByType_ReturnsProviderList()
        {
            _providerService.Setup(s => s.GetTaskProviders(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string type, CancellationToken cancellationToken) =>
                new List<TaskProvider>
                {
                    new TaskProvider
                    {
                        Id = 1,
                        Name = "AspMvcNet",
                        Type = type
                    }
                });

            var controller = new TaskProviderController(_providerService.Object, _providerAdditionalConfigService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetTaskProvidersByType("GeneratorProvider");

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<TaskProviderDto>>(okActionResult.Value);
            Assert.NotEmpty(returnValue);
            Assert.DoesNotContain(returnValue, p => p.Type != "GeneratorProvider");
        }

        [Fact]
        public async void GetProviderById_ReturnsProvider()
        {
            _providerService.Setup(s => s.GetTaskProviderById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) =>
                    new TaskProvider
                    {
                        Id = id,
                        Name = "AspMvcNet"
                    });

            var controller = new TaskProviderController(_providerService.Object, _providerAdditionalConfigService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetTaskProviderById(1);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TaskProviderDto>(okActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void GetProviderByName_ReturnsProvider()
        {
            _providerService.Setup(s => s.GetTaskProviderByName(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string serviceName, CancellationToken cancellationToken) =>
                    new TaskProvider
                    {
                        Id = 1,
                        Name = serviceName
                    });

            var controller = new TaskProviderController(_providerService.Object, _providerAdditionalConfigService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetTaskProviderByName("AspMvcNet");

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TaskProviderDto>(okActionResult.Value);
            Assert.Equal("AspMvcNet", returnValue.Name);
        }

        [Fact]
        public async void RegisterProvider_ReturnsCreatedProvider()
        {
            _providerService
                .Setup(s => s.AddTaskProvider(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string name, string type, string author, string version, string[] requiredServices, 
                    string displayName, string description, string thumnailUrl, string tags, DateTime created, DateTime? updated, CancellationToken cancellationToken) =>
                    new TaskProvider
                    {
                        Id = 1,
                        Name = name,
                        Type = type,
                        Author = author,
                        Version = version,
                        RequiredServicesString = string.Join(",", requiredServices)
                    });
            _providerAdditionalConfigService.Setup(s => s.AddAdditionalConfigs(It.IsAny<int>(), It.IsAny<List<TaskProviderAdditionalConfig>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<int> { 1 });

            var controller = new TaskProviderController(_providerService.Object, _providerAdditionalConfigService.Object, _mapper, _logger.Object);

            var dto = new NewTaskProviderDto
            {
                Name = "AspMvcNet",
                Type = "GeneratorProvider",
                Author = "polyrific",
                Version = "1.0.0",
                RequiredServices = new string[] { "test" }
            };
            var result = await controller.RegisterTaskProvider(dto);

            var createAtRouteActionResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<TaskProviderDto>(createAtRouteActionResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("AspMvcNet", returnValue.Name);
            Assert.Equal("GeneratorProvider", returnValue.Type);
            Assert.Equal("polyrific", returnValue.Author);
            Assert.Equal("1.0.0", returnValue.Version);
            Assert.NotEmpty(returnValue.RequiredServices);
        }
        
        [Fact]
        public async void DeleteProvider_ReturnsNoContent()
        {
            _providerService.Setup(s => s.DeleteTaskProvider(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var controller = new TaskProviderController(_providerService.Object, _providerAdditionalConfigService.Object, _mapper, _logger.Object);

            var result = await controller.DeleteTaskProviderById(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void GetProviderAdditionalConfigsByProviderName_ReturnsProviderAdditionalConfigList()
        {
            _providerAdditionalConfigService.Setup(s => s.GetByTaskProviderName(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TaskProviderAdditionalConfig>
                {
                    new TaskProviderAdditionalConfig
                    {
                        Id = 1,
                        Name = "ConnectionString"
                    }
                });

            var controller = new TaskProviderController(_providerService.Object, _providerAdditionalConfigService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetTaskProviderAdditionalConfigsByTaskProviderName("AspNetCoreMvc");

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<TaskProviderAdditionalConfigDto>>(okActionResult.Value);
            Assert.NotEmpty(returnValue);
        }
    }
}
