// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Api.Controllers;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.UnitTests.Utilities;
using Polyrific.Catapult.Shared.Dto.CatapultEngine;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Controllers
{
    public class CatapultEngineControllerTests
    {
        private readonly Mock<ICatapultEngineService> _catapultEngineService;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<CatapultEngineController>> _logger;

        public CatapultEngineControllerTests()
        {
            _catapultEngineService = new Mock<ICatapultEngineService>();

            _mapper = AutoMapperUtils.GetMapper();

            _logger = LoggerMock.GetLogger<CatapultEngineController>();
        }

        [Fact]
        public async void RegisterUser_ReturnsRegisteredUser()
        {
            _catapultEngineService
                .Setup(s => s.CreateCatapultEngine(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string name, CancellationToken cancellationToken) =>
                    new CatapultEngine
                    {
                        Id = 1,
                        Name = name
                    });
            _catapultEngineService.Setup(s => s.GenerateConfirmationToken(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync("xxx");
            _catapultEngineService.Setup(s => s.ConfirmRegistration(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var controller = new CatapultEngineController(_catapultEngineService.Object, _mapper, _logger.Object);

            var dto = new RegisterCatapultEngineDto
            {
                Name = "Engine01"
            };
            var result = await controller.RegisterEngine(dto);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CatapultEngineDto>(okActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void ConfirmRegistration_ReturnsSuccess()
        {
            _catapultEngineService.Setup(s => s.ConfirmRegistration(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var controller = new CatapultEngineController(_catapultEngineService.Object, _mapper, _logger.Object);

            var result = await controller.ConfirmRegistration(1, "xxx");

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Suspend_ReturnsSuccess()
        {
            _catapultEngineService.Setup(s => s.Suspend(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var controller = new CatapultEngineController(_catapultEngineService.Object, _mapper, _logger.Object);

            var result = await controller.Suspend(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Reactivate_ReturnsSuccess()
        {
            _catapultEngineService.Setup(s => s.Reactivate(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var controller = new CatapultEngineController(_catapultEngineService.Object, _mapper, _logger.Object);

            var result = await controller.Reactivate(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetCatapultEngines_ReturnsCatapultEngineList()
        {
            _catapultEngineService.Setup(s => s.GetCatapultEngines(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CatapultEngine>
                {
                    new CatapultEngine
                    {
                        Id = 1,
                        Name = "Engine01"
                    }
                });

            var controller = new CatapultEngineController(_catapultEngineService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetCatapultEngines(null);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<CatapultEngineDto>>(okActionResult.Value);
            Assert.NotEmpty(returnValue);
        }

        [Fact]
        public async void GetCatapultEngineByName_ReturnsCatapultEngine()
        {
            _catapultEngineService.Setup(s => s.GetCatapultEngine(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string engineName, CancellationToken cancellationToken) =>
                    new CatapultEngine
                    {
                        Id = 1,
                        Name = engineName
                    });

            var controller = new CatapultEngineController(_catapultEngineService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetCatapultEngineByName("Engine01");

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CatapultEngineDto>(okActionResult.Value);
            Assert.Equal("Engine01", returnValue.Name);
        }

        [Fact]
        public async void RemoveUser_ReturnsNoContent()
        {
            _catapultEngineService.Setup(s => s.DeleteCatapultEngine(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var controller = new CatapultEngineController(_catapultEngineService.Object, _mapper,  _logger.Object);

            var result = await controller.RemoveCatapultEngine(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
