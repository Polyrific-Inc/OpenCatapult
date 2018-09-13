// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Moq;
using Polyrific.Catapult.Cli.Commands;
using Polyrific.Catapult.Cli.Commands.Project;
using Polyrific.Catapult.Shared.Dto.Project;
using Polyrific.Catapult.Shared.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Polyrific.Catapult.Cli.UnitTests.Commands
{
    public class ProjectCommandTests
    {
        private readonly Mock<IConsole> _console;
        private readonly Mock<IProjectService> _projectService;
        private readonly Mock<ITemplateWriter> _templateWriter;

        public ProjectCommandTests()
        {
            var projects = new List<ProjectDto>
            {
                new ProjectDto
                {
                    Id = 1,
                    Name = "Project 1"
                }
            };

            _console = new Mock<IConsole>();

            _projectService = new Mock<IProjectService>();
            _projectService.Setup(p => p.GetProjects(It.IsAny<string>())).ReturnsAsync(projects);
            _projectService.Setup(p => p.GetProjectByName(It.IsAny<string>())).ReturnsAsync((string name) => projects.FirstOrDefault(p => p.Name == name));
            _projectService.Setup(p => p.CreateProject(It.IsAny<NewProjectDto>())).ReturnsAsync((NewProjectDto dto) => new ProjectDto
            {
                Id = 2,
                Name = dto.Name,
                Client = dto.Client
            });
            _projectService.Setup(p => p.CloneProject(It.IsAny<int>(), It.IsAny<CloneProjectOptionDto>())).ReturnsAsync((int projectId, CloneProjectOptionDto dto) => new ProjectDto
            {
                Id = 2,
                Name = dto.NewProjectName
            });

            _templateWriter = new Mock<ITemplateWriter>();
            _templateWriter.Setup(t => t.Read(It.IsAny<string>())).Returns(
@"name: Project 2
models:
- name: Product"
            );
            _templateWriter.Setup(t => t.Write(It.IsAny<string>(), It.IsAny<string>())).Returns((string filePath, string content) => filePath);
        }

        [Fact]
        public void Project_Execute_ReturnsEmpty()
        {
            var command = new ProjectCommand(_console.Object, LoggerMock.GetLogger<ProjectCommand>().Object);
            var resultMessage = command.Execute();

            Assert.Equal("", resultMessage);
        }

        [Fact]
        public void ProjectArchive_Execute_ReturnsSuccessMessage()
        {
            var command = new ArchiveCommand(_console.Object, LoggerMock.GetLogger<ArchiveCommand>().Object, _projectService.Object)
            {
                Name = "Project 1"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Project Project 1 archived", resultMessage);
        }

        [Fact]
        public void ProjectArchive_Execute_ReturnsNotFoundMessage()
        {
            var command = new ArchiveCommand(_console.Object, LoggerMock.GetLogger<ArchiveCommand>().Object, _projectService.Object)
            {
                Name = "Project 2"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Project Project 2 is not found", resultMessage);
        }

        [Fact]
        public void ProjectClone_Execute_ReturnsSuccessMessage()
        {
            var command = new CloneCommand(_console.Object, LoggerMock.GetLogger<CloneCommand>().Object, _projectService.Object)
            {
                Project = "Project 1",
                Name = "Project 2"
            };

            var resultMessage = command.Execute();

            Assert.StartsWith("Project cloned:", resultMessage);
        }

        [Fact]
        public void ProjectClone_Execute_ReturnsNotFoundMessage()
        {
            var command = new CloneCommand(_console.Object, LoggerMock.GetLogger<CloneCommand>().Object, _projectService.Object)
            {
                Project = "Project 2",
                Name = "Project 3"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Project Project 2 is not found", resultMessage);
        }

        [Fact]
        public void ProjectCreate_Execute_ReturnsSuccessMessage()
        {
            var command = new CreateCommand(_console.Object, LoggerMock.GetLogger<CreateCommand>().Object, _projectService.Object, _templateWriter.Object)
            {
                Name = "Project 2",
                Client = "Company",
            };

            var resultMessage = command.Execute();

            Assert.StartsWith("Project created:", resultMessage);
        }

        [Fact]
        public void ProjectCreate_Execute_WithTemplateReturnsSuccessMessage()
        {
            var command = new CreateCommand(_console.Object, LoggerMock.GetLogger<CreateCommand>().Object, _projectService.Object, _templateWriter.Object)
            {
                Name = "Project 2",
                Client = "Company",
                Template = "Test"
            };

            var resultMessage = command.Execute();

            Assert.StartsWith("Project created:", resultMessage);
            _projectService.Verify(s => s.CreateProject(It.Is<NewProjectDto>(p => p.Models.Count > 0)), Times.Once);
        }

        [Fact]
        public void ProjectExport_Execute_ReturnsSuccessMessage()
        {
            var command = new ExportCommand(_console.Object, LoggerMock.GetLogger<ExportCommand>().Object, _projectService.Object, _templateWriter.Object)
            {
                Name = "Project 1"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Project exported to Project 1", resultMessage);
        }

        [Fact]
        public void ProjectExport_Execute_SpecifyOutputReturnsSuccessMessage()
        {
            var command = new ExportCommand(_console.Object, LoggerMock.GetLogger<ExportCommand>().Object, _projectService.Object, _templateWriter.Object)
            {
                Name = "Project 1",
                Output = "C:\\Document\\project.yaml"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Project exported to C:\\Document\\project.yaml", resultMessage);
        }

        [Fact]
        public void ProjectExport_Execute_ReturnsNotFoundMessage()
        {
            var command = new ExportCommand(_console.Object, LoggerMock.GetLogger<ExportCommand>().Object, _projectService.Object, _templateWriter.Object)
            {
                Name = "Project 2",
            };

            var resultMessage = command.Execute();

            Assert.Equal("Project Project 2 is not found", resultMessage);
        }

        [Fact]
        public void ProjectList_Execute_ReturnsSuccessMessage()
        {
            var command = new ListCommand(_console.Object, LoggerMock.GetLogger<ListCommand>().Object, _projectService.Object);

            var resultMessage = command.Execute();

            Assert.StartsWith("Projects:", resultMessage);
        }

        [Fact]
        public void ProjectRemove_Execute_ReturnsSuccessMessage()
        {
            var command = new RemoveCommand(_console.Object, LoggerMock.GetLogger<RemoveCommand>().Object, _projectService.Object)
            {
                Name = "Project 1"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Project Project 1 was removed", resultMessage);
        }

        [Fact]
        public void ProjectRemove_Execute_ReturnsNotFoundMessage()
        {
            var command = new RemoveCommand(_console.Object, LoggerMock.GetLogger<RemoveCommand>().Object, _projectService.Object)
            {
                Name = "Project 2"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Project Project 2 is not found", resultMessage);
        }

        [Fact]
        public void ProjectUpdate_Execute_ReturnsSuccessMessage()
        {
            var command = new UpdateCommand(_console.Object, LoggerMock.GetLogger<UpdateCommand>().Object, _projectService.Object)
            {
                Name = "Project 1",
                Rename = "Project 2"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Project Project 1 was updated", resultMessage);
        }

        [Fact]
        public void ProjectUpdate_Execute_ReturnsNotFoundMessage()
        {
            var command = new UpdateCommand(_console.Object, LoggerMock.GetLogger<UpdateCommand>().Object, _projectService.Object)
            {
                Name = "Project 2",
                Rename = "Project 1"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Project Project 2 is not found", resultMessage);
        }
    }
}
