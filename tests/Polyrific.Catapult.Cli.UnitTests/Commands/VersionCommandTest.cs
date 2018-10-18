// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Moq;
using Polyrific.Catapult.Cli.Commands;
using Polyrific.Catapult.Cli.UnitTests.Commands.Utilities;
using Polyrific.Catapult.Shared.Service;
using Xunit;
using Xunit.Abstractions;

namespace Polyrific.Catapult.Cli.UnitTests.Commands
{
    public class VersionCommandTest
    {
        private readonly IConsole _console;
        private readonly Mock<IVersionService> _versionService;

        public VersionCommandTest(ITestOutputHelper output)
        {
            _console = new TestConsole(output);

            _versionService = new Mock<IVersionService>();
            _versionService.Setup(s => s.GetApiVersion()).ReturnsAsync("1.0");
        }

        [Fact]
        public void Version_Execute_ReturnString()
        {
            var command = new VersionCommand(_console, LoggerMock.GetLogger<VersionCommand>().Object, _versionService.Object);

            var resultMessage = command.Execute();

            Assert.Contains("API Version: 1.0", resultMessage);
            Assert.Contains("CLI Version:", resultMessage);
        }
    }
}
