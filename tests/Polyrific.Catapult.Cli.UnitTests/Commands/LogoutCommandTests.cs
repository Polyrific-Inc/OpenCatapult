// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using McMaster.Extensions.CommandLineUtils;
using Moq;
using Polyrific.Catapult.Cli.Commands;
using Polyrific.Catapult.Cli.UnitTests.Commands.Utilities;
using Polyrific.Catapult.Shared.Dto.User;
using Xunit;
using Xunit.Abstractions;

namespace Polyrific.Catapult.Cli.UnitTests.Commands
{
    public class LogoutCommandTests
    {
        private readonly IConsole _console;
        private readonly Mock<ITokenStore> _tokenStore;

        public LogoutCommandTests(ITestOutputHelper output)
        {
            var users = new List<UserDto>
            {
                new UserDto
                {
                    Id = "1",
                    Email = "user1@opencatapult.net"
                }
            };

            _console = new TestConsole(output);

            _tokenStore = new Mock<ITokenStore>();
        }

        [Fact]
        public void Login_Execute_ReturnsSuccessMessage()
        {
            var command = new LogoutCommand(_console, LoggerMock.GetLogger<LogoutCommand>().Object, _tokenStore.Object);

            var resultMessage = command.Execute();

            Assert.Equal("Logged out, bye!", resultMessage);
            _tokenStore.Verify(s => s.DeleteToken(), Times.Once);
        }
    }
}
