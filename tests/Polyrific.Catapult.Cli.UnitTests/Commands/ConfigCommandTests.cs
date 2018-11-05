// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Polyrific.Catapult.Cli.Commands;
using Polyrific.Catapult.Cli.Commands.Config;
using Polyrific.Catapult.Cli.UnitTests.Commands.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Polyrific.Catapult.Cli.UnitTests.Commands
{
    public class ConfigCommandTests
    {
        private readonly ITestOutputHelper _output;
        private readonly Mock<ICliConfig> _cliConfig;
        private readonly Dictionary<string, string> _configs;

        public ConfigCommandTests(ITestOutputHelper output)
        {
            _output = output;

            _configs = new Dictionary<string, string>
            {
                {"config1", "config 1 value"}
            };

            _cliConfig = new Mock<ICliConfig>();
            _cliConfig.Setup(c => c.Load()).Returns(Task.CompletedTask);
            _cliConfig.Setup(c => c.Configs).Returns(_configs);
            _cliConfig.Setup(c => c.GetValue(It.IsAny<string>())).Returns((string configName) =>
                _configs.ContainsKey(configName) ? _configs[configName] : "");
            _cliConfig.Setup(c => c.SetValue(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string configName, string configValue) =>
                {
                    if (_configs.ContainsKey(configName))
                        _configs[configName] = configValue;
                    else
                        _configs.Add(configName, configValue);
                });
            _cliConfig.Setup(c => c.RemoveValue(It.IsAny<string>()))
                .Callback((string configName) => _configs.Remove(configName));
            _cliConfig.Setup(c => c.Save()).Returns(Task.CompletedTask);
        }

        [Fact]
        public void Config_Execute_ReturnsEmpty()
        {
            var console = new TestConsole(_output);
            var command = new ConfigCommand(console, (new Mock<ILogger<ConfigCommand>>()).Object);

            var message = command.Execute();

            Assert.Equal("", message);
        }

        [Fact]
        public void ConfigGet_GetAll_Execute_ReturnsItems()
        {
            var console = new TestConsole(_output);
            var command = new GetCommand(_cliConfig.Object, console, (new Mock<ILogger<GetCommand>>()).Object);

            var message = command.Execute();

            Assert.StartsWith("Available CLI configs:", message);
        }

        [Fact]
        public void ConfigGet_Execute_ReturnsItem()
        {
            var console = new TestConsole(_output);
            var command = new GetCommand(_cliConfig.Object, console, (new Mock<ILogger<GetCommand>>()).Object)
            {
                ConfigName = "config1"
            };

            var message = command.Execute();

            Assert.StartsWith("config1: config 1 value", message);
        }

        [Fact]
        public void ConfigGet_Execute_ReturnsEmpty()
        {
            var console = new TestConsole(_output);
            var command = new GetCommand(_cliConfig.Object, console, (new Mock<ILogger<GetCommand>>()).Object)
            {
                ConfigName = "config2"
            };

            var message = command.Execute();

            Assert.StartsWith("", message);
        }

        [Fact]
        public void ConfigImport_Execute_ReturnsSuccess()
        {
            var content = new Dictionary<string, string>
            {
                {"config1", "config 1 value edited"},
                {"config2", "config 2 value"}
            };

            var filePath = Path.Combine(AppContext.BaseDirectory, "exists.json");
            if (!File.Exists(filePath))
                File.WriteAllText(filePath, JsonConvert.SerializeObject(content));

            var console = new TestConsole(_output);
            var command = new ImportCommand(_cliConfig.Object, console, (new Mock<ILogger<ImportCommand>>()).Object)
            {
                InputFile = filePath
            };

            var message = command.Execute();

            var sb = new StringBuilder("Configs have been imported.");
            sb.AppendLine("- Modified:");
            sb.AppendLine("  - config1: config 1 value edited");
            sb.AppendLine("- Added:");
            sb.AppendLine("  - config2: config 2 value");

            Assert.Equal(sb.ToString(), message);
        }

        [Fact]
        public void ConfigImport_Execute_ReturnsNotFound()
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "notexist.json");
            if (File.Exists(filePath))
                File.Delete(filePath);

            var console = new TestConsole(_output);
            var command = new ImportCommand(_cliConfig.Object, console, (new Mock<ILogger<ImportCommand>>()).Object)
            {
                InputFile = filePath
            };

            var message = command.Execute();

            Assert.Equal($"File \"{filePath}\" could not be found.", message);
        }

        [Fact]
        public void ConfigRemove_RemoveAll_Execute_ReturnsSuccess()
        {
            var console = new TestConsole(_output, "y");
            var command = new RemoveCommand(_cliConfig.Object, console, (new Mock<ILogger<RemoveCommand>>()).Object)
            {
                RemoveAll = true
            };

            var message = command.Execute();

            Assert.Empty(_configs);
            Assert.Equal("Config values have been removed successfully.", message);
        }

        [Fact]
        public void ConfigRemove_Execute_ReturnsSuccess()
        {
            var console = new TestConsole(_output, "y");
            var command = new RemoveCommand(_cliConfig.Object, console, (new Mock<ILogger<RemoveCommand>>()).Object)
            {
                ConfigName = "config1"
            };

            var message = command.Execute();

            Assert.True(!_configs.ContainsKey("config1"));
            Assert.Equal("Config \"config1\" has been removed.", message);
        }

        [Fact]
        public void ConfigSet_SetAll_Execute_ReturnsSuccess()
        {
            var console = new TestConsole(_output, "config value edited");
            var command = new SetCommand(_cliConfig.Object, console, (new Mock<ILogger<SetCommand>>()).Object);

            var message = command.Execute();

            Assert.Equal("config value edited", _configs["config1"]);
            Assert.Equal("Config values have been saved successfully.", message);
        }

        [Theory]
        [InlineData("config1", "config 1 value edited")]
        [InlineData("config2", "config 2 value")]
        [InlineData("config3", "")]
        [InlineData("", "config 3 value")]
        public void ConfigSet_Execute_ReturnsSuccess(string configName, string configValue)
        {
            var console = new TestConsole(_output);
            var command = new SetCommand(_cliConfig.Object, console, (new Mock<ILogger<SetCommand>>()).Object)
            {
                ConfigName = configName,
                ConfigValue = configValue
            };

            var message = command.Execute();

            if (!string.IsNullOrEmpty(configName) && !string.IsNullOrEmpty(configValue))
            {
                Assert.Equal(configValue, _configs[configName]);
                Assert.Equal($"The value of \"{configName}\" has been set to \"{configValue}\".", message);
            }
            else
            {
                Assert.Equal("", message);
            }
        }
    }
}
