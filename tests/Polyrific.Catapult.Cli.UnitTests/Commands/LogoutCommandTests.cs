// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Moq;
using Polyrific.Catapult.Cli.Commands;
using Polyrific.Catapult.Shared.Dto.User;
using System.Collections.Generic;
using Xunit;

namespace Polyrific.Catapult.Cli.UnitTests.Commands
{
    public class LogoutCommandTests
    {
        private readonly Mock<IConsole> _console;
        private readonly Mock<ITokenStore> _tokenStore;

        public LogoutCommandTests()
        {
            var users = new List<UserDto>
            {
                new UserDto
                {
                    Id = "1",
                    Email = "user1@opencatapult.net"
                }
            };

            _console = new Mock<IConsole>();

            _tokenStore = new Mock<ITokenStore>();
        }

        [Fact]
        public void Login_Execute_ReturnsSuccessMessage()
        {
            var command = new LogoutCommand(_console.Object, LoggerMock.GetLogger<LogoutCommand>().Object, _tokenStore.Object);

            var resultMessage = command.Execute();

            Assert.Equal("Logged out", resultMessage);
            _tokenStore.Verify(s => s.DeleteToken(), Times.Once);
        }
    }
}
