// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Moq;
using Polyrific.Catapult.Cli.Commands;
using Polyrific.Catapult.Cli.Commands.Engine;
using Polyrific.Catapult.Shared.Dto.CatapultEngine;
using Polyrific.Catapult.Shared.Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Polyrific.Catapult.Cli.UnitTests.Commands
{
    public class EngineCommandTests
    {
        private readonly Mock<IConsole> _console;
        private readonly Mock<ICatapultEngineService> _engineService;
        private readonly Mock<ITokenService> _tokenService;

        public EngineCommandTests()
        {
            var engines = new List<CatapultEngineDto>
            {
                new CatapultEngineDto
                {
                    Id = 1,
                    Name = "Engine01"
                }
            };

            _console = new Mock<IConsole>();
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
        }

        [Fact]
        public void Engine_Execute_ReturnsEmpty()
        {
            var command = new EngineCommand(_console.Object, LoggerMock.GetLogger<EngineCommand>().Object);
            var resultMessage = command.Execute();

            Assert.Equal("", resultMessage);
        }

        [Fact]
        public void EngineActivate_Execute_ReturnsSuccessMessage()
        {
            var command = new ActivateCommand(_console.Object, LoggerMock.GetLogger<ActivateCommand>().Object, _engineService.Object)
            {
                Name = "Engine01"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Engine Engine01 has been activated", resultMessage);
        }

        [Fact]
        public void EngineActivate_Execute_ReturnsNotFoundMessage()
        {
            var command = new ActivateCommand(_console.Object, LoggerMock.GetLogger<ActivateCommand>().Object, _engineService.Object)
            {
                Name = "Engine02"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Engine Engine02 is not found", resultMessage);
        }

        [Fact]
        public void EngineRegister_Execute_ReturnsSuccessMessage()
        {
            _engineService.Setup(s => s.RegisterEngine(It.IsAny<RegisterCatapultEngineDto>()))
                .ReturnsAsync(new RegisterCatapultEngineResponseDto());

            var command = new RegisterCommand(_console.Object, LoggerMock.GetLogger<RegisterCommand>().Object, _engineService.Object)
            {
                Name = "Engine02"
            };

            var resultMessage = command.Execute();

            Assert.StartsWith("Engine registered:", resultMessage);
        }

        [Fact]
        public void EngineRemove_Execute_ReturnsSuccessMessage()
        {
            var command = new RemoveCommand(_console.Object, LoggerMock.GetLogger<RemoveCommand>().Object, _engineService.Object)
            {
                Name = "Engine01"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Engine Engine01 has been removed", resultMessage);
        }

        [Fact]
        public void EngineRemove_Execute_ReturnsNotFoundMessage()
        {
            var command = new RemoveCommand(_console.Object, LoggerMock.GetLogger<RemoveCommand>().Object, _engineService.Object)
            {
                Name = "Engine02"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Engine Engine02 is not found", resultMessage);
        }

        [Fact]
        public void EngineSuspend_Execute_ReturnsSuccessMessage()
        {
            var command = new SuspendCommand(_console.Object, LoggerMock.GetLogger<SuspendCommand>().Object, _engineService.Object)
            {
                Name = "Engine01"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Engine Engine01 has been suspended", resultMessage);
        }

        [Fact]
        public void EngineSuspend_Execute_ReturnsNotFoundMessage()
        {
            var command = new SuspendCommand(_console.Object, LoggerMock.GetLogger<SuspendCommand>().Object, _engineService.Object)
            {
                Name = "Engine02"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Engine Engine02 is not found", resultMessage);
        }

        [Fact]
        public void EngineGet_Execute_ReturnsSuccessMessage()
        {
            var command = new GetCommand(_console.Object, LoggerMock.GetLogger<GetCommand>().Object, _engineService.Object)
            {
                Name = "Engine01"
            };

            var resultMessage = command.Execute();

            Assert.StartsWith("Engine Engine01", resultMessage);
        }

        [Fact]
        public void EngineGet_Execute_ReturnsNotFoundMessage()
        {
            var command = new GetCommand(_console.Object, LoggerMock.GetLogger<GetCommand>().Object, _engineService.Object)
            {
                Name = "Engine02"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Engine Engine02 is not found", resultMessage);
        }

        [Fact]
        public void EngineList_Execute_ReturnsSuccessMessage()
        {
            var command = new ListCommand(_console.Object, LoggerMock.GetLogger<ListCommand>().Object, _engineService.Object);

            var resultMessage = command.Execute();

            Assert.StartsWith("Registered engines (all):", resultMessage);
        }

        [Fact]
        public void EngineToken_Execute_ReturnsSuccessMessage()
        {
            var command = new TokenCommand(_console.Object, LoggerMock.GetLogger<TokenCommand>().Object, _tokenService.Object, _engineService.Object)
            {
                Name = "Engine01"
            };

            var resultMessage = command.Execute();

            Assert.StartsWith("Token: testToken", resultMessage);
        }

        [Fact]
        public void EngineToken_Execute_ReturnsNotFoundMessage()
        {
            var command = new TokenCommand(_console.Object, LoggerMock.GetLogger<TokenCommand>().Object, _tokenService.Object, _engineService.Object)
            {
                Name = "Engine02"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Engine Engine02 is not found", resultMessage);
        }
    }
}
