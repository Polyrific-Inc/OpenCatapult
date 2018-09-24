// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Moq;
using Polyrific.Catapult.Cli.Commands;
using Polyrific.Catapult.Cli.Commands.Project;
using Polyrific.Catapult.Cli.UnitTests.Commands.Utilities;
using Polyrific.Catapult.Shared.Dto.ExternalService;
using Polyrific.Catapult.Shared.Dto.Plugin;
using Polyrific.Catapult.Shared.Dto.Project;
using Polyrific.Catapult.Shared.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Polyrific.Catapult.Cli.UnitTests.Commands
{
    public class ProjectCommandTests
    {
        private readonly ITestOutputHelper _output;
        private readonly Mock<IConsole> _console;
        private readonly Mock<IConsoleReader> _consoleReader;
        private readonly Mock<IProjectService> _projectService;
        private readonly Mock<IPluginService> _pluginService;
        private readonly Mock<IExternalServiceService> _externalServiceService;
        private readonly Mock<ITemplateWriter> _templateWriter;

        public ProjectCommandTests(ITestOutputHelper output)
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

            var plugins = new List<PluginDto>
            {
                new PluginDto
                {
                    Id = 1,
                    Name = "AspNetCoreMvc"
                },
                new PluginDto
                {
                    Id = 2,
                    Name = "GitHubRepositoryProvider",
                    RequiredServices = new string[] { "GitHub" }
                },
                new PluginDto
                {
                    Id = 3,
                    Name = "AzureAppService",
                    AdditionalConfigs = new PluginAdditionalConfigDto[]
                    {
                        new PluginAdditionalConfigDto
                        {
                            Name = "SubscriptionId",
                            Label = "Subscription Id",
                            Type = "string",
                            IsRequired = true,
                            IsSecret = false
                        },
                        new PluginAdditionalConfigDto
                        {
                            Name = "AppKey",
                            Label = "AppKey Id",
                            Type = "string",
                            IsRequired = true,
                            IsSecret = true
                        }
                    }
                }
            };

            var services = new List<ExternalServiceDto>
            {
                new ExternalServiceDto
                {
                    Id = 1,
                    ExternalServiceTypeId = 1,
                    ExternalServiceTypeName = "GitHub",
                    Name = "github-default",
                    Description = "Default github service",
                    Config = new Dictionary<string, string> { { "user", "test" } }
                },
                new ExternalServiceDto
                {
                    Id = 2,
                    ExternalServiceTypeId = 2,
                    ExternalServiceTypeName = "AzureAppService",
                    Name = "azure-default",
                    Description = "Default azure service",
                    Config = new Dictionary<string, string> { { "user", "test" } }
                }
            };

            _console = new Mock<IConsole>();
            _consoleReader = new Mock<IConsoleReader>();

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

            _pluginService = new Mock<IPluginService>();
            _pluginService.Setup(s => s.GetPluginByName(It.IsAny<string>()))
                .ReturnsAsync((string pluginName) => plugins.FirstOrDefault(x => x.Name == pluginName));

            _externalServiceService = new Mock<IExternalServiceService>();
            _externalServiceService.Setup(s => s.GetExternalServiceByName(It.IsAny<string>())).ReturnsAsync((string name) => services.FirstOrDefault(u => u.Name == name));

            _templateWriter = new Mock<ITemplateWriter>();
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
            var command = new CreateCommand(_console.Object, LoggerMock.GetLogger<CreateCommand>().Object, _consoleReader.Object, _projectService.Object, _pluginService.Object, _externalServiceService.Object, _templateWriter.Object)
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
            _templateWriter.Setup(t => t.Read(It.IsAny<string>())).Returns(
@"name: Project 2
models:
    - name: Product
jobs:
- name: Default
  tasks:
  - name: Generate
    type: Generate
    provider: AspNetCoreMvc
  - name: Push
    type: Generate
    provider: GitHubRepositoryProvider
    configs:
      Branch: master
      GitHubExternalService: github-default
  - name: Deploy
    type: Deploy
    provider: AzureAppService
    configs:
      AzureAppServiceExternalService: azure-default"
            );

            _consoleReader.Setup(x => x.GetPassword(It.IsAny<string>(), null, null)).Returns("testPassword");

            var console = new TestConsole(_output, "test");
            var command = new CreateCommand(console, LoggerMock.GetLogger<CreateCommand>().Object, _consoleReader.Object, _projectService.Object, _pluginService.Object, _externalServiceService.Object, _templateWriter.Object)
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
        public void ProjectCreate_Execute_WithTemplateReturnsProviderNotInstalled()
        {
            _templateWriter.Setup(t => t.Read(It.IsAny<string>())).Returns(
@"name: Project 2
models:
    - name: Product
jobs:
- name: Default
  tasks:
  - name: Generate
    type: Generate
    provider: AspNetCoreMvc2"
            );

            var command = new CreateCommand(_console.Object, LoggerMock.GetLogger<CreateCommand>().Object, _consoleReader.Object, _projectService.Object, _pluginService.Object, _externalServiceService.Object, _templateWriter.Object)
            {
                Name = "Project 2",
                Client = "Company",
                Template = "Test"
            };

            var resultMessage = command.Execute();

            Assert.Equal("The provider \"AspNetCoreMvc2\" is not installed", resultMessage);
        }

        [Fact]
        public void ProjectCreate_Execute_WithTemplateReturnsExternalServiceRequired()
        {
            _templateWriter.Setup(t => t.Read(It.IsAny<string>())).Returns(
@"name: Project 2
models:
    - name: Product
jobs:
- name: Default
  tasks:
  - name: Generate
    type: Generate
    provider: AspNetCoreMvc
  - name: Push
    type: Generate
    provider: GitHubRepositoryProvider
    configs:
      Branch: master"
            );

            var command = new CreateCommand(_console.Object, LoggerMock.GetLogger<CreateCommand>().Object, _consoleReader.Object, _projectService.Object, _pluginService.Object, _externalServiceService.Object, _templateWriter.Object)
            {
                Name = "Project 2",
                Client = "Company",
                Template = "Test"
            };

            var resultMessage = command.Execute();

            Assert.Equal("The GitHub external service is required for the provider GitHubRepositoryProvider. Please check the template file", resultMessage);
        }

        [Fact]
        public void ProjectCreate_Execute_WithTemplateReturnsExternalServiceNotFound()
        {
            _templateWriter.Setup(t => t.Read(It.IsAny<string>())).Returns(
@"name: Project 2
models:
    - name: Product
jobs:
- name: Default
  tasks:
  - name: Generate
    type: Generate
    provider: AspNetCoreMvc
  - name: Push
    type: Generate
    provider: GitHubRepositoryProvider
    configs:
      Branch: master
      GitHubExternalService: github-default2"
            );

            var command = new CreateCommand(_console.Object, LoggerMock.GetLogger<CreateCommand>().Object, _consoleReader.Object, _projectService.Object, _pluginService.Object, _externalServiceService.Object, _templateWriter.Object)
            {
                Name = "Project 2",
                Client = "Company",
                Template = "Test"
            };

            var resultMessage = command.Execute();

            Assert.Equal("The external service github-default2 is not found. Please add them using \"service add\" command", resultMessage);
        }

        [Fact]
        public void ProjectCreate_Execute_WithTemplateReturnsIncorrectType()
        {
            _templateWriter.Setup(t => t.Read(It.IsAny<string>())).Returns(
@"name: Project 2
models:
    - name: Product
jobs:
- name: Default
  tasks:
  - name: Generate
    type: Generate
    provider: AspNetCoreMvc
  - name: Push
    type: Generate
    provider: GitHubRepositoryProvider
    configs:
      Branch: master
      GitHubExternalService: azure-default"
            );

            var command = new CreateCommand(_console.Object, LoggerMock.GetLogger<CreateCommand>().Object, _consoleReader.Object, _projectService.Object, _pluginService.Object, _externalServiceService.Object, _templateWriter.Object)
            {
                Name = "Project 2",
                Client = "Company",
                Template = "Test"
            };

            var resultMessage = command.Execute();

            Assert.Equal("The external service azure-default is not a GitHub service", resultMessage);
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
