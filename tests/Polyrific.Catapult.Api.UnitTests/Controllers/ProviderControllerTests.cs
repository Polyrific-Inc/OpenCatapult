// Copyright (c) Polyrific, Inc 2018. All rights reserved.

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
        private readonly Mock<IPluginService> _pluginService;
        private readonly Mock<IPluginAdditionalConfigService> _pluginAdditionalConfigService;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ProviderController>> _logger;

        public ProviderControllerTests()
        {
            _pluginService = new Mock<IPluginService>();

            _pluginAdditionalConfigService = new Mock<IPluginAdditionalConfigService>();

            _mapper = AutoMapperUtils.GetMapper();

            _logger = LoggerMock.GetLogger<ProviderController>();
        }

        [Fact]
        public async void GetProviders_ReturnsProviderList()
        {
            _pluginService.Setup(s => s.GetPlugins(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Plugin>
                {
                    new Plugin
                    {
                        Id = 1,
                        Name = "AspMvcNet"
                    }
                });

            var controller = new ProviderController(_pluginService.Object, _pluginAdditionalConfigService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetProviders();

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ProviderDto>>(okActionResult.Value);
            Assert.NotEmpty(returnValue);
        }

        [Fact]
        public async void GetProvidersByType_ReturnsProviderList()
        {
            _pluginService.Setup(s => s.GetPlugins(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string type, CancellationToken cancellationToken) =>
                new List<Plugin>
                {
                    new Plugin
                    {
                        Id = 1,
                        Name = "AspMvcNet",
                        Type = type
                    }
                });

            var controller = new ProviderController(_pluginService.Object, _pluginAdditionalConfigService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetProvidersByType("GeneratorProvider");

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ProviderDto>>(okActionResult.Value);
            Assert.NotEmpty(returnValue);
            Assert.DoesNotContain(returnValue, p => p.Type != "GeneratorProvider");
        }

        [Fact]
        public async void GetProviderById_ReturnsProvider()
        {
            _pluginService.Setup(s => s.GetPluginById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) =>
                    new Plugin
                    {
                        Id = id,
                        Name = "AspMvcNet"
                    });

            var controller = new ProviderController(_pluginService.Object, _pluginAdditionalConfigService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetProviderById(1);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProviderDto>(okActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void GetProviderByName_ReturnsProvider()
        {
            _pluginService.Setup(s => s.GetPluginByName(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string serviceName, CancellationToken cancellationToken) =>
                    new Plugin
                    {
                        Id = 1,
                        Name = serviceName
                    });

            var controller = new ProviderController(_pluginService.Object, _pluginAdditionalConfigService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetProviderByName("AspMvcNet");

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProviderDto>(okActionResult.Value);
            Assert.Equal("AspMvcNet", returnValue.Name);
        }

        [Fact]
        public async void RegisterProvider_ReturnsCreatedProvider()
        {
            _pluginService
                .Setup(s => s.AddPlugin(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string name, string type, string author, string version, string[] requiredServices, CancellationToken cancellationToken) =>
                    new Plugin
                    {
                        Id = 1,
                        Name = name,
                        Type = type,
                        Author = author,
                        Version = version,
                        RequiredServicesString = string.Join(",", requiredServices)
                    });
            _pluginAdditionalConfigService.Setup(s => s.AddAdditionalConfigs(It.IsAny<int>(), It.IsAny<List<PluginAdditionalConfig>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<int> { 1 });

            var controller = new ProviderController(_pluginService.Object, _pluginAdditionalConfigService.Object, _mapper, _logger.Object);

            var dto = new NewProviderDto
            {
                Name = "AspMvcNet",
                Type = "GeneratorProvider",
                Author = "polyrific",
                Version = "1.0.0",
                RequiredServices = new string[] { "test" }
            };
            var result = await controller.RegisterProvider(dto);

            var createAtRouteActionResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<ProviderDto>(createAtRouteActionResult.Value);
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
            _pluginService.Setup(s => s.DeletePlugin(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var controller = new ProviderController(_pluginService.Object, _pluginAdditionalConfigService.Object, _mapper, _logger.Object);

            var result = await controller.DeleteProviderById(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void GetProviderAdditionalConfigsByProviderName_ReturnsProviderAdditionalConfigList()
        {
            _pluginAdditionalConfigService.Setup(s => s.GetByPluginName(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PluginAdditionalConfig>
                {
                    new PluginAdditionalConfig
                    {
                        Id = 1,
                        Name = "ConnectionString"
                    }
                });

            var controller = new ProviderController(_pluginService.Object, _pluginAdditionalConfigService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetProviderAdditionalConfigsByProviderName("AspNetCoreMvc");

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ProviderAdditionalConfigDto>>(okActionResult.Value);
            Assert.NotEmpty(returnValue);
        }
    }
}
