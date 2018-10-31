// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Polyrific.Catapult.Cli.Commands;
using Polyrific.Catapult.Cli.Commands.Service;
using Polyrific.Catapult.Cli.UnitTests.Commands.Utilities;
using Polyrific.Catapult.Shared.Dto.ExternalService;
using Polyrific.Catapult.Shared.Dto.ExternalServiceType;
using Polyrific.Catapult.Shared.Service;
using Xunit;
using Xunit.Abstractions;

namespace Polyrific.Catapult.Cli.UnitTests.Commands
{
    public class ServiceCommandTests
    {
        private readonly ITestOutputHelper _output;
        private readonly Mock<IConsoleReader> _consoleReader;
        private readonly Mock<IExternalServiceService> _externalServiceService;
        private readonly Mock<IExternalServiceTypeService> _externalServiceTypeService;

        public ServiceCommandTests(ITestOutputHelper output)
        {
            _output = output;

            var services = new List<ExternalServiceDto>
            {
                new ExternalServiceDto
                {
                    Id = 1,
                    ExternalServiceTypeId = 1,
                    Name = "Default-Github",
                    Description = "Default github service",
                    Config = new Dictionary<string, string> { { "user", "test" } }
                }
            };

            var serviceTypes = new List<ExternalServiceTypeDto>
            {
                new ExternalServiceTypeDto
                {
                    Id = 1,
                    Name = "GitHub",
                    ExternalServiceProperties = new List<ExternalServicePropertyDto>
                    {
                        new ExternalServicePropertyDto
                        {
                            Name = "RemoteUrl",
                            Description = "Remote Url",
                            IsRequired = true
                        },
                        new ExternalServicePropertyDto
                        {
                            Name = "RemoteAuthType",
                            Description = "Remote auth type",
                            IsRequired = true,
                            AllowedValues = new string[] { "userPassword", "authToken" }
                        },
                        new ExternalServicePropertyDto
                        {
                            Name = "AuthToken",
                            Description = "Auth token",
                            IsSecret = true,
                            AdditionalLogic = new AdditionalLogicDto
                            {
                                RequiredCondition = new PropertyConditionDto
                                {
                                    PropertyName = "RemoteAuthType",
                                    PropertyValue = "authToken"
                                },
                                HideCondition = new PropertyConditionDto
                                {
                                    PropertyName = "RemoteAuthType",
                                    PropertyValue = "userPassword"
                                }
                            }
                        },
                        new ExternalServicePropertyDto
                        {
                            Name = "Username",
                            Description = "Username",
                            AdditionalLogic = new AdditionalLogicDto
                            {
                                RequiredCondition = new PropertyConditionDto
                                {
                                    PropertyName = "RemoteAuthType",
                                    PropertyValue = "userPassword"
                                },
                                HideCondition = new PropertyConditionDto
                                {
                                    PropertyName = "RemoteAuthType",
                                    PropertyValue = "authToken"
                                }
                            }
                        }
                    }
                }
            };
                       
            _consoleReader = new Mock<IConsoleReader>();
            _consoleReader.Setup(x => x.GetPassword(It.IsAny<string>(), It.IsAny<ConsoleColor>(), It.IsAny<ConsoleColor>())).Returns("testPassword");

            _externalServiceService = new Mock<IExternalServiceService>();
            _externalServiceService.Setup(s => s.GetExternalServiceByName(It.IsAny<string>())).ReturnsAsync((string name) => services.FirstOrDefault(u => u.Name == name));
            _externalServiceService.Setup(s => s.CreateExternalService(It.IsAny<CreateExternalServiceDto>())).ReturnsAsync((CreateExternalServiceDto dto) => new ExternalServiceDto
            {
                Id = 2,
                Name = dto.Name,
                ExternalServiceTypeId = dto.ExternalServiceTypeId,
                Description = dto.Description,
                Config = dto.Config
            });
            _externalServiceService.Setup(s => s.GetExternalServices()).ReturnsAsync(services);

            _externalServiceTypeService = new Mock<IExternalServiceTypeService>();
            _externalServiceTypeService.Setup(s => s.GetExternalServiceTypeByName(It.IsAny<string>())).ReturnsAsync((string name) => serviceTypes.FirstOrDefault(u => u.Name == name));
            _externalServiceTypeService.Setup(s => s.GetExternalServiceType(It.IsAny<int>())).ReturnsAsync((int id) => serviceTypes.FirstOrDefault(u => u.Id == id));
        }

