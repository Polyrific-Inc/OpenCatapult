// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Api.Controllers;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.UnitTests.Utilities;
using Polyrific.Catapult.Shared.Dto.ExternalServiceType;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Controllers
{
    public class ExternalServiceTypeControllerTests
    {
        private readonly Mock<IExternalServiceTypeService> _externalServiceTypeService;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ExternalServiceTypeController>> _logger;

        public ExternalServiceTypeControllerTests()
        {
            _externalServiceTypeService = new Mock<IExternalServiceTypeService>();

            _mapper = AutoMapperUtils.GetMapper();

            _logger = LoggerMock.GetLogger<ExternalServiceTypeController>();
        }

        [Fact]
        public async void GetExternalServiceTypes_ReturnsExternalServiceTypeList()
        {
            _externalServiceTypeService.Setup(s => s.GetExternalServiceTypes(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ExternalServiceType>
                {
                    new ExternalServiceType
                    {
                        Id = 1,
                        Name = "GitHub"
                    }
                });

            var controller = new ExternalServiceTypeController(_externalServiceTypeService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetExternalServiceTypes(true);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ExternalServiceTypeDto>>(okActionResult.Value);
            Assert.NotEmpty(returnValue);
        }


        [Fact]
        public async void GetExternalServiceTypeById_ReturnsExternalServiceType()
        {
            _externalServiceTypeService.Setup(s => s.GetExternalServiceType(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) =>
                    new ExternalServiceType
                    {
                        Id = 1,
                        Name = "GitHub"
                    });

            var controller = new ExternalServiceTypeController(_externalServiceTypeService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetExternalServiceType(1);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ExternalServiceTypeDto>(okActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void GetExternalServiceTypeByName_ReturnsExternalServiceType()
        {
            _externalServiceTypeService.Setup(s => s.GetExternalServiceTypeByName(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string serviceName, CancellationToken cancellationToken) =>
                    new ExternalServiceType
                    {
                        Id = 1,
                        Name = serviceName
                    });

            var controller = new ExternalServiceTypeController(_externalServiceTypeService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetExternalServiceType("GitHub");

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ExternalServiceTypeDto>(okActionResult.Value);
            Assert.Equal("GitHub", returnValue.Name);
        }
    }
}
