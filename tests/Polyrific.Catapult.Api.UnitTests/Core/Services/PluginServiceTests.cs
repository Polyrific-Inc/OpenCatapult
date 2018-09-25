// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Shared.Dto.Constants;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Core.Services
{
    public class PluginServiceTests
    {
        private readonly Mock<IPluginRepository> _pluginRepository;
        private readonly Mock<IExternalServiceTypeRepository> _externalServiceTypeRepository;

        public PluginServiceTests()
        {
            _pluginRepository = new Mock<IPluginRepository>();
            _externalServiceTypeRepository = new Mock<IExternalServiceTypeRepository>();
        }

        [Fact]
        public async void GetPlugins_ReturnItems()
        {
            _pluginRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<Plugin>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Plugin>
                {
                    new Plugin { Id = 1, Name = "GeneratorProvider1", Type = PluginType.GeneratorProvider },
                    new Plugin { Id = 2, Name = "RepositoryProvider1", Type = PluginType.RepositoryProvider },
                    new Plugin {Id = 3, Name = "BuildProvider1", Type = PluginType.BuildProvider}
                });

            var service = new PluginService(_pluginRepository.Object, _externalServiceTypeRepository.Object);

            var results = await service.GetPlugins();

            Assert.Equal(3, results.Count);
        }

        [Fact]
        public async void GetPluginsByType_ReturnItems()
        {
            _pluginRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<Plugin>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Plugin>
                    {new Plugin {Id = 3, Name = "BuildProvider1", Type = PluginType.BuildProvider}});

            var service = new PluginService(_pluginRepository.Object, _externalServiceTypeRepository.Object);

            var results = await service.GetPlugins(PluginType.BuildProvider);

            Assert.Single(results);
            Assert.Equal("BuildProvider1", results.First().Name);
        }

        [Fact]
        public async void GetPluginsByType_ReturnEmpty()
        {
            _pluginRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<Plugin>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Plugin>());

            var service = new PluginService(_pluginRepository.Object, _externalServiceTypeRepository.Object);

            var results = await service.GetPlugins(PluginType.HostingProvider);

            Assert.Empty(results);
        }

        [Fact]
        public async void GetPluginById_ReturnItem()
        {
            _pluginRepository
                .Setup(r => r.GetById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Plugin {Id = 1, Name = "GeneratorProvider1"});

            var service = new PluginService(_pluginRepository.Object, _externalServiceTypeRepository.Object);

            var result = await service.GetPluginById(1);

            Assert.NotNull(result);
            Assert.Equal("GeneratorProvider1", result.Name);
        }

        [Fact]
        public async void GetPluginById_ReturnNull()
        {
            _pluginRepository
                .Setup(r => r.GetById(4, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Plugin)null);

            var service = new PluginService(_pluginRepository.Object, _externalServiceTypeRepository.Object);

            var result = await service.GetPluginById(4);

            Assert.Null(result);
        }

        [Fact]
        public async void GetPluginByName_ReturnItem()
        {
            _pluginRepository
                .Setup(r => r.GetSingleBySpec(It.IsAny<ISpecification<Plugin>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new Plugin { Id = 1, Name = "GeneratorProvider1"});

            var service = new PluginService(_pluginRepository.Object, _externalServiceTypeRepository.Object);

            var result = await service.GetPluginByName("GeneratorProvider1");

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async void GetPluginByName_ReturnNull()
        {
            _pluginRepository
                .Setup(r => r.GetSingleBySpec(It.IsAny<ISpecification<Plugin>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Plugin)null);

            var service = new PluginService(_pluginRepository.Object, _externalServiceTypeRepository.Object);

            var result = await service.GetPluginByName("HostingProvider1");

            Assert.Null(result);
        }

        [Fact]
        public async void AddPlugin_Success()
        {
            _pluginRepository.Setup(r => r.Create(It.IsAny<Plugin>(), It.IsAny<CancellationToken>())).ReturnsAsync(4);
            _pluginRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => new Plugin {Id = id, Name = "HostingProvider1"});

            var service = new PluginService(_pluginRepository.Object, _externalServiceTypeRepository.Object);

            var result = await service.AddPlugin("HostingProvider1", PluginType.HostingProvider, "Frandi", "1.0", null);

            Assert.Equal(4, result.Id);
        }

        [Fact]
        public void AddPlugin_RequiredServiceNotSupporedException()
        {
            _pluginRepository.Setup(r => r.Create(It.IsAny<Plugin>(), It.IsAny<CancellationToken>())).ReturnsAsync(4);
            _pluginRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => new Plugin { Id = id, Name = "HostingProvider1" });

            var service = new PluginService(_pluginRepository.Object, _externalServiceTypeRepository.Object);

            var exception = Record.ExceptionAsync(() => service.AddPlugin("HostingProvider1", PluginType.HostingProvider, "Frandi", "1.0", new string[] { "Service" }));

            Assert.IsType<RequiredServicesNotSupportedException>(exception?.Result);
        }

        [Fact]
        public async void DeletePlugin_Success()
        {
            _pluginRepository.Setup(r => r.Delete(1, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var service = new PluginService(_pluginRepository.Object, _externalServiceTypeRepository.Object);

            await service.DeletePlugin(1);

            _pluginRepository.Verify(r => r.Delete(1, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
