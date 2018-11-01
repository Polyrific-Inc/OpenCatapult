// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Api.Controllers;
using Polyrific.Catapult.Api.UnitTests.Utilities;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Controllers
{
    public class VersionControllerTests
    {
        private readonly Mock<ILogger<VersionController>> _logger;

        public VersionControllerTests()
        {
            _logger = LoggerMock.GetLogger<VersionController>();
        }

        [Fact]
        public void Get_ReturnsSuccess()
        {
            var controller = new VersionController(_logger.Object);

            var result = controller.Get();

            Assert.IsType<OkObjectResult>(result);
        }
    }
}
