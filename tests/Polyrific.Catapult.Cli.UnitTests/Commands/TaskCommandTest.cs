// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Moq;
using Polyrific.Catapult.Cli.Commands;
using Polyrific.Catapult.Cli.Commands.Task;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.JobDefinition;
using Polyrific.Catapult.Shared.Dto.Project;
using Polyrific.Catapult.Shared.Service;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Polyrific.Catapult.Cli.UnitTests.Commands
{
    public class TaskCommandTests
    {
        private readonly Mock<IConsole> _console;
        private readonly Mock<IProjectService> _projectService;
        private readonly Mock<IJobDefinitionService> _jobDefinitionService;

        public TaskCommandTests()
        {
            var projects = new List<ProjectDto>
            {
                new ProjectDto
                {
                    Id = 1,
                    Name = "Project 1"
                }
            };

            var jobs = new List<JobDefinitionDto>
            {
                new JobDefinitionDto
                {
                    Id = 1,
                    ProjectId = 1,
                    Name = "Default"
                }
            };

            var tasks = new List<JobTaskDefinitionDto>
            {
                new JobTaskDefinitionDto
                {
                    Id = 1,
                    JobDefinitionId = 1,
                    Name = "Generate",
                    Type = JobTaskDefinitionType.Generate
                }
            };

            _console = new Mock<IConsole>();

            _jobDefinitionService = new Mock<IJobDefinitionService>();
            _jobDefinitionService.Setup(s => s.GetJobDefinitionByName(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync((int projectId, string name) => jobs.FirstOrDefault(u => u.ProjectId == projectId && u.Name == name));
            _jobDefinitionService.Setup(s => s.CreateJobTaskDefinition(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CreateJobTaskDefinitionDto>())).ReturnsAsync((int projectId, int jobDefinitionId, CreateJobTaskDefinitionDto dto) => new JobTaskDefinitionDto
            {
                Id = 2,
                Name = dto.Name,
                JobDefinitionId = jobDefinitionId
            });
            _jobDefinitionService.Setup(s => s.GetJobTaskDefinitions(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((int projectId, int jobDefinitionId) => tasks);
            _jobDefinitionService.Setup(s => s.GetJobTaskDefinitionByName(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync((int projectId, int jobDefinitionId, string name) => tasks.FirstOrDefault(t => t.JobDefinitionId == jobDefinitionId && t.Name == name));

            _projectService = new Mock<IProjectService>();
            _projectService.Setup(p => p.GetProjectByName(It.IsAny<string>())).ReturnsAsync((string name) => projects.FirstOrDefault(p => p.Name == name));
        }

        [Fact]
        public void Task_Execute_ReturnsEmpty()
        {
            var command = new TaskCommand(_console.Object, LoggerMock.GetLogger<TaskCommand>().Object);
            var resultMessage = command.Execute();

            Assert.Equal("", resultMessage);
        }

        [Fact]
        public void TaskAdd_Execute_ReturnsSuccessMessage()
        {
            var command = new AddCommand(_console.Object, LoggerMock.GetLogger<AddCommand>().Object, _projectService.Object, _jobDefinitionService.Object)
            {
                Project = "Project 1",
                Job = "Default",
                Name = "Push",
                Type = JobTaskDefinitionType.Push
            };

            var resultMessage = command.Execute();

            Assert.StartsWith("Task Push added to job Default:", resultMessage);
        }

        [Fact]
        public void TaskAdd_Execute_ReturnsNotFoundMessage()
        {
            var command = new AddCommand(_console.Object, LoggerMock.GetLogger<AddCommand>().Object, _projectService.Object, _jobDefinitionService.Object)
            {
                Project = "Project 1",
                Job = "Default 2",
                Name = "Push",
                Type = JobTaskDefinitionType.Push
            };

            var resultMessage = command.Execute();

            Assert.Equal("Failed adding task Push. Make sure the project and job definition names are correct.", resultMessage);
        }

        [Fact]
        public void TaskGet_Execute_ReturnsSuccessMessage()
        {
            var command = new GetCommand(_console.Object, LoggerMock.GetLogger<GetCommand>().Object, _projectService.Object, _jobDefinitionService.Object)
            {
                Project = "Project 1",
                Job = "Default",
                Name = "Generate"
            };

            var resultMessage = command.Execute();

            Assert.StartsWith("Task Generate in job Default:", resultMessage);
        }

        [Fact]
        public void TaskGet_Execute_ReturnsNotFoundMessage()
        {
            var command = new GetCommand(_console.Object, LoggerMock.GetLogger<GetCommand>().Object, _projectService.Object, _jobDefinitionService.Object)
            {
                Project = "Project 1",
                Job = "Default 2",
                Name = "Push"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Failed fetching task Push. Make sure the project, job definition, and task names are correct.", resultMessage);
        }

        [Fact]
        public void TaskList_Execute_ReturnsSuccessMessage()
        {
            var command = new ListCommand(_console.Object, LoggerMock.GetLogger<ListCommand>().Object, _projectService.Object, _jobDefinitionService.Object)
            {
                Project = "Project 1",
                Job = "Default"
            };

            var resultMessage = command.Execute();

            Assert.StartsWith("Job task definitions in job Default:", resultMessage);
        }

        [Fact]
        public void TaskList_Execute_ReturnsNotFoundMessage()
        {
            var command = new ListCommand(_console.Object, LoggerMock.GetLogger<ListCommand>().Object, _projectService.Object, _jobDefinitionService.Object)
            {
                Project = "Project 1",
                Job = "Default 2"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Failed fetching tasks. Make sure the project and job names are correct.", resultMessage);
        }

        [Fact]
        public void TaskUpdate_Execute_ReturnsSuccessMessage()
        {
            var command = new UpdateCommand(_console.Object, LoggerMock.GetLogger<UpdateCommand>().Object, _projectService.Object, _jobDefinitionService.Object)
            {
                Project = "Project 1",
                Job = "Default",
                Name = "Generate"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Task Generate was updated", resultMessage);
        }

        [Fact]
        public void TaskUpdate_Execute_ReturnsNotFoundMessage()
        {
            var command = new UpdateCommand(_console.Object, LoggerMock.GetLogger<UpdateCommand>().Object, _projectService.Object, _jobDefinitionService.Object)
            {
                Project = "Project 1",
                Job = "Default",
                Name = "Push"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Failed updating task Push. Make sure the project and job definition names are correct.", resultMessage);
        }
    }
}
