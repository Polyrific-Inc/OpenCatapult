// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Api.Controllers;
using Polyrific.Catapult.Api.UnitTests.Utilities;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Controllers
{
    public class HealthControllerTests
    {
        private readonly Mock<ILogger<HealthController>> _logger;

        public HealthControllerTests()
        {
            _logger = LoggerMock.GetLogger<HealthController>();
        }

        [Fact]
        public void Get_ReturnsSuccess()
        {
            var controller = new HealthController(_logger.Object);

            var result = controller.Get();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetSecure_ReturnsSuccess()
        {
            var controller = new HealthController(_logger.Object);

            var result = controller.GetSecure();

            Assert.IsType<OkObjectResult>(result);
        }
    }
}