        [Fact]
        public void Service_Execute_ReturnsEmpty()
        {
            var console = new TestConsole(_output, "userPassword");
            var command = new ServiceCommand(console, LoggerMock.GetLogger<ServiceCommand>().Object);
            var resultMessage = command.Execute();

            Assert.Equal("", resultMessage);
        }

        [Fact]
        public void ServiceAdd_Execute_ReturnsSuccessMessage()
        {
            var console = new TestConsole(_output, "userPassword");
            var command = new AddCommand(console, LoggerMock.GetLogger<AddCommand>().Object, _consoleReader.Object, _externalServiceService.Object, _externalServiceTypeService.Object)
            {
                Type = "GitHub",
                Name = "GitHub-account",
                Description = "GitHub service"
            };

            var resultMessage = command.Execute();

            Assert.StartsWith("External service has been added:", resultMessage);
        }

        [Fact]
        public void ServiceAdd_Execute_ReturnsNotFoundMessage()
        {
            var console = new TestConsole(_output, "userPassword");
            var command = new AddCommand(console, LoggerMock.GetLogger<AddCommand>().Object, _consoleReader.Object, _externalServiceService.Object, _externalServiceTypeService.Object)
            {
                Type = "VSTS",
                Name = "vsts-account",
                Description = "vsts service"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Service type VSTS was not found", resultMessage);
        }

        [Fact]
        public void ServiceGet_Execute_ReturnsSuccessMessage()
        {
            var console = new TestConsole(_output, "userPassword");
            var command = new GetCommand(console, LoggerMock.GetLogger<GetCommand>().Object, _externalServiceService.Object, _externalServiceTypeService.Object)
            {
                Name = "Default-Github"
            };

            var resultMessage = command.Execute();

            Assert.StartsWith("External Service Default-Github:", resultMessage);
        }

        [Fact]
        public void ServiceGet_Execute_ReturnsNotFoundMessage()
        {
            var console = new TestConsole(_output, "userPassword");
            var command = new GetCommand(console, LoggerMock.GetLogger<GetCommand>().Object, _externalServiceService.Object, _externalServiceTypeService.Object)
            {
                Name = "Default-VSTS"
            };

            var resultMessage = command.Execute();

            Assert.Equal("External Service Default-VSTS was not found", resultMessage);
        }

        [Fact]
        public void ServiceList_Execute_ReturnsSuccessMessage()
        {
            var console = new TestConsole(_output, "userPassword");
            var command = new ListCommand(console, LoggerMock.GetLogger<ListCommand>().Object, _externalServiceService.Object);

            var resultMessage = command.Execute();

            Assert.StartsWith("Found 1 external service(s):", resultMessage);
        }

        [Fact]
        public void ServiceUpdate_Execute_ReturnsSuccessMessage()
        {
            var console = new TestConsole(_output, "userPassword");
            var command = new UpdateCommand(console, LoggerMock.GetLogger<UpdateCommand>().Object, _consoleReader.Object, _externalServiceService.Object, _externalServiceTypeService.Object)
            {
                Name = "Default-Github",
            };

            var resultMessage = command.Execute();

            Assert.Equal("External Service Default-Github has been updated successfully", resultMessage);
        }

        [Fact]
        public void ServiceUpdate_Execute_ReturnsNotFoundMessage()
        {
            var console = new TestConsole(_output, "userPassword");
            var command = new UpdateCommand(console, LoggerMock.GetLogger<UpdateCommand>().Object, _consoleReader.Object, _externalServiceService.Object, _externalServiceTypeService.Object)
            {
                Name = "Default-VSTS"
            };

            var resultMessage = command.Execute();

            Assert.Equal("External Service Default-VSTS was not found", resultMessage);
        }

        [Fact]
        public void ServiceRemove_Execute_ReturnsSuccessMessage()
        {
            var console = new TestConsole(_output, "y", "userPassword");
            var command = new RemoveCommand(console, LoggerMock.GetLogger<RemoveCommand>().Object, _externalServiceService.Object)
            {
                Name = "Default-Github"
            };

            var resultMessage = command.Execute();

            Assert.Equal("External Service Default-Github has been removed successfully", resultMessage);
        }

        [Fact]
        public void ServiceRemove_Execute_ReturnsNotFoundMessage()
        {
            var console = new TestConsole(_output, "y", "userPassword");
            var command = new RemoveCommand(console, LoggerMock.GetLogger<RemoveCommand>().Object, _externalServiceService.Object)
            {
                Name = "Default-VSTS"
            };

            var resultMessage = command.Execute();

            Assert.Equal("External Service Default-VSTS was not found", resultMessage);
        }
    }
}
