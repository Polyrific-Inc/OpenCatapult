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
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Controllers
{
    public class VersionControllerTests
    {
        private readonly Mock<ICatapultEngineService> _catapultEngineService;
        private readonly Mock<ITaskProviderService> _providerService;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<VersionController>> _logger;

        public VersionControllerTests()
        {
            _catapultEngineService = new Mock<ICatapultEngineService>();
            _providerService = new Mock<ITaskProviderService>();

            _mapper = AutoMapperUtils.GetMapper();

            _logger = LoggerMock.GetLogger<VersionController>();
        }

        public Mock<ICatapultEngineService> CatapultEngineService => _catapultEngineService;

        [Fact]
        public async void Get_ReturnsSuccess()
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
            _providerService.Setup(s => s.GetTaskProviders(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TaskProvider>
                {
                    new TaskProvider
                    {
                        Id = 1,
                        Name = "AspMvcNet"
                    }
                });

            var controller = new VersionController(_catapultEngineService.Object, _providerService.Object, _mapper, _logger.Object);

            var result = await controller.Get();

            Assert.IsType<OkObjectResult>(result);
        }
    }
}
