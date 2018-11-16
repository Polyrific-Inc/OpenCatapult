﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Moq;
using Polyrific.Catapult.Engine.Commands;
using Polyrific.Catapult.Engine.Commands.Task;
using Polyrific.Catapult.Engine.Core;
using Polyrific.Catapult.Engine.UnitTests.Commands.Utilities;
using Polyrific.Catapult.Engine.Utility;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.Project;
using Xunit;
using Xunit.Abstractions;

namespace Polyrific.Catapult.Engine.UnitTests.Commands
{
    public class TaskCommandTests
    {
        private readonly IConsole _console;
        private readonly Mock<ICatapultEngine> _engine;
        private readonly Mock<IProjectTemplateReader> _templateReader;

        public TaskCommandTests(ITestOutputHelper output)
        {
            _console = new TestConsole(output);
            _engine = new Mock<ICatapultEngine>();
            _templateReader = new Mock<IProjectTemplateReader>();
        }

        [Fact]
        public void Task_Execute_ReturnsEmpty()
        {
            var command = new TaskCommand(_console, LoggerMock.GetLogger<TaskCommand>().Object);
            var message = command.Execute();

            Assert.Equal("", message);
        }

        [Fact]
        public void TaskRun_Execute_ReturnsCompleted()
        {
            _engine.Setup(e =>
                    e.ExecuteTask(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<NewProjectDto>()))
                .Returns(Task.CompletedTask);

            var command = new RunCommand(_engine.Object, _templateReader.Object, _console, LoggerMock.GetLogger<RunCommand>().Object)
            {
                Type = JobTaskDefinitionType.Build,
                Provider = "DotNetCore"
            };

            var message = command.Execute();

            Assert.Equal("Completed", message);
        }
    }
}
