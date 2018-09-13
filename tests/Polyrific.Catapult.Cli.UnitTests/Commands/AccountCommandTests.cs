// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Moq;
using Polyrific.Catapult.Cli.Commands;
using Polyrific.Catapult.Cli.Commands.Account;
using Polyrific.Catapult.Shared.Dto.User;
using Polyrific.Catapult.Shared.Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Polyrific.Catapult.Cli.UnitTests.Commands
{
    public class AccountCommandTests
    {
        private readonly Mock<IConsole> _console;
        private readonly Mock<IAccountService> _accountService;

        public AccountCommandTests()
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

            _accountService = new Mock<IAccountService>();
            _accountService.Setup(s => s.GetUsers(It.IsAny<string>())).ReturnsAsync(users);
            _accountService.Setup(s => s.GetUserByEmail(It.IsAny<string>())).ReturnsAsync((string email) => users.FirstOrDefault(u => u.Email == email));
            _accountService.Setup(s => s.GetUserByUserName(It.IsAny<string>())).ReturnsAsync((string userName) => users.FirstOrDefault(u => u.UserName == userName));
            _accountService.Setup(s => s.RemoveUser(It.IsAny<int>())).Returns(Task.CompletedTask).Callback((int id) =>
            {
                var user = users.FirstOrDefault(u => u.Id == id.ToString());
                if (user != null)
                    users.Remove(user);
            });
        }

        [Fact]
        public void Account_Execute_ReturnsEmpty()
        {
            var command = new AccountCommand(_console.Object, LoggerMock.GetLogger<AccountCommand>().Object);
            var resultMessage = command.Execute();

            Assert.Equal("", resultMessage);
        }

        [Fact]
        public void AccountActivate_Execute_ReturnsSuccessMessage()
        {
            var command = new ActivateCommand(_console.Object, LoggerMock.GetLogger<ActivateCommand>().Object, _accountService.Object)
            {
                Email = "user1@opencatapult.net"
            };

            var resultMessage = command.Execute();

            Assert.Equal("User user1@opencatapult.net has been activated", resultMessage);
        }

        [Fact]
        public void AccountActivate_Execute_ReturnsNotFoundMessage()
        {
            var command = new ActivateCommand(_console.Object, LoggerMock.GetLogger<ActivateCommand>().Object, _accountService.Object)
            {
                Email = "user2@opencatapult.net"
            };

            var resultMessage = command.Execute();

            Assert.Equal("User user2@opencatapult.net is not found", resultMessage);
        }

        [Fact]
        public void AccountRegister_Execute_ReturnsSuccessMessage()
        {
            _accountService.Setup(s => s.RegisterUser(It.IsAny<RegisterUserDto>()))
                .ReturnsAsync(new RegisterUserResultDto());

            var command = new RegisterCommand(_console.Object, LoggerMock.GetLogger<RegisterCommand>().Object, _accountService.Object);
            var resultMessage = command.Execute();

            Assert.StartsWith("User registered", resultMessage);
        }

        [Fact]
        public void AccountRemove_Execute_ReturnsSuccessMessage()
        {
            var command = new RemoveCommand(_console.Object, LoggerMock.GetLogger<RemoveCommand>().Object, _accountService.Object)
            {
                Email = "user1@opencatapult.net"
            };

            var resultMessage = command.Execute();

            Assert.Equal("User user1@opencatapult.net has been removed", resultMessage);
        }

        [Fact]
        public void AccountRemove_Execute_ReturnsNotFoundMessage()
        {
            var command = new RemoveCommand(_console.Object, LoggerMock.GetLogger<RemoveCommand>().Object, _accountService.Object)
            {
                Email = "user2@opencatapult.net"
            };

            var resultMessage = command.Execute();

            Assert.Equal("User user2@opencatapult.net is not found", resultMessage);
        }

        [Fact]
        public void AccountSuspend_Execute_ReturnsSuccessMessage()
        {
            var command = new SuspendCommand(_console.Object, LoggerMock.GetLogger<SuspendCommand>().Object, _accountService.Object)
            {
                Email = "user1@opencatapult.net"
            };

            var resultMessage = command.Execute();

            Assert.Equal("User user1@opencatapult.net has been suspended", resultMessage);
        }

        [Fact]
        public void AccountSuspend_Execute_ReturnsNotFoundMessage()
        {
            var command = new SuspendCommand(_console.Object, LoggerMock.GetLogger<SuspendCommand>().Object, _accountService.Object)
            {
                Email = "user2@opencatapult.net"
            };

            var resultMessage = command.Execute();

            Assert.Equal("User user2@opencatapult.net is not found", resultMessage);
        }
    }
}