// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Security.Claims;
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
using Polyrific.Catapult.Shared.Dto.ExternalService;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Controllers
{
    public class ExternalServiceControllerTests
    {
        private readonly Mock<IExternalServiceService> _externalServiceService;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ExternalServiceController>> _logger;

        public ExternalServiceControllerTests()
        {
            _externalServiceService = new Mock<IExternalServiceService>();

            _mapper = AutoMapperUtils.GetMapper();

            _logger = LoggerMock.GetLogger<ExternalServiceController>();
        }

        [Fact]
        public async void GetExternalServices_ReturnsExternalServiceList()
        {
            _externalServiceService.Setup(s => s.GetExternalServices(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ExternalService>
                {
                    new ExternalService
                    {
                        Id = 1,
                        Name = "my connection"
                    }
                });

            var httpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new[]
                {
                    new ClaimsIdentity(new[] {new Claim(ClaimTypes.NameIdentifier, "1")})
                })
            };

            var controller = new ExternalServiceController(_externalServiceService.Object, _mapper,
                _logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };

            var result = await controller.GetExternalServices();

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ExternalServiceDto>>(okActionResult.Value);
            Assert.NotEmpty(returnValue);
        }

        [Fact]
        public async void GetExternalServiceById_ReturnsExternalService()
        {
            _externalServiceService.Setup(s => s.GetExternalService(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) =>
                    new ExternalService
                    {
                        Id = id,
                        Name = "my connection"
                    });

            var controller = new ExternalServiceController(_externalServiceService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetExternalService(1);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ExternalServiceDto>(okActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void GetExternalServiceByName_ReturnsExternalService()
        {
            _externalServiceService.Setup(s => s.GetExternalServiceByName(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string serviceName, CancellationToken cancellationToken) =>
                    new ExternalService
                    {
                        Id = 1,
                        Name = serviceName
                    });

            var controller = new ExternalServiceController(_externalServiceService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetExternalService("my connection");

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ExternalServiceDto>(okActionResult.Value);
            Assert.Equal("my connection", returnValue.Name);
        }

        [Fact]
        public async void CreateExternalService_ReturnsCreatedExternalService()
        {
            _externalServiceService
                .Setup(s => s.AddExternalService(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
            _externalServiceService.Setup(s => s.GetExternalService(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) =>
                    new ExternalService
                    {
                        Id = id,
                        Name = "my connection"
                    });
            
            var httpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new[]
                {
                    new ClaimsIdentity(new[] {new Claim(ClaimTypes.NameIdentifier, "1")})
                })
            };

            var controller = new ExternalServiceController(_externalServiceService.Object, _mapper, _logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };

            var dto = new CreateExternalServiceDto
            {
                Name = "my connection"
            };
            var result = await controller.CreateExternalService(dto);

            var createAtRouteActionResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<ExternalServiceDto>(createAtRouteActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void UpdateExternalService_ReturnsSuccess()
        {
            _externalServiceService.Setup(s => s.UpdateExternalService(It.IsAny<ExternalService>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var controller = new ExternalServiceController(_externalServiceService.Object, _mapper, _logger.Object);

            var result = await controller.UpdateExternalService(1, new UpdateExternalServiceDto());

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void DeleteExternalService_ReturnsNoContent()
        {
            _externalServiceService.Setup(s => s.DeleteExternalService(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var controller = new ExternalServiceController(_externalServiceService.Object, _mapper, _logger.Object);

            var result = await controller.DeleteExternalService(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
