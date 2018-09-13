// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Engine.Commands;
using Polyrific.Catapult.Engine.Core;
using Polyrific.Catapult.Engine.UnitTests.Commands.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Polyrific.Catapult.Engine.UnitTests.Commands
{
    public class StartCommandTests
    {
        private readonly Mock<ICatapultEngine> _engine;
        private readonly Mock<ICatapultEngineConfig> _engineConfig;
        private readonly ITestOutputHelper _output;

        private const string ApiUrl = "http://localhost";

        public StartCommandTests(ITestOutputHelper output)
        {
            _engine = new Mock<ICatapultEngine>();
            
            _engineConfig = new Mock<ICatapultEngineConfig>();
            _engineConfig.Setup(c => c.ApiUrl).Returns(ApiUrl);
            _engineConfig.Setup(c => c.JobCheckingInterval).Returns(30);

            _output = output;
        }

        [Fact]
        public void Start_Execute_ReturnsConnectionFailed()
        {
            _engine.Setup(e => e.CheckApiConnection()).ReturnsAsync(false);

            var console = new TestConsole(_output);
            var command = new StartCommand(_engine.Object, _engineConfig.Object, console,
                (new Mock<ILogger<StartCommand>>()).Object);

            var exception = Record.Exception(() => command.Execute());

            Assert.Equal($"Connection to {ApiUrl} was failed.", exception?.Message);
        }
    }
}