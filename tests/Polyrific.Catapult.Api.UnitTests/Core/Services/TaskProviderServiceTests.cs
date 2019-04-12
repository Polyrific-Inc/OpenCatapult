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
    public class TaskProviderServiceTests
    {
        private readonly Mock<ITaskProviderRepository> _taskProviderRepository;
        private readonly Mock<IExternalServiceTypeRepository> _externalServiceTypeRepository;
        private readonly Mock<ITagRepository> _tagRepository;

        public TaskProviderServiceTests()
        {
            _taskProviderRepository = new Mock<ITaskProviderRepository>();
            _externalServiceTypeRepository = new Mock<IExternalServiceTypeRepository>();
            _tagRepository = new Mock<ITagRepository>();
        }

        [Fact]
        public async void GetTaskProviders_ReturnItems()
        {
            _taskProviderRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<TaskProvider>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TaskProvider>
                {
                    new TaskProvider { Id = 1, Name = "GeneratorProvider1", Type = TaskProviderType.GeneratorProvider },
                    new TaskProvider { Id = 2, Name = "RepositoryProvider1", Type = TaskProviderType.RepositoryProvider },
                    new TaskProvider {Id = 3, Name = "BuildProvider1", Type = TaskProviderType.BuildProvider}
                });

            var service = new TaskProviderService(_taskProviderRepository.Object, _externalServiceTypeRepository.Object, _tagRepository.Object);

            var results = await service.GetTaskProviders();

            Assert.Equal(3, results.Count);
        }

        [Fact]
        public async void GetTaskProvidersByType_ReturnItems()
        {
            _taskProviderRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<TaskProvider>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TaskProvider>
                    {new TaskProvider {Id = 3, Name = "BuildProvider1", Type = TaskProviderType.BuildProvider}});

            var service = new TaskProviderService(_taskProviderRepository.Object, _externalServiceTypeRepository.Object, _tagRepository.Object);

            var results = await service.GetTaskProviders(TaskProviderType.BuildProvider);

            Assert.Single(results);
            Assert.Equal("BuildProvider1", results.First().Name);
        }

        [Fact]
        public async void GetTaskProvidersByType_ReturnEmpty()
        {
            _taskProviderRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<TaskProvider>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TaskProvider>());

            var service = new TaskProviderService(_taskProviderRepository.Object, _externalServiceTypeRepository.Object, _tagRepository.Object);

            var results = await service.GetTaskProviders(TaskProviderType.HostingProvider);

            Assert.Empty(results);
        }

        [Fact]
        public async void GetTaskProviderById_ReturnItem()
        {
            _taskProviderRepository
                .Setup(r => r.GetSingleBySpec(It.IsAny<TaskProviderFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TaskProvider {Id = 1, Name = "GeneratorProvider1"});

            var service = new TaskProviderService(_taskProviderRepository.Object, _externalServiceTypeRepository.Object, _tagRepository.Object);

            var result = await service.GetTaskProviderById(1);

            Assert.NotNull(result);
            Assert.Equal("GeneratorProvider1", result.Name);
        }

        [Fact]
        public async void GetTaskProviderById_ReturnNull()
        {
            _taskProviderRepository
                .Setup(r => r.GetById(4, It.IsAny<CancellationToken>()))
                .ReturnsAsync((TaskProvider)null);

            var service = new TaskProviderService(_taskProviderRepository.Object, _externalServiceTypeRepository.Object, _tagRepository.Object);

            var result = await service.GetTaskProviderById(4);

            Assert.Null(result);
        }

        [Fact]
        public async void GetTaskProviderByName_ReturnItem()
        {
            _taskProviderRepository
                .Setup(r => r.GetSingleBySpec(It.IsAny<ISpecification<TaskProvider>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new TaskProvider { Id = 1, Name = "GeneratorProvider1"});

            var service = new TaskProviderService(_taskProviderRepository.Object, _externalServiceTypeRepository.Object, _tagRepository.Object);

            var result = await service.GetTaskProviderByName("GeneratorProvider1");

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async void GetTaskProviderByName_ReturnNull()
        {
            _taskProviderRepository
                .Setup(r => r.GetSingleBySpec(It.IsAny<ISpecification<TaskProvider>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TaskProvider)null);

            var service = new TaskProviderService(_taskProviderRepository.Object, _externalServiceTypeRepository.Object, _tagRepository.Object);

            var result = await service.GetTaskProviderByName("HostingProvider1");

            Assert.Null(result);
        }

        [Fact]
        public async void AddTaskProvider_Success()
        {
            _taskProviderRepository.Setup(r => r.Create(It.IsAny<TaskProvider>(), It.IsAny<CancellationToken>())).ReturnsAsync(4);
            _taskProviderRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => new TaskProvider {Id = id, Name = "HostingProvider1"});

            var service = new TaskProviderService(_taskProviderRepository.Object, _externalServiceTypeRepository.Object, _tagRepository.Object);

            var result = await service.AddTaskProvider("HostingProvider1", TaskProviderType.HostingProvider, "Frandi", "1.0", null, null, null, null, null, DateTime.UtcNow, null);

            Assert.Equal(4, result.Id);
        }

        [Fact]
        public void AddTaskProvider_RequiredServiceNotSupporedException()
        {
            _taskProviderRepository.Setup(r => r.Create(It.IsAny<TaskProvider>(), It.IsAny<CancellationToken>())).ReturnsAsync(4);
            _taskProviderRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => new TaskProvider { Id = id, Name = "HostingProvider1" });

            var service = new TaskProviderService(_taskProviderRepository.Object, _externalServiceTypeRepository.Object, _tagRepository.Object);

            var exception = Record.ExceptionAsync(() => service.AddTaskProvider("HostingProvider1", TaskProviderType.HostingProvider, "Frandi", "1.0", new string[] { "Service" }, null, null, null, null, DateTime.UtcNow, null));

            Assert.IsType<RequiredServicesNotSupportedException>(exception?.Result);
        }

        [Fact]
        public async void DeleteTaskProvider_Success()
        {
            _taskProviderRepository.Setup(r => r.Delete(1, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var service = new TaskProviderService(_taskProviderRepository.Object, _externalServiceTypeRepository.Object, _tagRepository.Object);

            await service.DeleteTaskProvider(1);

            _taskProviderRepository.Verify(r => r.Delete(1, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
