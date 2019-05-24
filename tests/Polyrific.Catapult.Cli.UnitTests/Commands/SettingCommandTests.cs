// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Polyrific.Catapult.Cli.Commands.Setting;
using Polyrific.Catapult.Cli.UnitTests.Commands.Utilities;
using Polyrific.Catapult.Shared.Dto.ApplicationSetting;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Service;
using Xunit;
using Xunit.Abstractions;

namespace Polyrific.Catapult.Cli.UnitTests.Commands
{
    public class SettingCommandTests
    {
        private readonly ITestOutputHelper _output;
        private readonly Mock<IConsoleReader> _consoleReader;
        private readonly Mock<IApplicationSettingService> _applicationSettingService;

        public SettingCommandTests(ITestOutputHelper output)
        {
            _output = output;

            var settings = new List<ApplicationSettingDto>
            {
                new ApplicationSettingDto
                {
                    Key = "test",
                    DataType = ApplicationSettingDataTypes.Bool,
                    Label = "Test"
                }
            };
                       
            _consoleReader = new Mock<IConsoleReader>();
            _consoleReader.Setup(x => x.GetPassword(It.IsAny<string>(), It.IsAny<ConsoleColor>(), It.IsAny<ConsoleColor>())).Returns("testPassword");

            _applicationSettingService = new Mock<IApplicationSettingService>();
            _applicationSettingService.Setup(s => s.GetApplicationSettings()).ReturnsAsync(settings);
            _applicationSettingService.Setup(s => s.GetApplicationSettingValue()).ReturnsAsync(new ApplicationSettingValueDto
            {
                EnableTwoFactorAuth = true
            });
            _applicationSettingService.Setup(s => s.UpdateApplicationSetting(It.IsAny<UpdateApplicationSettingDto>())).Returns(Task.CompletedTask);
        }

        [Fact]
        public void SettingList_Execute_ReturnsSuccessMessage()
        {
            var console = new TestConsole(_output);
            var command = new ListCommand(console, LoggerMock.GetLogger<ListCommand>().Object, _applicationSettingService.Object);

            var resultMessage = command.Execute();

            Assert.StartsWith("Application setting(s):", resultMessage);
        }

        [Fact]
        public void SettingUpdate_Execute_ReturnsSuccessMessage()
        {
            var console = new TestConsole(_output, "y");
            var command = new UpdateCommand(console, LoggerMock.GetLogger<UpdateCommand>().Object, _consoleReader.Object, _applicationSettingService.Object);

            var resultMessage = command.Execute();

            Assert.Equal("Application settings has been updated successfully", resultMessage);
        }
    }
}
