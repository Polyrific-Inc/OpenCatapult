// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Moq;
using Polyrific.Catapult.Cli.Commands;
using Polyrific.Catapult.Shared.Dto.User;
using Polyrific.Catapult.Shared.Service;
using System.Collections.Generic;
using Xunit;

namespace Polyrific.Catapult.Cli.UnitTests.Commands
{
    public class LoginCommandTests
    {
        private readonly Mock<IConsole> _console;
        private readonly Mock<ITokenStore> _tokenStore;
        private readonly Mock<ITokenService> _tokenService;

        public LoginCommandTests()
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
            _tokenService = new Mock<ITokenService>();
            _tokenService.Setup(t => t.RequestToken(It.IsAny<RequestTokenDto>())).ReturnsAsync("testToken");

            _tokenStore = new Mock<ITokenStore>();
            _tokenStore.Setup(s => s.GetSavedToken()).ReturnsAsync("testToken");
        }

        [Fact]
        public void Login_Execute_ReturnsSuccessMessage()
        {
            var command = new LoginCommand(_console.Object, LoggerMock.GetLogger<LoginCommand>().Object, _tokenService.Object, _tokenStore.Object)
            {
                Username = "user1@opencatapult.net"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Logged in as user1@opencatapult.net", resultMessage);
            _tokenStore.Verify(s => s.SaveToken("testToken"), Times.Once);
        }
    }
}
