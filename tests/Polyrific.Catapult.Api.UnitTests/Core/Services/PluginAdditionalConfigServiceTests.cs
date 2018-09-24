// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading;
using Moq;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Core.Specifications;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Core.Services
{
    public class PluginAdditionalConfigServiceTests
    {
        private readonly Mock<IPluginRepository> _pluginRepository;
        private readonly Mock<IPluginAdditionalConfigRepository> _pluginAdditionalConfigRepository;

        public PluginAdditionalConfigServiceTests()
        {
            _pluginAdditionalConfigRepository = new Mock<IPluginAdditionalConfigRepository>();
            _pluginRepository = new Mock<IPluginRepository>();
        }

        [Fact]
        public async void GetByPlugin_ReturnItems()
        {
            _pluginAdditionalConfigRepository
                .Setup(r => r.GetBySpec(It.IsAny<PluginAdditionalConfigFilterSpecification>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(new List<PluginAdditionalConfig>
                {
                    new PluginAdditionalConfig {Id = 1, PluginId = 1, Name = "Config1"}
                });

            var service =
                new PluginAdditionalConfigService(_pluginRepository.Object, _pluginAdditionalConfigRepository.Object);

            var configs = await service.GetByPlugin(1);

            Assert.NotEmpty(configs);
        }

        [Fact]
        public async void GetByPlugin_ReturnEmpty()
        {
            _pluginAdditionalConfigRepository
                .Setup(r => r.GetBySpec(It.IsAny<PluginAdditionalConfigFilterSpecification>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(new List<PluginAdditionalConfig>());

            var service =
                new PluginAdditionalConfigService(_pluginRepository.Object, _pluginAdditionalConfigRepository.Object);

            var configs = await service.GetByPlugin(1);

            Assert.Empty(configs);
        }

        [Fact]
        public async void GetByPluginName_ReturnItems()
        {
            _pluginAdditionalConfigRepository
                .Setup(r => r.GetBySpec(It.IsAny<PluginAdditionalConfigFilterSpecification>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(new List<PluginAdditionalConfig>
                {
                    new PluginAdditionalConfig {Id = 1, PluginId = 1, Plugin = new Plugin { Name = "Plugin1" }, Name = "Config1"}
                });

            var service =
                new PluginAdditionalConfigService(_pluginRepository.Object, _pluginAdditionalConfigRepository.Object);

            var configs = await service.GetByPluginName("Plugin1");

            Assert.NotEmpty(configs);
        }

        [Fact]
        public async void GetByPluginName_ReturnEmpty()
        {
            _pluginAdditionalConfigRepository
                .Setup(r => r.GetBySpec(It.IsAny<PluginAdditionalConfigFilterSpecification>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(new List<PluginAdditionalConfig>());

            var service =
                new PluginAdditionalConfigService(_pluginRepository.Object, _pluginAdditionalConfigRepository.Object);

            var configs = await service.GetByPluginName("Plugin1");

            Assert.Empty(configs);
        }

        [Fact]
        public async void AddAdditionalConfigs_Success()
        {
            _pluginRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => new Plugin { Id = id });

            _pluginAdditionalConfigRepository
                .Setup(r => r.AddRange(It.IsAny<List<PluginAdditionalConfig>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    (List<PluginAdditionalConfig> configs, CancellationToken cancellationToken) =>
                    {
                        var ids = new List<int>();
                        for (int i = 0; i < configs.Count; i++)
                        {
                            ids.Add(i + 1);
                        }

                        return ids;
                    });

            var service =
                new PluginAdditionalConfigService(_pluginRepository.Object, _pluginAdditionalConfigRepository.Object);

            var newConfigs = new List<PluginAdditionalConfig>
            {
                new PluginAdditionalConfig { Name = "Config1" },
                new PluginAdditionalConfig { Name = "Config2" }
            };
            var results = await service.AddAdditionalConfigs(1, newConfigs);

            Assert.Equal(2, results.Count);
        }

        [Fact]
        public void AddAdditionalConfigs_PluginNotFound()
        {
            _pluginRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Plugin)null);

            var service =
                new PluginAdditionalConfigService(_pluginRepository.Object, _pluginAdditionalConfigRepository.Object);
            
            var exception = Record.ExceptionAsync(() => service.AddAdditionalConfigs(1, new List<PluginAdditionalConfig>()));

            Assert.IsType<PluginNotFoundException>(exception?.Result);
        }
    }
}
