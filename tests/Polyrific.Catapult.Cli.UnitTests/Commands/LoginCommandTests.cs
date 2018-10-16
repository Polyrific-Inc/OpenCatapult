// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using McMaster.Extensions.CommandLineUtils;
using Moq;
using Polyrific.Catapult.Cli.Commands;
using Polyrific.Catapult.Cli.UnitTests.Commands.Utilities;
using Polyrific.Catapult.Shared.Dto.User;
using Polyrific.Catapult.Shared.Service;
using Xunit;
using Xunit.Abstractions;

namespace Polyrific.Catapult.Cli.UnitTests.Commands
{
    public class LoginCommandTests
    {
        private readonly IConsole _console;
        private readonly Mock<ITokenStore> _tokenStore;
        private readonly Mock<ITokenService> _tokenService;
        private readonly Mock<IConsoleReader> _consoleReader;

        public LoginCommandTests(ITestOutputHelper output)
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
            _tokenService = new Mock<ITokenService>();
            _tokenService.Setup(t => t.RequestToken(It.IsAny<RequestTokenDto>())).ReturnsAsync("testToken");

            _tokenStore = new Mock<ITokenStore>();
            _tokenStore.Setup(s => s.GetSavedToken()).ReturnsAsync("testToken");

            _consoleReader = new Mock<IConsoleReader>();
        }

        [Fact]
        public void Login_Execute_ReturnsSuccessMessage()
        {
            var command = new LoginCommand(_console, LoggerMock.GetLogger<LoginCommand>().Object, _tokenService.Object, _tokenStore.Object, _consoleReader.Object)
            {
                Username = "user1@opencatapult.net"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Logged in as user1@opencatapult.net", resultMessage);
            _tokenStore.Verify(s => s.SaveToken("testToken"), Times.Once);
        }
    }
}
