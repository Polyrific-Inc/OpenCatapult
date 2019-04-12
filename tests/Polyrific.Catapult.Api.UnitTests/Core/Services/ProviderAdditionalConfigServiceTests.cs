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
    public class ProviderAdditionalConfigServiceTests
    {
        private readonly Mock<IProviderRepository> _providerRepository;
        private readonly Mock<IProviderAdditionalConfigRepository> _providerAdditionalConfigRepository;

        public ProviderAdditionalConfigServiceTests()
        {
            _providerAdditionalConfigRepository = new Mock<IProviderAdditionalConfigRepository>();
            _providerRepository = new Mock<IProviderRepository>();
        }

        [Fact]
        public async void GetByProvider_ReturnItems()
        {
            _providerAdditionalConfigRepository
                .Setup(r => r.GetBySpec(It.IsAny<ProviderAdditionalConfigFilterSpecification>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(new List<ProviderAdditionalConfig>
                {
                    new ProviderAdditionalConfig {Id = 1, ProviderId = 1, Name = "Config1"}
                });

            var service =
                new ProviderAdditionalConfigService(_providerRepository.Object, _providerAdditionalConfigRepository.Object);

            var configs = await service.GetByProvider(1);

            Assert.NotEmpty(configs);
        }

        [Fact]
        public async void GetByProvider_ReturnEmpty()
        {
            _providerAdditionalConfigRepository
                .Setup(r => r.GetBySpec(It.IsAny<ProviderAdditionalConfigFilterSpecification>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(new List<ProviderAdditionalConfig>());

            var service =
                new ProviderAdditionalConfigService(_providerRepository.Object, _providerAdditionalConfigRepository.Object);

            var configs = await service.GetByProvider(1);

            Assert.Empty(configs);
        }

        [Fact]
        public async void GetByProviderName_ReturnItems()
        {
            _providerAdditionalConfigRepository
                .Setup(r => r.GetBySpec(It.IsAny<ProviderAdditionalConfigFilterSpecification>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(new List<ProviderAdditionalConfig>
                {
                    new ProviderAdditionalConfig {Id = 1, ProviderId = 1, Provider = new Provider { Name = "Provider1" }, Name = "Config1"}
                });

            var service =
                new ProviderAdditionalConfigService(_providerRepository.Object, _providerAdditionalConfigRepository.Object);

            var configs = await service.GetByProviderName("Provider1");

            Assert.NotEmpty(configs);
        }

        [Fact]
        public async void GetByProviderName_ReturnEmpty()
        {
            _providerAdditionalConfigRepository
                .Setup(r => r.GetBySpec(It.IsAny<ProviderAdditionalConfigFilterSpecification>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(new List<ProviderAdditionalConfig>());

            var service =
                new ProviderAdditionalConfigService(_providerRepository.Object, _providerAdditionalConfigRepository.Object);

            var configs = await service.GetByProviderName("Provider1");

            Assert.Empty(configs);
        }

        [Fact]
        public async void AddAdditionalConfigs_Success()
        {
            _providerRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => new Provider { Id = id });

            _providerAdditionalConfigRepository
                .Setup(r => r.AddRange(It.IsAny<List<ProviderAdditionalConfig>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    (List<ProviderAdditionalConfig> configs, CancellationToken cancellationToken) =>
                    {
                        var ids = new List<int>();
                        for (int i = 0; i < configs.Count; i++)
                        {
                            ids.Add(i + 1);
                        }

                        return ids;
                    });

            var service =
                new ProviderAdditionalConfigService(_providerRepository.Object, _providerAdditionalConfigRepository.Object);

            var newConfigs = new List<ProviderAdditionalConfig>
            {
                new ProviderAdditionalConfig { Name = "Config1" },
                new ProviderAdditionalConfig { Name = "Config2" }
            };
            var results = await service.AddAdditionalConfigs(1, newConfigs);

            Assert.Equal(2, results.Count);
        }

        [Fact]
        public void AddAdditionalConfigs_ProviderNotFound()
        {
            _providerRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Provider)null);

            var service =
                new ProviderAdditionalConfigService(_providerRepository.Object, _providerAdditionalConfigRepository.Object);
            
            var exception = Record.ExceptionAsync(() => service.AddAdditionalConfigs(1, new List<ProviderAdditionalConfig>()));

            Assert.IsType<ProviderNotFoundException>(exception?.Result);
        }
    }
}
