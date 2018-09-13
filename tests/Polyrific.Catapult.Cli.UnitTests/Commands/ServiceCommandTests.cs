// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Moq;
using Polyrific.Catapult.Cli.Commands;
using Polyrific.Catapult.Cli.Commands.Service;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.ExternalService;
using Polyrific.Catapult.Shared.Dto.Project;
using Polyrific.Catapult.Shared.Service;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Polyrific.Catapult.Cli.UnitTests.Commands
{
    public class ServiceCommandTests
    {
        private readonly Mock<IConsole> _console;
        private readonly Mock<IExternalServiceService> _externalServiceService;

        public ServiceCommandTests()
        {

            var services = new List<ExternalServiceDto>
            {
                new ExternalServiceDto
                {
                    Id = 1,
                    Type = "github",
                    Name = "Default-Github",
                    Description = "Default github service",
                    Config = new Dictionary<string, string> { { "user", "test" } }
                }
            };

            _console = new Mock<IConsole>();

            _externalServiceService = new Mock<IExternalServiceService>();
            _externalServiceService.Setup(s => s.GetExternalServiceByName(It.IsAny<string>())).ReturnsAsync((string name) => services.FirstOrDefault(u => u.Name == name));
            _externalServiceService.Setup(s => s.CreateExternalService(It.IsAny<CreateExternalServiceDto>())).ReturnsAsync((CreateExternalServiceDto dto) => new ExternalServiceDto
            {
                Id = 2,
                Name = dto.Name,
                Type = dto.Type,
                Description = dto.Description,
                Config = dto.Config
            });
            _externalServiceService.Setup(s => s.GetExternalServices()).ReturnsAsync(services);
        }

        [Fact]
        public void Service_Execute_ReturnsEmpty()
        {
            var command = new ServiceCommand(_console.Object, LoggerMock.GetLogger<ServiceCommand>().Object);
            var resultMessage = command.Execute();

            Assert.Equal("", resultMessage);
        }

        [Fact]
        public void ServiceAdd_Execute_ReturnsSuccessMessage()
        {
            var command = new AddCommand(_console.Object, LoggerMock.GetLogger<AddCommand>().Object, _externalServiceService.Object)
            {
                Type = "vsts",
                Name = "vsts-account",
                Description = "vsts service",
                Property = new (string,string)[] { ("user", "test")}
            };

            var resultMessage = command.Execute();

            Assert.StartsWith("External service vsts-account created:", resultMessage);
        }

        [Fact]
        public void ServiceGet_Execute_ReturnsSuccessMessage()
        {
            var command = new GetCommand(_console.Object, LoggerMock.GetLogger<GetCommand>().Object, _externalServiceService.Object)
            {
                Name = "Default-Github"
            };

            var resultMessage = command.Execute();

            Assert.StartsWith("External Service Default-Github:", resultMessage);
        }

        [Fact]
        public void ServiceGet_Execute_ReturnsNotFoundMessage()
        {
            var command = new GetCommand(_console.Object, LoggerMock.GetLogger<GetCommand>().Object, _externalServiceService.Object)
            {
                Name = "Default-VSTS"
            };

            var resultMessage = command.Execute();

            Assert.Equal("External Service Default-VSTS is not found", resultMessage);
        }

        [Fact]
        public void ServiceList_Execute_ReturnsSuccessMessage()
        {
            var command = new ListCommand(_console.Object, LoggerMock.GetLogger<ListCommand>().Object, _externalServiceService.Object);

            var resultMessage = command.Execute();

            Assert.StartsWith("Your external services:", resultMessage);
        }

        [Fact]
        public void ServiceUpdate_Execute_ReturnsSuccessMessage()
        {
            var command = new UpdateCommand(_console.Object, LoggerMock.GetLogger<UpdateCommand>().Object, _externalServiceService.Object)
            {
                Name = "Default-Github",
                Property = new (string, string)[] { ("user", "test"), ("password", "test") }
            };

            var resultMessage = command.Execute();

            Assert.Equal("External Service Default-Github was updated", resultMessage);
        }

        [Fact]
        public void ServiceUpdate_Execute_ReturnsNotFoundMessage()
        {
            var command = new UpdateCommand(_console.Object, LoggerMock.GetLogger<UpdateCommand>().Object, _externalServiceService.Object)
            {
                Name = "Default-VSTS",
                Property = new (string, string)[] { ("user", "test"), ("password", "test") }
            };

            var resultMessage = command.Execute();

            Assert.Equal("External Service Default-VSTS is not found", resultMessage);
        }

        [Fact]
        public void ServiceRemove_Execute_ReturnsSuccessMessage()
        {
            var command = new RemoveCommand(_console.Object, LoggerMock.GetLogger<RemoveCommand>().Object, _externalServiceService.Object)
            {
                Name = "Default-Github"
            };

            var resultMessage = command.Execute();

            Assert.Equal("External Service Default-Github was removed", resultMessage);
        }

        [Fact]
        public void ServiceRemove_Execute_ReturnsNotFoundMessage()
        {
            var command = new RemoveCommand(_console.Object, LoggerMock.GetLogger<RemoveCommand>().Object, _externalServiceService.Object)
            {
                Name = "Default-VSTS"
            };

            var resultMessage = command.Execute();

            Assert.Equal("External Service Default-VSTS is not found", resultMessage);
        }
    }
}
