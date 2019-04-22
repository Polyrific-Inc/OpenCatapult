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
    public class TaskProviderAdditionalConfigServiceTests
    {
        private readonly Mock<ITaskProviderRepository> _taskProviderRepository;
        private readonly Mock<ITaskProviderAdditionalConfigRepository> _taskProviderAdditionalConfigRepository;

        public TaskProviderAdditionalConfigServiceTests()
        {
            _taskProviderAdditionalConfigRepository = new Mock<ITaskProviderAdditionalConfigRepository>();
            _taskProviderRepository = new Mock<ITaskProviderRepository>();
        }

        [Fact]
        public async void GetByTaskProvider_ReturnItems()
        {
            _taskProviderAdditionalConfigRepository
                .Setup(r => r.GetBySpec(It.IsAny<TaskProviderAdditionalConfigFilterSpecification>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(new List<TaskProviderAdditionalConfig>
                {
                    new TaskProviderAdditionalConfig {Id = 1, TaskProviderId = 1, Name = "Config1"}
                });

            var service =
                new TaskProviderAdditionalConfigService(_taskProviderRepository.Object, _taskProviderAdditionalConfigRepository.Object);

            var configs = await service.GetByTaskProvider(1);

            Assert.NotEmpty(configs);
        }

        [Fact]
        public async void GetByTaskProvider_ReturnEmpty()
        {
            _taskProviderAdditionalConfigRepository
                .Setup(r => r.GetBySpec(It.IsAny<TaskProviderAdditionalConfigFilterSpecification>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(new List<TaskProviderAdditionalConfig>());

            var service =
                new TaskProviderAdditionalConfigService(_taskProviderRepository.Object, _taskProviderAdditionalConfigRepository.Object);

            var configs = await service.GetByTaskProvider(1);

            Assert.Empty(configs);
        }

        [Fact]
        public async void GetByTaskProviderName_ReturnItems()
        {
            _taskProviderAdditionalConfigRepository
                .Setup(r => r.GetBySpec(It.IsAny<TaskProviderAdditionalConfigFilterSpecification>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(new List<TaskProviderAdditionalConfig>
                {
                    new TaskProviderAdditionalConfig {Id = 1, TaskProviderId = 1, TaskProvider = new TaskProvider { Name = "Provider1" }, Name = "Config1"}
                });

            var service =
                new TaskProviderAdditionalConfigService(_taskProviderRepository.Object, _taskProviderAdditionalConfigRepository.Object);

            var configs = await service.GetByTaskProviderName("Provider1");

            Assert.NotEmpty(configs);
        }

        [Fact]
        public async void GetByTaskProviderName_ReturnEmpty()
        {
            _taskProviderAdditionalConfigRepository
                .Setup(r => r.GetBySpec(It.IsAny<TaskProviderAdditionalConfigFilterSpecification>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(new List<TaskProviderAdditionalConfig>());

            var service =
                new TaskProviderAdditionalConfigService(_taskProviderRepository.Object, _taskProviderAdditionalConfigRepository.Object);

            var configs = await service.GetByTaskProviderName("Provider1");

            Assert.Empty(configs);
        }

        [Fact]
        public async void AddAdditionalConfigs_Success()
        {
            _taskProviderRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => new TaskProvider { Id = id });

            _taskProviderAdditionalConfigRepository
                .Setup(r => r.AddRange(It.IsAny<List<TaskProviderAdditionalConfig>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    (List<TaskProviderAdditionalConfig> configs, CancellationToken cancellationToken) =>
                    {
                        var ids = new List<int>();
                        for (int i = 0; i < configs.Count; i++)
                        {
                            ids.Add(i + 1);
                        }

                        return ids;
                    });

            var service =
                new TaskProviderAdditionalConfigService(_taskProviderRepository.Object, _taskProviderAdditionalConfigRepository.Object);

            var newConfigs = new List<TaskProviderAdditionalConfig>
            {
                new TaskProviderAdditionalConfig { Name = "Config1" },
                new TaskProviderAdditionalConfig { Name = "Config2" }
            };
            var results = await service.AddAdditionalConfigs(1, newConfigs);

            Assert.Equal(2, results.Count);
        }

        [Fact]
        public void AddAdditionalConfigs_ProviderNotFound()
        {
            _taskProviderRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TaskProvider)null);

            var service =
                new TaskProviderAdditionalConfigService(_taskProviderRepository.Object, _taskProviderAdditionalConfigRepository.Object);
            
            var exception = Record.ExceptionAsync(() => service.AddAdditionalConfigs(1, new List<TaskProviderAdditionalConfig>()));

            Assert.IsType<TaskProviderNotFoundException>(exception?.Result);
        }
    }
}
