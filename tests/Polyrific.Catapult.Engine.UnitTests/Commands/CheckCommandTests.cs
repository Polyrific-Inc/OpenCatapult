// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Engine.Commands;
using Polyrific.Catapult.Engine.Core;
using Polyrific.Catapult.Engine.UnitTests.Commands.Utilities;
using Polyrific.Catapult.Shared.Dto.JobQueue;
using Xunit;
using Xunit.Abstractions;

namespace Polyrific.Catapult.Engine.UnitTests.Commands
{
    public class CheckCommandTests
    {
        private readonly Mock<ICatapultEngine> _catapultEngine;
        private readonly ITestOutputHelper _output;

        public CheckCommandTests(ITestOutputHelper output)
        {
            _catapultEngine = new Mock<ICatapultEngine>();

            _output = output;
        }

        [Fact]
        public void Check_Execute_ReturnsFoundAndAutoExecute()
        {
            _catapultEngine.Setup(e => e.GetJobInQueue()).ReturnsAsync(() => new JobDto{ProjectId = 1, Code = "20180817.1"});

            var console = new TestConsole(_output);
            var command = new CheckCommand(_catapultEngine.Object, console, (new Mock<ILogger<CheckCommand>>()).Object)
            {
                AutoExecute = true
            };

            var message = command.Execute();

            Assert.Equal("Job queue \"20180817.1\" execution has completed.", message);
        }

        [Fact]
        public void Check_Execute_ReturnsFoundAndExecute()
        {
            _catapultEngine.Setup(e => e.GetJobInQueue()).ReturnsAsync(() => new JobDto{ProjectId = 1, Code = "20180817.1"});

            var console = new TestConsole(_output, "y");
            var command = new CheckCommand(_catapultEngine.Object, console, (new Mock<ILogger<CheckCommand>>()).Object);
            var message = command.Execute();

            Assert.Equal("Job queue \"20180817.1\" execution has completed.", message);
        }

        [Fact]
        public void Check_Execute_ReturnsFoundButNotExecuted()
        {
            _catapultEngine.Setup(e => e.GetJobInQueue()).ReturnsAsync(() => new JobDto{ProjectId = 1, Code = "20180817.1"});

            var console = new TestConsole(_output, "n");
            var command = new CheckCommand(_catapultEngine.Object, console, (new Mock<ILogger<CheckCommand>>()).Object);
            var message = command.Execute();

            Assert.Equal("Job queue \"20180817.1\" has not been executed.", message);
        }

        [Fact]
        public void Check_Execute_ReturnsNotFound()
        {
            _catapultEngine.Setup(e => e.GetJobInQueue()).ReturnsAsync(() => null);

            var console = new TestConsole(_output);
            var command = new CheckCommand(_catapultEngine.Object, console, (new Mock<ILogger<CheckCommand>>()).Object);
            var message = command.Execute();

            Assert.Equal("No job was found in queue.", message);
        }
    }
}