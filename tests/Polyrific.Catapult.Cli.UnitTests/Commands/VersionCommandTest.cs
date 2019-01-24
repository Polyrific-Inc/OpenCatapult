// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using McMaster.Extensions.CommandLineUtils;
using Moq;
using Polyrific.Catapult.Cli.Commands;
using Polyrific.Catapult.Cli.UnitTests.Commands.Utilities;
using Polyrific.Catapult.Shared.Dto.CatapultEngine;
using Polyrific.Catapult.Shared.Dto.Provider;
using Polyrific.Catapult.Shared.Dto.Version;
using Polyrific.Catapult.Shared.Service;
using Xunit;
using Xunit.Abstractions;

namespace Polyrific.Catapult.Cli.UnitTests.Commands
{
    public class VersionCommandTest
    {
        private readonly IConsole _console;
        private readonly Mock<IVersionService> _versionService;

        public VersionCommandTest(ITestOutputHelper output)
        {
            _console = new TestConsole(output);

            _versionService = new Mock<IVersionService>();
            _versionService.Setup(s => s.GetApiVersion()).ReturnsAsync(new VersionDto
            {
                ApiVersion = "1.0",
                Engines = new List<CatapultEngineDto>
                {
                    new CatapultEngineDto
                    {
                        Name = "Engine01",
                        Version = "1.0"
                    }
                },
                Providers = new List<ProviderDto>
                {
                    new ProviderDto
                    {
                        Name = "AspNetMvc",
                        Version = "1.0"
                    }
                }
            });
        }

        [Fact]
        public void Version_Execute_ReturnString()
        {
            var command = new VersionCommand(_console, LoggerMock.GetLogger<VersionCommand>().Object, _versionService.Object);

            var resultMessage = command.Execute();

            Assert.Contains("API Version: 1.0", resultMessage);
            Assert.Contains("CLI Version:", resultMessage);
            Assert.Contains("- Engine01: 1.0", resultMessage);
            Assert.Contains("- AspNetMvc: 1.0", resultMessage);
        }
    }
}
