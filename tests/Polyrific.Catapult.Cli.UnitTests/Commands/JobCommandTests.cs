// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Moq;
using Polyrific.Catapult.Cli.Commands;
using Polyrific.Catapult.Cli.Commands.Job;
using Polyrific.Catapult.Cli.UnitTests.Commands.Utilities;
using Polyrific.Catapult.Shared.Dto.JobDefinition;
using Polyrific.Catapult.Shared.Dto.Project;
using Polyrific.Catapult.Shared.Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Polyrific.Catapult.Cli.UnitTests.Commands
{
    public class JobCommandTests
    {
        private readonly IConsole _console;
        private readonly Mock<IProjectService> _projectService;
        private readonly Mock<IJobDefinitionService> _jobDefinitionService;
        private readonly Mock<IProviderService> _providerService;
        private readonly ITestOutputHelper _output;

        public JobCommandTests(ITestOutputHelper output)
        {
            _output = output;
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
                    Name = "Default",
                    Tasks = new List<JobTaskDefinitionDto>
                    {
                        new JobTaskDefinitionDto
                        {
                            Id = 1,
                            Name = "Generate",
                            Type = "Generate"
                        }
                    }
                }
            };

            _console = new TestConsole(output);

            _jobDefinitionService = new Mock<IJobDefinitionService>();
            _jobDefinitionService.Setup(s => s.CreateJobDefinition(It.IsAny<int>(), It.IsAny<CreateJobDefinitionDto>())).ReturnsAsync((int projectId, CreateJobDefinitionDto dto) => new JobDefinitionDto
            {
                Id = 2,
                Name = dto.Name,
                ProjectId = projectId
            });
            _jobDefinitionService.Setup(s => s.GetJobDefinitionByName(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync((int projectId, string name) => jobs.FirstOrDefault(u => u.ProjectId == projectId && u.Name == name));
            _jobDefinitionService.Setup(s => s.GetJobDefinitions(It.IsAny<int>())).ReturnsAsync((int projectId) => jobs.Where(j => j.ProjectId == projectId).ToList());
            _jobDefinitionService.Setup(s => s.DeleteJobDefinition(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.CompletedTask).Callback((int projectId, int id) =>
            {
                var job = jobs.FirstOrDefault(u => u.Id == id);
                if (job != null)
                    jobs.Remove(job);
            });

            _projectService = new Mock<IProjectService>();
            _projectService.Setup(p => p.GetProjectByName(It.IsAny<string>())).ReturnsAsync((string name) => projects.FirstOrDefault(p => p.Name == name));

            _providerService = new Mock<IProviderService>();
            _providerService.Setup(s => s.GetProviderAdditionalConfigByProviderName(It.IsAny<string>())).ReturnsAsync(new List<Shared.Dto.Provider.ProviderAdditionalConfigDto>());
        }

        [Fact]
        public void Job_Execute_ReturnsEmpty()
        {
            var command = new JobCommand(_console, LoggerMock.GetLogger<JobCommand>().Object);
            var resultMessage = command.Execute();

            Assert.Equal("", resultMessage);
        }

        [Fact]
        public void JobAdd_Execute_ReturnsSuccessMessage()
        {
            var command = new AddCommand(_console, LoggerMock.GetLogger<AddCommand>().Object, _projectService.Object, _jobDefinitionService.Object)
            {
                Project = "Project 1",
                Name = "Default 2"
            };

            var resultMessage = command.Execute();

            Assert.StartsWith("Job definition has been added", resultMessage);
        }

        [Fact]
        public void JobAdd_Execute_ReturnsNotFoundMessage()
        {
            var command = new AddCommand(_console, LoggerMock.GetLogger<AddCommand>().Object, _projectService.Object, _jobDefinitionService.Object)
            {
                Project = "Project 2",
                Name = "Default 2"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Project Project 2 was not found", resultMessage);
        }

        [Fact]
        public void JobList_Execute_ReturnsSuccessMessage()
        {
            var command = new ListCommand(_console, LoggerMock.GetLogger<ListCommand>().Object, _projectService.Object, _jobDefinitionService.Object)
            {
                Project = "Project 1"
            };

            var resultMessage = command.Execute();

            Assert.StartsWith("Found 1 job definition(s):", resultMessage);
        }

        [Fact]
        public void JobList_Execute_ReturnsNotFoundMessage()
        {
            var command = new ListCommand(_console, LoggerMock.GetLogger<ListCommand>().Object, _projectService.Object, _jobDefinitionService.Object)
            {
                Project = "Project 2"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Project Project 2 was not found", resultMessage);
        }

        [Fact]
        public void JobRemove_Execute_ReturnsSuccessMessage()
        {
            var console = new TestConsole(_output, "y");
            var command = new RemoveCommand(console, LoggerMock.GetLogger<RemoveCommand>().Object, _projectService.Object, _jobDefinitionService.Object)
            {
                Project = "Project 1",
                Name = "Default"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Job definition Default has been removed successfully", resultMessage);
        }

        [Fact]
        public void JobRemove_Execute_ReturnsNotFoundMessage()
        {
            var console = new TestConsole(_output, "y");
            var command = new RemoveCommand(console, LoggerMock.GetLogger<RemoveCommand>().Object, _projectService.Object, _jobDefinitionService.Object)
            {
                Project = "Project 2",
                Name = "Default"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Failed to remove job definition Default. Make sure the project and job definition names are correct.", resultMessage);
        }

        [Fact]
        public void JobUpdate_Execute_ReturnsSuccessMessage()
        {
            var command = new UpdateCommand(_console, LoggerMock.GetLogger<UpdateCommand>().Object, _projectService.Object, _jobDefinitionService.Object)
            {
                Project = "Project 1",
                Name = "Default"
            };

            var resultMessage = command.Execute();
          
            Assert.Equal("Job definition Default has been updated successfully", resultMessage);
        }

        [Fact]
        public void JobUpdate_Execute_ReturnsNotFoundMessage()
        {
            var command = new UpdateCommand(_console, LoggerMock.GetLogger<UpdateCommand>().Object, _projectService.Object, _jobDefinitionService.Object)
            {
                Project = "Project 2",
                Name = "Default"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Failed to update job definition Default. Make sure the project and job definition names are correct.", resultMessage);
        }          
  
        [Fact]
        public void JobGet_Execute_ReturnsSuccessMessage()
        {
            var command = new GetCommand(_console, LoggerMock.GetLogger<GetCommand>().Object, _projectService.Object, _jobDefinitionService.Object, _providerService.Object)
            {
                Project = "Project 1",
                Name = "Default"
            };
  
            var resultMessage = command.Execute();

            Assert.StartsWith("Job definition Default", resultMessage);
        }
          
        [Fact]
        public void JobGet_Execute_ReturnsNotFoundMessage()
        {
            var command = new GetCommand(_console, LoggerMock.GetLogger<GetCommand>().Object, _projectService.Object, _jobDefinitionService.Object, _providerService.Object)
            {
                Project = "Project 2",
                Name = "Default"
            };

            var resultMessage = command.Execute();
          
            Assert.Equal("Failed to get job definition Default. Make sure the project and job definition names are correct.", resultMessage);
        }
    }
}
