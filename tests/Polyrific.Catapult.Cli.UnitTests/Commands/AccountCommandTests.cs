// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Moq;
using Polyrific.Catapult.Cli.Commands;
using Polyrific.Catapult.Cli.Commands.Account;
using Polyrific.Catapult.Cli.Commands.Account.Password;
using Polyrific.Catapult.Cli.UnitTests.Commands.Utilities;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.User;
using Polyrific.Catapult.Shared.Service;
using Xunit;
using Xunit.Abstractions;

namespace Polyrific.Catapult.Cli.UnitTests.Commands
{
    public class AccountCommandTests
    {
        private readonly IConsole _console;
        private readonly Mock<IAccountService> _accountService;
        private readonly Mock<IConsoleReader> _consoleReader;

        public AccountCommandTests(ITestOutputHelper output)
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

            _accountService = new Mock<IAccountService>();
            _accountService.Setup(s => s.GetUsers(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(users);
            _accountService.Setup(s => s.GetUserByEmail(It.IsAny<string>())).ReturnsAsync((string email) => users.FirstOrDefault(u => u.Email == email));
            _accountService.Setup(s => s.GetUserByUserName(It.IsAny<string>())).ReturnsAsync((string userName) => users.FirstOrDefault(u => u.UserName == userName));
            _accountService.Setup(s => s.RemoveUser(It.IsAny<int>())).Returns(Task.CompletedTask).Callback((int id) =>
            {
                var user = users.FirstOrDefault(u => u.Id == id.ToString());
                if (user != null)
                    users.Remove(user);
            });

            _consoleReader = new Mock<IConsoleReader>();
        }

        [Fact]
        public void Account_Execute_ReturnsEmpty()
        {
            var command = new AccountCommand(_console, LoggerMock.GetLogger<AccountCommand>().Object);
            var resultMessage = command.Execute();

            Assert.Equal("", resultMessage);
        }

        [Fact]
        public void AccountActivate_Execute_ReturnsSuccessMessage()
        {
            var command = new ActivateCommand(_console, LoggerMock.GetLogger<ActivateCommand>().Object, _accountService.Object)
            {
                User = "user1@opencatapult.net"
            };

            var resultMessage = command.Execute();

            Assert.Equal("User user1@opencatapult.net has been activated", resultMessage);
        }

        [Fact]
        public void AccountActivate_Execute_ReturnsNotFoundMessage()
        {
            var command = new ActivateCommand(_console, LoggerMock.GetLogger<ActivateCommand>().Object, _accountService.Object)
            {
                User = "user2@opencatapult.net"
            };

            var resultMessage = command.Execute();

            Assert.Equal("User user2@opencatapult.net was not found", resultMessage);
        }

        [Fact]
        public void AccountRegister_Execute_ReturnsSuccessMessage()
        {
            _accountService.Setup(s => s.RegisterUser(It.IsAny<RegisterUserDto>()))
                .ReturnsAsync(new RegisterUserResultDto());

            var command = new RegisterCommand(_console, LoggerMock.GetLogger<RegisterCommand>().Object, _accountService.Object)
            {
                Email = "user2@opencatapult.net"
            };
            var resultMessage = command.Execute();

            Assert.StartsWith("User user2@opencatapult.net has been registered", resultMessage);
        }

        [Fact]
        public void AccountRemove_Execute_ReturnsSuccessMessage()
        {
            var command = new RemoveCommand(_console, LoggerMock.GetLogger<RemoveCommand>().Object, _accountService.Object)
            {
                User = "user1@opencatapult.net"
            };

            var resultMessage = command.Execute();

            Assert.Equal("User user1@opencatapult.net has been removed", resultMessage);
        }

        [Fact]
        public void AccountRemove_Execute_ReturnsNotFoundMessage()
        {
            var command = new RemoveCommand(_console, LoggerMock.GetLogger<RemoveCommand>().Object, _accountService.Object)
            {
                User = "user2@opencatapult.net"
            };

            var resultMessage = command.Execute();

            Assert.Equal("User user2@opencatapult.net was not found", resultMessage);
        }

        [Fact]
        public void AccountSuspend_Execute_ReturnsSuccessMessage()
        {
            var command = new SuspendCommand(_console, LoggerMock.GetLogger<SuspendCommand>().Object, _accountService.Object)
            {
                User = "user1@opencatapult.net"
            };

            var resultMessage = command.Execute();

            Assert.Equal("User user1@opencatapult.net has been suspended", resultMessage);
        }

        [Fact]
        public void AccountSuspend_Execute_ReturnsNotFoundMessage()
        {
            var command = new SuspendCommand(_console, LoggerMock.GetLogger<SuspendCommand>().Object, _accountService.Object)
            {
                User = "user2@opencatapult.net"
            };

            var resultMessage = command.Execute();

            Assert.Equal("User user2@opencatapult.net was not found", resultMessage);
        }

        [Fact]
        public void AccountUpdate_Execute_ReturnsSuccessMessage()
        {
            var command = new Cli.Commands.Account.UpdateCommand(_console, LoggerMock.GetLogger<Cli.Commands.Account.UpdateCommand>().Object, _accountService.Object)
            {
                User = "user1@opencatapult.net"
            };

            var resultMessage = command.Execute();

            Assert.Equal("User user1@opencatapult.net has been updated", resultMessage);
        }

        [Fact]
        public void AccountUpdate_Execute_ReturnsNotFoundMessage()
        {
            var command = new Cli.Commands.Account.UpdateCommand(_console, LoggerMock.GetLogger<Cli.Commands.Account.UpdateCommand>().Object, _accountService.Object)
            {
                User = "user2@opencatapult.net"
            };

            var resultMessage = command.Execute();

            Assert.Equal("User user2@opencatapult.net was not found", resultMessage);
        }

        [Fact]
        public void AccountUpdatePassword_Execute_ReturnsSuccessMessage()
        {
            var command = new Cli.Commands.Account.Password.UpdateCommand(_console, LoggerMock.GetLogger<Cli.Commands.Account.Password.UpdateCommand>().Object, _accountService.Object, _consoleReader.Object)
            {
                User = "user1@opencatapult.net"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Password for user user1@opencatapult.net has been updated", resultMessage);
        }

        [Fact]
        public void AccountUpdatePassword_Execute_ReturnsNotFoundMessage()
        {
            var command = new Cli.Commands.Account.Password.UpdateCommand(_console, LoggerMock.GetLogger<Cli.Commands.Account.Password.UpdateCommand>().Object, _accountService.Object, _consoleReader.Object)
            {
                User = "user2@opencatapult.net"
            };

            var resultMessage = command.Execute();

            Assert.Equal("User user2@opencatapult.net is not found", resultMessage);
        }

        [Fact]
        public void AccountPasswordResetToken_Execute_ReturnsSuccessMessage()
        {
            var command = new ResetTokenCommand(_console, LoggerMock.GetLogger<ResetTokenCommand>().Object, _accountService.Object)
            {
                User = "user1@opencatapult.net"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Reset password token has been sent to user1@opencatapult.net", resultMessage);
        }

        [Fact]
        public void AccountPasswordResetToken_Execute_ReturnsNotFoundMessage()
        {
            var command = new ResetTokenCommand(_console, LoggerMock.GetLogger<ResetTokenCommand>().Object, _accountService.Object)
            {
                User = "user2@opencatapult.net"
            };

            var resultMessage = command.Execute();

            Assert.Equal("User user2@opencatapult.net is not found", resultMessage);
        }

        [Fact]
        public void AccountPasswordReset_Execute_ReturnsSuccessMessage()
        {
            var command = new ResetCommand(_console, LoggerMock.GetLogger<ResetCommand>().Object, _accountService.Object, _consoleReader.Object)
            {
                User = "user1@opencatapult.net"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Password for user user1@opencatapult.net has been reset", resultMessage);
        }

        [Fact]
        public void AccountPasswordReset_Execute_ReturnsNotFoundMessage()
        {
            var command = new ResetCommand(_console, LoggerMock.GetLogger<ResetCommand>().Object, _accountService.Object, _consoleReader.Object)
            {
                User = "user2@opencatapult.net"
            };

            var resultMessage = command.Execute();

            Assert.Equal("User user2@opencatapult.net is not found", resultMessage);
        }

        [Fact]
        public void AccountSetRole_Execute_ReturnsSuccessMessage()
        {
            var command = new SetRoleCommand(_console, LoggerMock.GetLogger<SetRoleCommand>().Object, _accountService.Object)
            {
                User = "user1@opencatapult.net",
                Role = UserRole.Basic
            };

            var resultMessage = command.Execute();

            Assert.Equal("User user1@opencatapult.net has been assigned to role \"Basic\"", resultMessage);
        }

        [Fact]
        public void AccountSetRole_Execute_ReturnsNotFoundMessage()
        {
            var command = new SetRoleCommand(_console, LoggerMock.GetLogger<SetRoleCommand>().Object, _accountService.Object)
            {
                User = "user2@opencatapult.net"
            };

            var resultMessage = command.Execute();

            Assert.Equal("User user2@opencatapult.net was not found", resultMessage);
        }

        [Fact]
        public void AccountList_Execute_ReturnsSuccessMessage()
        {
            var command = new ListCommand(_console, LoggerMock.GetLogger<ListCommand>().Object, _accountService.Object);

            var resultMessage = command.Execute();

            Assert.StartsWith("Found 1 user(s):", resultMessage);
        }
    }
}
