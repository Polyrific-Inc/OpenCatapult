// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Core.Specifications;
using Polyrific.Catapult.Shared.Dto.Constants;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Core.Services
{
    public class ProviderServiceTests
    {
        private readonly Mock<IProviderRepository> _providerRepository;
        private readonly Mock<IExternalServiceTypeRepository> _externalServiceTypeRepository;
        private readonly Mock<ITagRepository> _tagRepository;

        public ProviderServiceTests()
        {
            _providerRepository = new Mock<IProviderRepository>();
            _externalServiceTypeRepository = new Mock<IExternalServiceTypeRepository>();
            _tagRepository = new Mock<ITagRepository>();
        }

        [Fact]
        public async void GetProviders_ReturnItems()
        {
            _providerRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<Provider>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Provider>
                {
                    new Provider { Id = 1, Name = "GeneratorProvider1", Type = ProviderType.GeneratorProvider },
                    new Provider { Id = 2, Name = "RepositoryProvider1", Type = ProviderType.RepositoryProvider },
                    new Provider {Id = 3, Name = "BuildProvider1", Type = ProviderType.BuildProvider}
                });

            var service = new ProviderService(_providerRepository.Object, _externalServiceTypeRepository.Object, _tagRepository.Object);

            var results = await service.GetProviders();

            Assert.Equal(3, results.Count);
        }

        [Fact]
        public async void GetProvidersByType_ReturnItems()
        {
            _providerRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<Provider>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Provider>
                    {new Provider {Id = 3, Name = "BuildProvider1", Type = ProviderType.BuildProvider}});

            var service = new ProviderService(_providerRepository.Object, _externalServiceTypeRepository.Object, _tagRepository.Object);

            var results = await service.GetProviders(ProviderType.BuildProvider);

            Assert.Single(results);
            Assert.Equal("BuildProvider1", results.First().Name);
        }

        [Fact]
        public async void GetProvidersByType_ReturnEmpty()
        {
            _providerRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<Provider>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Provider>());

            var service = new ProviderService(_providerRepository.Object, _externalServiceTypeRepository.Object, _tagRepository.Object);

            var results = await service.GetProviders(ProviderType.HostingProvider);

            Assert.Empty(results);
        }

        [Fact]
        public async void GetProviderById_ReturnItem()
        {
            _providerRepository
                .Setup(r => r.GetSingleBySpec(It.IsAny<ProviderFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Provider {Id = 1, Name = "GeneratorProvider1"});

            var service = new ProviderService(_providerRepository.Object, _externalServiceTypeRepository.Object, _tagRepository.Object);

            var result = await service.GetProviderById(1);

            Assert.NotNull(result);
            Assert.Equal("GeneratorProvider1", result.Name);
        }

        [Fact]
        public async void GetProviderById_ReturnNull()
        {
            _providerRepository
                .Setup(r => r.GetById(4, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Provider)null);

            var service = new ProviderService(_providerRepository.Object, _externalServiceTypeRepository.Object, _tagRepository.Object);

            var result = await service.GetProviderById(4);

            Assert.Null(result);
        }

        [Fact]
        public async void GetProviderByName_ReturnItem()
        {
            _providerRepository
                .Setup(r => r.GetSingleBySpec(It.IsAny<ISpecification<Provider>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new Provider { Id = 1, Name = "GeneratorProvider1"});

            var service = new ProviderService(_providerRepository.Object, _externalServiceTypeRepository.Object, _tagRepository.Object);

            var result = await service.GetProviderByName("GeneratorProvider1");

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async void GetProviderByName_ReturnNull()
        {
            _providerRepository
                .Setup(r => r.GetSingleBySpec(It.IsAny<ISpecification<Provider>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Provider)null);

            var service = new ProviderService(_providerRepository.Object, _externalServiceTypeRepository.Object, _tagRepository.Object);

            var result = await service.GetProviderByName("HostingProvider1");

            Assert.Null(result);
        }

        [Fact]
        public async void AddProvider_Success()
        {
            _providerRepository.Setup(r => r.Create(It.IsAny<Provider>(), It.IsAny<CancellationToken>())).ReturnsAsync(4);
            _providerRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => new Provider {Id = id, Name = "HostingProvider1"});

            var service = new ProviderService(_providerRepository.Object, _externalServiceTypeRepository.Object, _tagRepository.Object);

            var result = await service.AddProvider("HostingProvider1", ProviderType.HostingProvider, "Frandi", "1.0", null, null, null, null, null, DateTime.UtcNow, null);

            Assert.Equal(4, result.Id);
        }

        [Fact]
        public void AddProvider_RequiredServiceNotSupporedException()
        {
            _providerRepository.Setup(r => r.Create(It.IsAny<Provider>(), It.IsAny<CancellationToken>())).ReturnsAsync(4);
            _providerRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => new Provider { Id = id, Name = "HostingProvider1" });

            var service = new ProviderService(_providerRepository.Object, _externalServiceTypeRepository.Object, _tagRepository.Object);

            var exception = Record.ExceptionAsync(() => service.AddProvider("HostingProvider1", ProviderType.HostingProvider, "Frandi", "1.0", new string[] { "Service" }, null, null, null, null, DateTime.UtcNow, null));

            Assert.IsType<RequiredServicesNotSupportedException>(exception?.Result);
        }

        [Fact]
        public async void DeleteProvider_Success()
        {
            _providerRepository.Setup(r => r.Delete(1, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var service = new ProviderService(_providerRepository.Object, _externalServiceTypeRepository.Object, _tagRepository.Object);

            await service.DeleteProvider(1);

            _providerRepository.Verify(r => r.Delete(1, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
