// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Moq;
using Polyrific.Catapult.Cli.Commands;
using Polyrific.Catapult.Cli.Commands.Plugin;
using Polyrific.Catapult.Cli.UnitTests.Commands.Utilities;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.Plugin;
using Polyrific.Catapult.Shared.Service;
using Xunit;
using Xunit.Abstractions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Polyrific.Catapult.Cli.UnitTests.Commands
{
    public class PluginCommandTests
    {
        private readonly Mock<IConsole> _console;
        private readonly Mock<IPluginService> _pluginService;
        private readonly ITestOutputHelper _output;

        public PluginCommandTests(ITestOutputHelper output)
        {
            _output = output;
            _console = new Mock<IConsole>();

            _pluginService = new Mock<IPluginService>();
        }

        [Fact]
        public void Plugin_Execute_ReturnsEmpty()
        {
            var command = new PluginCommand(_console.Object, LoggerMock.GetLogger<PluginCommand>().Object);

            var message = command.Execute();

            Assert.Equal("", message);
        }

        [Fact]
        public void PluginGet_Execute_ReturnsSuccessMessage()
        {
            _pluginService.Setup(s => s.GetPluginByName(It.IsAny<string>()))
                .ReturnsAsync((string pluginName) => new PluginDto {Id = 1, Name = pluginName});

            var command = new GetCommand(_pluginService.Object, _console.Object, LoggerMock.GetLogger<GetCommand>().Object)
            {
                PluginName = "APlugin01"
            };

            var message = command.Execute();

            Assert.StartsWith("Plugin APlugin01:", message);
        }

        [Fact]
        public void PluginGet_Execute_ReturnsNotFoundMessage()
        {
            _pluginService.Setup(s => s.GetPluginByName(It.IsAny<string>()))
                .ReturnsAsync((PluginDto)null);

            var command = new GetCommand(_pluginService.Object, _console.Object, LoggerMock.GetLogger<GetCommand>().Object)
            {
                PluginName = "APlugin01"
            };

            var message = command.Execute();

            Assert.Equal("Plugin APlugin01 was not found.", message);
        }

        [Fact]
        public void PluginList_Execute_ReturnsSuccessMessage()
        {
            _pluginService.Setup(s => s.GetPlugins(It.IsAny<string>())).ReturnsAsync(new List<PluginDto>
            {
                new PluginDto {Id = 1, Name = "APlugin01"}
            });

            var command = new ListCommand(_pluginService.Object, _console.Object, LoggerMock.GetLogger<ListCommand>().Object);

            var message = command.Execute();

            Assert.StartsWith("Registered plugins:", message);
        }

        [Fact]
        public void PluginList_Execute_ReturnsNoPluginsMessage()
        {
            _pluginService.Setup(s => s.GetPlugins(It.IsAny<string>())).ReturnsAsync(new List<PluginDto>());

            var command = new ListCommand(_pluginService.Object, _console.Object, LoggerMock.GetLogger<ListCommand>().Object);

            var message = command.Execute();

            Assert.Equal("No registered plugins found.", message);
        }

        [Fact]
        public void PluginRegister_Execute_ReturnsSuccessMessage()
        {
            var testFile = Path.Combine(AppContext.BaseDirectory, "pluginTest.yaml");

            if (File.Exists(testFile))
                File.Delete(testFile);

            var testContent = new NewPluginDto
            {
                Name = "APlugin01",
                Type = PluginType.HostingProvider,
                Author = "Frandi",
                Version = "1.0"
            };
            var stringContent = YamlSerialize(testContent);
            File.WriteAllText(testFile, stringContent);

            _pluginService.Setup(s => s.AddPlugin(It.IsAny<NewPluginDto>())).ReturnsAsync((NewPluginDto dto) => new PluginDto
            {
                Id = 1,
                Name = dto.Name,
                Type = dto.Type,
                Author = dto.Author,
                Version = dto.Version
            });

            var command = new RegisterCommand(_pluginService.Object, _console.Object, LoggerMock.GetLogger<RegisterCommand>().Object)
            {
                MetadataFile = testFile
            };

            var message = command.Execute();

            Assert.Equal("Plugin APlugin01 (v1.0) by Frandi has been registered successfully.", message);
        }

        [Fact]
        public void PluginRegister_Execute_ReturnsNoFileMessage()
        {
            var testFile = Path.Combine(AppContext.BaseDirectory, "pluginTest.yaml");

            if (File.Exists(testFile))
                File.Delete(testFile);
            
            var command = new RegisterCommand(_pluginService.Object, _console.Object, LoggerMock.GetLogger<RegisterCommand>().Object)
            {
                MetadataFile = testFile
            };

            var message = command.Execute();

            Assert.Equal($"Could not find \"{testFile}\".", message);
        }

        [Fact]
        public void PluginRegister_Execute_ReturnsNotParsedMessage()
        {
            var testFile = Path.Combine(AppContext.BaseDirectory, "pluginTest.yaml");

            if (File.Exists(testFile))
                File.Delete(testFile);

            File.WriteAllText(testFile, "");
            
            var command = new RegisterCommand(_pluginService.Object, _console.Object, LoggerMock.GetLogger<RegisterCommand>().Object)
            {
                MetadataFile = testFile
            };

            var message = command.Execute();

            Assert.Equal("Plugin metadata could not be parsed from the file content.", message);
        }

        [Fact]
        public void PluginRemove_Execute_ReturnsSuccessMessage()
        {
            var console = new TestConsole(_output, "y");
            _pluginService.Setup(s => s.GetPluginByName(It.IsAny<string>()))
                .ReturnsAsync((string pluginName) => new PluginDto {Id = 1, Name = pluginName});
            _pluginService.Setup(s => s.DeletePlugin(It.IsAny<int>())).Returns(Task.CompletedTask);

            var command = new RemoveCommand(_pluginService.Object, console,
                LoggerMock.GetLogger<RemoveCommand>().Object)
            {
                PluginName = "APlugin01"
            };

            var message = command.Execute();

            Assert.Equal("Plugin APlugin01 has been removed.", message);
        }

        [Fact]
        public void PluginRemove_Execute_ReturnsNotFoundMessage()
        {
            var console = new TestConsole(_output, "y");

            _pluginService.Setup(s => s.GetPluginByName(It.IsAny<string>()))
                .ReturnsAsync((PluginDto)null);

            var command = new RemoveCommand(_pluginService.Object, console,
                LoggerMock.GetLogger<RemoveCommand>().Object)
            {
                PluginName = "APlugin01"
            };

            var message = command.Execute();

            Assert.Equal("Plugin APlugin01 was not found.", message);
        }

        private string YamlSerialize(NewPluginDto dto)
        {
            var serializer = new SerializerBuilder().WithNamingConvention(new HyphenatedNamingConvention()).Build();
            return serializer.Serialize(dto);
        }
    }
}
