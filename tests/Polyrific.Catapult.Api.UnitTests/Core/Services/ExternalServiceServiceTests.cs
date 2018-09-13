// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Moq;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Core.Specifications;
using Polyrific.Catapult.Shared.Common.Interface;
using Polyrific.Catapult.Shared.Dto.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Core.Services
{
    public class ExternalServiceServiceTests
    {
        private readonly List<ExternalService> _data;
        private readonly Mock<IExternalServiceRepository> _externalServiceRepository;
        private readonly Mock<ISecretVault> _secretVault;

        public ExternalServiceServiceTests()
        {
            _data = new List<ExternalService>
            {
                new ExternalService
                {
                    Id = 1,
                    Name = "Github-Default",
                    Description = "The default github service",
                    Type = "Github",
                    UserId = 1
                }
            };

            _externalServiceRepository = new Mock<IExternalServiceRepository>();
            _externalServiceRepository.Setup(r =>
                    r.GetBySpec(It.IsAny<ExternalServiceFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ExternalServiceFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.Where(spec.Criteria.Compile()));
            _externalServiceRepository.Setup(r =>
                    r.GetSingleBySpec(It.IsAny<ExternalServiceFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ExternalServiceFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.FirstOrDefault(spec.Criteria.Compile()));
            _externalServiceRepository.Setup(r =>
                    r.CountBySpec(It.IsAny<ExternalServiceFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ExternalServiceFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.Count(spec.Criteria.Compile()));
            _externalServiceRepository.Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Callback((int id, CancellationToken cancellationToken) =>
                {
                    var entity = _data.FirstOrDefault(d => d.Id == id);
                    if (entity != null)
                        _data.Remove(entity);
                });
            _externalServiceRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => _data.FirstOrDefault(d => d.Id == id));
            _externalServiceRepository.Setup(r => r.Create(It.IsAny<ExternalService>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(2).Callback((ExternalService entity, CancellationToken cancellationToken) =>
                {
                    entity.Id = 2;
                    _data.Add(entity);
                });
            _externalServiceRepository.Setup(r => r.Update(It.IsAny<ExternalService>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Callback((ExternalService entity, CancellationToken cancellationToken) =>
                {
                    var oldEntity = _data.FirstOrDefault(d => d.Id == entity.Id);
                    if (oldEntity != null)
                    {
                        _data.Remove(oldEntity);
                        _data.Add(entity);
                    }
                });
            
            _secretVault = new Mock<ISecretVault>();
        }

        [Fact]
        public async void AddExternalService_ValidItem()
        {
            var externalServiceService = new ExternalServiceService(_externalServiceRepository.Object, _secretVault.Object);
            int newId = await externalServiceService.AddExternalService("vstsBuild", null, null, "config", 1);

            Assert.True(newId > 1);
            Assert.True(_data.Count > 1);
            _secretVault.Verify(s => s.Add("vstsBuild", "config", It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void AddExternalService_InvalidProject()
        {
            var externalServiceService = new ExternalServiceService(_externalServiceRepository.Object, _secretVault.Object);
            var exception = Record.ExceptionAsync(() => externalServiceService.AddExternalService("Github-Default", null, null, "config", 1));

            Assert.IsType<DuplicateExternalServiceException>(exception?.Result);
        }

        [Fact]
        public async void GetExternalServices_ReturnItems()
        {
            var externalServiceService = new ExternalServiceService(_externalServiceRepository.Object, _secretVault.Object);
            var externalServices = await externalServiceService.GetExternalServices(1);

            Assert.NotEmpty(externalServices);
        }

        [Fact]
        public async void GetExternalServices_ReturnEmpty()
        {
            var externalServiceService = new ExternalServiceService(_externalServiceRepository.Object, _secretVault.Object);
            var externalServices = await externalServiceService.GetExternalServices(2);

            Assert.Empty(externalServices);
        }
        
        [Fact]
        public async void GetExternalService_ReturnItem()
        {
            var externalServiceService = new ExternalServiceService(_externalServiceRepository.Object, _secretVault.Object);
            var entity = await externalServiceService.GetExternalService(1);

            Assert.NotNull(entity);
            Assert.Equal(1, entity.Id);
            _secretVault.Verify(s => s.Get("Github-Default", It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async void GetExternalService_ReturnNull()
        {
            var externalServiceService = new ExternalServiceService(_externalServiceRepository.Object, _secretVault.Object);
            var externalService = await externalServiceService.GetExternalService(2);

            Assert.Null(externalService);
        }

        [Fact]
        public async void GetExternalServiceByName_ReturnItem()
        {
            var externalServiceService = new ExternalServiceService(_externalServiceRepository.Object, _secretVault.Object);
            var entity = await externalServiceService.GetExternalServiceByName("Github-Default");

            Assert.NotNull(entity);
            Assert.Equal(1, entity.Id);
            _secretVault.Verify(s => s.Get("Github-Default", It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async void GetExternalServiceByName_ReturnNull()
        {
            var externalServiceService = new ExternalServiceService(_externalServiceRepository.Object, _secretVault.Object);
            var externalService = await externalServiceService.GetExternalServiceByName("vstsBuild");

            Assert.Null(externalService);
        }

        [Fact]
        public async void UpdateExternalService_ValidItem()
        {
            var externalServiceService = new ExternalServiceService(_externalServiceRepository.Object, _secretVault.Object);
            await externalServiceService.UpdateExternalService(new ExternalService
            {
                Id = 1,
                Description = "Updated description",
                ConfigString = "Updated Config"
            });

            var externalService = _data.First(d => d.Id == 1);

            Assert.Equal("Updated description", externalService.Description);
            _secretVault.Verify(s => s.Update("Github-Default", "Updated Config", It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async void DeleteExternalService_ValidItem()
        {
            var externalServiceService = new ExternalServiceService(_externalServiceRepository.Object, _secretVault.Object);
            await externalServiceService.DeleteExternalService(1);

            Assert.Empty(_data);
            _secretVault.Verify(s => s.Delete("Github-Default", It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}