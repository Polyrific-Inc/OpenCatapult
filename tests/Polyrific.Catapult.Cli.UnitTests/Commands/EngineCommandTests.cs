// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Moq;
using Polyrific.Catapult.Cli.Commands;
using Polyrific.Catapult.Cli.Commands.Engine;
using Polyrific.Catapult.Cli.UnitTests.Commands.Utilities;
using Polyrific.Catapult.Shared.Dto.CatapultEngine;
using Polyrific.Catapult.Shared.Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Polyrific.Catapult.Cli.UnitTests.Commands
{
    public class EngineCommandTests
    {
        private readonly IConsole _console;
        private readonly Mock<ICatapultEngineService> _engineService;
        private readonly Mock<ITokenService> _tokenService;
        private readonly ITestOutputHelper _output;
        private readonly Mock<IHelpContextService> _helpContextService;

        public EngineCommandTests(ITestOutputHelper output)
        {
            _output = output;

            var engines = new List<CatapultEngineDto>
            {
                new CatapultEngineDto
                {
                    Id = 1,
                    Name = "Engine01"
                }
            };

            _console = new TestConsole(output);
            _tokenService = new Mock<ITokenService>();
            _tokenService.Setup(t => t.RequestEngineToken(It.IsAny<int>(), It.IsAny<RequestEngineTokenDto>())).ReturnsAsync("testToken");

            _engineService = new Mock<ICatapultEngineService>();
            _engineService.Setup(s => s.GetCatapultEngines(It.IsAny<string>())).ReturnsAsync(engines);
            _engineService.Setup(s => s.GetCatapultEngineByName(It.IsAny<string>())).ReturnsAsync((string name) => engines.FirstOrDefault(u => u.Name == name));
            _engineService.Setup(s => s.RemoveCatapultEngine(It.IsAny<int>())).Returns(Task.CompletedTask).Callback((int id) =>
            {
                var engine = engines.FirstOrDefault(u => u.Id == id);
                if (engine != null)
                    engines.Remove(engine);
            });

            _helpContextService = new Mock<IHelpContextService>();
        }

        [Fact]
        public void Engine_Execute_ReturnsEmpty()
        {
            var command = new EngineCommand(_helpContextService.Object, _console, LoggerMock.GetLogger<EngineCommand>().Object);
            var resultMessage = command.Execute();

            Assert.Equal("", resultMessage);
        }

        [Fact]
        public void EngineActivate_Execute_ReturnsSuccessMessage()
        {
            var command = new ActivateCommand(_console, LoggerMock.GetLogger<ActivateCommand>().Object, _engineService.Object)
            {
                Name = "Engine01"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Engine Engine01 has been activated successfully", resultMessage);
        }

        [Fact]
        public void EngineActivate_Execute_ReturnsNotFoundMessage()
        {
            var command = new ActivateCommand(_console, LoggerMock.GetLogger<ActivateCommand>().Object, _engineService.Object)
            {
                Name = "Engine02"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Engine Engine02 was not found", resultMessage);
        }

        [Fact]
        public void EngineRegister_Execute_ReturnsSuccessMessage()
        {
            _engineService.Setup(s => s.RegisterEngine(It.IsAny<RegisterCatapultEngineDto>()))
                .ReturnsAsync(new CatapultEngineDto());

            var command = new RegisterCommand(_console, LoggerMock.GetLogger<RegisterCommand>().Object, _engineService.Object)
            {
                Name = "Engine02"
            };

            var resultMessage = command.Execute();

            Assert.StartsWith("Engine has been registered:", resultMessage);
        }

        [Fact]
        public void EngineRemove_Execute_ReturnsSuccessMessage()
        {
            var console = new TestConsole(_output, "y");
            var command = new RemoveCommand(console, LoggerMock.GetLogger<RemoveCommand>().Object, _engineService.Object)
            {
                Name = "Engine01"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Engine Engine01 has been removed successfully", resultMessage);
        }

        [Fact]
        public void EngineRemove_Execute_ReturnsNotFoundMessage()
        {
            var console = new TestConsole(_output, "y");
            var command = new RemoveCommand(console, LoggerMock.GetLogger<RemoveCommand>().Object, _engineService.Object)
            {
                Name = "Engine02"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Engine Engine02 was not found", resultMessage);
        }

        [Fact]
        public void EngineSuspend_Execute_ReturnsSuccessMessage()
        {
            var command = new SuspendCommand(_console, LoggerMock.GetLogger<SuspendCommand>().Object, _engineService.Object)
            {
                Name = "Engine01"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Engine Engine01 has been suspended successfully", resultMessage);
        }

        [Fact]
        public void EngineSuspend_Execute_ReturnsNotFoundMessage()
        {
            var command = new SuspendCommand(_console, LoggerMock.GetLogger<SuspendCommand>().Object, _engineService.Object)
            {
                Name = "Engine02"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Engine Engine02 was not found", resultMessage);
        }

        [Fact]
        public void EngineGet_Execute_ReturnsSuccessMessage()
        {
            var command = new GetCommand(_console, LoggerMock.GetLogger<GetCommand>().Object, _engineService.Object)
            {
                Name = "Engine01"
            };

            var resultMessage = command.Execute();

            Assert.StartsWith("Engine Engine01", resultMessage);
        }

        [Fact]
        public void EngineGet_Execute_ReturnsNotFoundMessage()
        {
            var command = new GetCommand(_console, LoggerMock.GetLogger<GetCommand>().Object, _engineService.Object)
            {
                Name = "Engine02"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Engine Engine02 was not found", resultMessage);
        }

        [Fact]
        public void EngineList_Execute_ReturnsSuccessMessage()
        {
            var command = new ListCommand(_console, LoggerMock.GetLogger<ListCommand>().Object, _engineService.Object);

            var resultMessage = command.Execute();

            Assert.StartsWith("Found 1 engine(s):", resultMessage);
        }

        [Fact]
        public void EngineToken_Execute_ReturnsSuccessMessage()
        {
            var command = new TokenCommand(_console, LoggerMock.GetLogger<TokenCommand>().Object, _tokenService.Object, _engineService.Object)
            {
                Name = "Engine01"
            };

            var resultMessage = command.Execute();

            Assert.StartsWith("Token: testToken", resultMessage);
        }

        [Fact]
        public void EngineToken_Execute_ReturnsNotFoundMessage()
        {
            var command = new TokenCommand(_console, LoggerMock.GetLogger<TokenCommand>().Object, _tokenService.Object, _engineService.Object)
            {
                Name = "Engine02"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Engine Engine02 was not found", resultMessage);
        }
    }
}
