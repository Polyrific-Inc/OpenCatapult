// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class CatapultEngineServiceTests
    {
        private readonly List<CatapultEngine> _data;
        private readonly Mock<ICatapultEngineRepository> _catapultEngineRepository;

        public CatapultEngineServiceTests()
        {
            _data = new List<CatapultEngine>
            {
                new CatapultEngine
                {
                    Id = 1,
                    Name = "test"
                }
            };

            _catapultEngineRepository = new Mock<ICatapultEngineRepository>();
            _catapultEngineRepository.Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Callback((int id, CancellationToken cancellationToken) =>
                {
                    var entity = _data.FirstOrDefault(d => d.Id == id);
                    if (entity != null)
                        _data.Remove(entity);
                });
            _catapultEngineRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => _data.FirstOrDefault(d => d.Id == id));
            _catapultEngineRepository.Setup(r => r.GetByCatapultEngineName(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string name, CancellationToken cancellationToken) => _data.FirstOrDefault(d => d.Name == name));
            _catapultEngineRepository.Setup(r => r.GetCatapultEngineRole(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(UserRole.Engine);
            _catapultEngineRepository.Setup(r => r.GetByPrincipal(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ClaimsPrincipal principal, CancellationToken cancellationToken) => _data.FirstOrDefault());
            _catapultEngineRepository.Setup(r => r.GenerateConfirmationToken(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("test");
            _catapultEngineRepository.Setup(r => r.ValidateCatapultEnginePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _catapultEngineRepository.Setup(r => r.GetAll(It.IsAny<bool?>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_data);
            _catapultEngineRepository.Setup(r => r.Create(It.IsAny<CatapultEngine>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(2).Callback((CatapultEngine entity, CancellationToken cancellationToken) =>
                {
                    entity.Id = 2;
                    _data.Add(entity);
                });
            _catapultEngineRepository.Setup(r => r.Update(It.IsAny<CatapultEngine>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Callback((CatapultEngine entity, CancellationToken cancellationToken) =>
                {
                    var oldEntity = _data.FirstOrDefault(d => d.Id == entity.Id);
                    if (oldEntity != null)
                    {
                        _data.Remove(oldEntity);
                        _data.Add(entity);
                    }
                });
        }

        [Fact]
        public async void AddCatapultEngine_ValidItem()
        {
            var catapultEngineService = new CatapultEngineService(_catapultEngineRepository.Object);
            var newCatapultEngine = await catapultEngineService.CreateCatapultEngine("enginetest");

            Assert.NotNull(newCatapultEngine);
            Assert.True(_data.Count > 1);
        }

        [Fact]
        public void AddCatapultEngine_DuplicateItem()
        {
            var catapultEngineService = new CatapultEngineService(_catapultEngineRepository.Object);
            var exception = Record.ExceptionAsync(() => catapultEngineService.CreateCatapultEngine("test"));

            Assert.IsType<DuplicateCatapultEngineException>(exception?.Result);
        }

        [Fact]
        public async void GetCatapultEngines_ReturnItems()
        {
            var catapultEngineService = new CatapultEngineService(_catapultEngineRepository.Object);
            var catapultEngines = await catapultEngineService.GetCatapultEngines(null);

            Assert.NotEmpty(catapultEngines);
        }

        [Fact]
        public async void GetCatapultEngines_ReturnEmpty()
        {
            _data.Clear();

            var catapultEngineService = new CatapultEngineService(_catapultEngineRepository.Object);
            var catapultEngines = await catapultEngineService.GetCatapultEngines(null);

            Assert.Empty(catapultEngines);
        }

        [Fact]
        public async void GetCatapultEngine_ReturnItem()
        {
            var catapultEngineService = new CatapultEngineService(_catapultEngineRepository.Object);
            var projectCatapultEngine = await catapultEngineService.GetCatapultEngine(1);

            Assert.NotNull(projectCatapultEngine);
            Assert.Equal(1, projectCatapultEngine.Id);
        }

        [Fact]
        public async void GetCatapultEngine_ReturnNull()
        {
            var catapultEngineService = new CatapultEngineService(_catapultEngineRepository.Object);
            var catapultEngine = await catapultEngineService.GetCatapultEngine(2);

            Assert.Null(catapultEngine);
        }

        [Fact]
        public async void DeleteCatapultEngine_ValidItem()
        {
            var catapultEngineService = new CatapultEngineService(_catapultEngineRepository.Object);
            await catapultEngineService.DeleteCatapultEngine(1);

            Assert.Empty(_data);
        }

        [Fact]
        public async void GenerateConfirmationToken_ReturnToken()
        {
            var catapultEngineService = new CatapultEngineService(_catapultEngineRepository.Object);
            var token = await catapultEngineService.GenerateConfirmationToken(1);

            Assert.NotNull(token);
        }

        [Fact]
        public async void ConfirmRegistration_Success()
        {
            var catapultEngineService = new CatapultEngineService(_catapultEngineRepository.Object);
            await catapultEngineService.ConfirmRegistration(1, "test");

            _catapultEngineRepository.Verify(r => r.ConfirmRegistration(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async void Suspend_Success()
        {
            var catapultEngineService = new CatapultEngineService(_catapultEngineRepository.Object);
            await catapultEngineService.Suspend(1);

            _catapultEngineRepository.Verify(r => r.Suspend(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async void Reactivate_Success()
        {
            var catapultEngineService = new CatapultEngineService(_catapultEngineRepository.Object);
            await catapultEngineService.Reactivate(1);

            _catapultEngineRepository.Verify(r => r.Reactivate(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async void GetCatapultEngineId_ReturnId()
        {
            var catapultEngineService = new CatapultEngineService(_catapultEngineRepository.Object);
            var id = await catapultEngineService.GetCatapultEngineId(new ClaimsPrincipal());

            Assert.NotEqual(0, id);
        }

        [Fact]
        public async void ValidateCatapultEnginePassword_ReturnValid()
        {
            var catapultEngineService = new CatapultEngineService(_catapultEngineRepository.Object);
            var result = await catapultEngineService.ValidateCatapultEnginePassword("test", "test");

            Assert.True(result);
        }

        [Fact]
        public async void GetCatapultEngineRole_ReturnEngineRole()
        {
            var catapultEngineService = new CatapultEngineService(_catapultEngineRepository.Object);
            var result = await catapultEngineService.GetCatapultEngineRole(1);

            Assert.Equal(UserRole.Engine, result);
        }

        [Fact]
        public async void UpdateLastSeen_Success()
        {
            var date1 = DateTime.UtcNow;

            var catapultEngineService = new CatapultEngineService(_catapultEngineRepository.Object);
            await catapultEngineService.UpdateLastSeen("test", "1.0.0");

            var date2 = DateTime.UtcNow;
            var testEngine = _data.First();

            Assert.NotNull(testEngine.LastSeen);
            Assert.InRange(testEngine.LastSeen.Value, date1, date2);
        }
    }
}
