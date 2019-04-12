// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Moq;
using Polyrific.Catapult.Cli.Commands;
using Polyrific.Catapult.Cli.Commands.Provider;
using Polyrific.Catapult.Cli.UnitTests.Commands.Utilities;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.Provider;
using Polyrific.Catapult.Shared.Service;
using Xunit;
using Xunit.Abstractions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Polyrific.Catapult.Cli.UnitTests.Commands
{
    public class ProviderCommandTests
    {
        private readonly IConsole _console;
        private readonly Mock<IProviderService> _providerService;
        private readonly ITestOutputHelper _output;

        public ProviderCommandTests(ITestOutputHelper output)
        {
            _output = output;
            _console = new TestConsole(output);

            _providerService = new Mock<IProviderService>();
        }

        [Fact]
        public void Provider_Execute_ReturnsEmpty()
        {
            var command = new ProviderCommand(_console, LoggerMock.GetLogger<ProviderCommand>().Object);

            var message = command.Execute();

            Assert.Equal("", message);
        }

        [Fact]
        public void ProviderGet_Execute_ReturnsSuccessMessage()
        {
            _providerService.Setup(s => s.GetProviderByName(It.IsAny<string>()))
                .ReturnsAsync((string providerName) => new TaskProviderDto { Id = 1, Name = providerName});

            var command = new GetCommand(_providerService.Object, _console, LoggerMock.GetLogger<GetCommand>().Object)
            {
                ProviderName = "AProvider01"
            };

            var message = command.Execute();

            Assert.StartsWith("Task provider AProvider01:", message);
        }

        [Fact]
        public void ProviderGet_Execute_ReturnsNotFoundMessage()
        {
            _providerService.Setup(s => s.GetProviderByName(It.IsAny<string>()))
                .ReturnsAsync((TaskProviderDto)null);

            var command = new GetCommand(_providerService.Object, _console, LoggerMock.GetLogger<GetCommand>().Object)
            {
                ProviderName = "AProvider01"
            };

            var message = command.Execute();

            Assert.Equal("Task provider AProvider01 was not found.", message);
        }

        [Fact]
        public void ProviderList_Execute_ReturnsSuccessMessage()
        {
            _providerService.Setup(s => s.GetProviders(It.IsAny<string>())).ReturnsAsync(new List<TaskProviderDto>
            {
                new TaskProviderDto {Id = 1, Name = "AProvider01"}
            });

            var command = new ListCommand(_providerService.Object, _console, LoggerMock.GetLogger<ListCommand>().Object);

            var message = command.Execute();

            Assert.StartsWith("Found 1 task provider(s):", message);
        }

        [Fact]
        public void ProviderList_Execute_ReturnsNoProvidersMessage()
        {
            _providerService.Setup(s => s.GetProviders(It.IsAny<string>())).ReturnsAsync(new List<TaskProviderDto>());

            var command = new ListCommand(_providerService.Object, _console, LoggerMock.GetLogger<ListCommand>().Object);

            var message = command.Execute();

            Assert.Equal("No registered task providers found.", message);
        }

        [Fact]
        public void ProviderRegister_Execute_ReturnsSuccessMessage()
        {
            var testFile = Path.Combine(AppContext.BaseDirectory, "providerTest.yaml");

            if (File.Exists(testFile))
                File.Delete(testFile);

            var testContent = new NewTaskProviderDto
            {
                Name = "AProvider01",
                Type = TaskProviderType.HostingProvider,
                Author = "Frandi",
                Version = "1.0"
            };
            var stringContent = YamlSerialize(testContent);
            File.WriteAllText(testFile, stringContent);

            _providerService.Setup(s => s.AddProvider(It.IsAny<NewTaskProviderDto>())).ReturnsAsync((NewTaskProviderDto dto) => new TaskProviderDto
            {
                Id = 1,
                Name = dto.Name,
                Type = dto.Type,
                Author = dto.Author,
                Version = dto.Version
            });

            var command = new RegisterCommand(_providerService.Object, _console, LoggerMock.GetLogger<RegisterCommand>().Object)
            {
                MetadataFile = testFile
            };

            var message = command.Execute();

            Assert.Equal("Task provider AProvider01 (v1.0) by Frandi has been registered successfully.", message);
        }

        [Fact]
        public void ProviderRegister_Execute_ReturnsNoFileMessage()
        {
            var testFile = Path.Combine(AppContext.BaseDirectory, "providerTest.yaml");

            if (File.Exists(testFile))
                File.Delete(testFile);
            
            var command = new RegisterCommand(_providerService.Object, _console, LoggerMock.GetLogger<RegisterCommand>().Object)
            {
                MetadataFile = testFile
            };

            var message = command.Execute();

            Assert.Equal($"Could not find \"{testFile}\".", message);
        }

        [Fact]
        public void ProviderRegister_Execute_ReturnsNotParsedMessage()
        {
            var testFile = Path.Combine(AppContext.BaseDirectory, "providerTest.yaml");

            if (File.Exists(testFile))
                File.Delete(testFile);

            File.WriteAllText(testFile, "");
            
            var command = new RegisterCommand(_providerService.Object, _console, LoggerMock.GetLogger<RegisterCommand>().Object)
            {
                MetadataFile = testFile
            };

            var message = command.Execute();

            Assert.Equal("Task provider metadata could not be parsed from the file content.", message);
        }

        [Fact]
        public void ProviderRemove_Execute_ReturnsSuccessMessage()
        {
            var console = new TestConsole(_output, "y");
            _providerService.Setup(s => s.GetProviderByName(It.IsAny<string>()))
                .ReturnsAsync((string providerName) => new TaskProviderDto {Id = 1, Name = providerName});
            _providerService.Setup(s => s.DeleteProvider(It.IsAny<int>())).Returns(Task.CompletedTask);

            var command = new RemoveCommand(_providerService.Object, console,
                LoggerMock.GetLogger<RemoveCommand>().Object)
            {
                ProviderName = "AProvider01"
            };

            var message = command.Execute();

            Assert.Equal("Task provider AProvider01 has been removed.", message);
        }

        [Fact]
        public void ProviderRemove_Execute_ReturnsNotFoundMessage()
        {
            var console = new TestConsole(_output, "y");

            _providerService.Setup(s => s.GetProviderByName(It.IsAny<string>()))
                .ReturnsAsync((TaskProviderDto)null);

            var command = new RemoveCommand(_providerService.Object, console,
                LoggerMock.GetLogger<RemoveCommand>().Object)
            {
                ProviderName = "AProvider01"
            };

            var message = command.Execute();

            Assert.Equal("Task provider AProvider01 was not found.", message);
        }

        private string YamlSerialize(NewTaskProviderDto dto)
        {
            var serializer = new SerializerBuilder().WithNamingConvention(new HyphenatedNamingConvention()).Build();
            return serializer.Serialize(dto);
        }
    }
}
