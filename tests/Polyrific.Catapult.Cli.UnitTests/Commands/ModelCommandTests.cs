// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Moq;
using Polyrific.Catapult.Cli.Commands;
using Polyrific.Catapult.Cli.Commands.Model;
using Polyrific.Catapult.Shared.Dto.Project;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;
using Polyrific.Catapult.Shared.Service;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Polyrific.Catapult.Cli.UnitTests.Commands
{
    public class ModelCommandTests
    {
        private readonly Mock<IConsole> _console;
        private readonly Mock<IProjectService> _projectService;
        private readonly Mock<IProjectDataModelService> _projectModelService;

        public ModelCommandTests()
        {
            var projects = new List<ProjectDto>
            {
                new ProjectDto
                {
                    Id = 1,
                    Name = "Project 1"
                }
            };

            var projectModels = new List<ProjectDataModelDto>
            {
                new ProjectDataModelDto
                {
                    Id = 1,
                    ProjectId = 1,
                    Name = "Product"
                }
            };

            _console = new Mock<IConsole>();

            _projectService = new Mock<IProjectService>();
            _projectService.Setup(p => p.GetProjectByName(It.IsAny<string>())).ReturnsAsync((string name) => projects.FirstOrDefault(p => p.Name == name));

            _projectModelService = new Mock<IProjectDataModelService>();
            _projectModelService.Setup(p => p.CreateProjectDataModel(It.IsAny<int>(), It.IsAny<CreateProjectDataModelDto>())).ReturnsAsync((int projectId, CreateProjectDataModelDto dto) =>
            {
                var newProjectDataModel = new ProjectDataModelDto
                {
                    Id = 2,
                    ProjectId = projectId,
                    Name = dto.Name,
                    Description = dto.Description,
                    Label = dto.Label
                };
                projectModels.Add(newProjectDataModel);
                return newProjectDataModel;
            });
            _projectModelService.Setup(p => p.GetProjectDataModels(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(projectModels);
            _projectModelService.Setup(p => p.GetProjectDataModelByName(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync((int projectId, string name) =>
                projectModels.FirstOrDefault(p => p.ProjectId == projectId && p.Name == name));
        }

        [Fact]
        public void Model_Execute_ReturnsEmpty()
        {
            var command = new ModelCommand(_console.Object, LoggerMock.GetLogger<ModelCommand>().Object);
            var resultMessage = command.Execute();

            Assert.Equal("", resultMessage);
        }

        [Fact]
        public void ModelAdd_Execute_ReturnsSuccessMessage()
        {
            var command = new AddCommand(_console.Object, LoggerMock.GetLogger<AddCommand>().Object, _projectService.Object, _projectModelService.Object)
            {
                Project = "Project 1",
                Name = "Tag"
            };

            var resultMessage = command.Execute();

            Assert.StartsWith("Model Tag was added to project Project 1:", resultMessage);
        }

        [Fact]
        public void ModelAdd_Execute_ReturnsNotFoundMessage()
        {
            var command = new AddCommand(_console.Object, LoggerMock.GetLogger<AddCommand>().Object, _projectService.Object, _projectModelService.Object)
            {
                Project = "Project 2",
                Name = "Tag"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Project Project 2 is not found", resultMessage);
        }

        [Fact]
        public void ModelList_Execute_ReturnsSuccessMessage()
        {
            var command = new ListCommand(_console.Object, LoggerMock.GetLogger<ListCommand>().Object, _projectService.Object, _projectModelService.Object)
            {
                Project = "Project 1"
            };

            var resultMessage = command.Execute();

            Assert.StartsWith("Models in project Project 1:", resultMessage);
        }

        [Fact]
        public void ModelList_Execute_ReturnsNotFoundMessage()
        {
            var command = new ListCommand(_console.Object, LoggerMock.GetLogger<ListCommand>().Object, _projectService.Object, _projectModelService.Object)
            {
                Project = "Project 2"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Project Project 2 is not found", resultMessage);
        }

        [Fact]
        public void ModelRemove_Execute_ReturnsSuccessMessage()
        {
            var command = new RemoveCommand(_console.Object, LoggerMock.GetLogger<RemoveCommand>().Object, _projectService.Object, _projectModelService.Object)
            {
                Project = "Project 1",
                Name = "Product"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Model Product was removed", resultMessage);
        }

        [Fact]
        public void ModelRemove_Execute_ReturnsNotFoundMessage()
        {
            var command = new RemoveCommand(_console.Object, LoggerMock.GetLogger<RemoveCommand>().Object, _projectService.Object, _projectModelService.Object)
            {
                Project = "Project 1",
                Name = "Tag"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Failed removing model Tag. Make sure the project and model names are correct.", resultMessage);
        }

        [Fact]
        public void ModelUpdate_Execute_ReturnsSuccessMessage()
        {
            var command = new UpdateCommand(_console.Object, LoggerMock.GetLogger<UpdateCommand>().Object, _projectService.Object, _projectModelService.Object)
            {
                Project = "Project 1",
                Name = "Product"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Model Product was updated", resultMessage);
        }

        [Fact]
        public void ModelUpdate_Execute_ReturnsNotFoundMessage()
        {
            var command = new UpdateCommand(_console.Object, LoggerMock.GetLogger<UpdateCommand>().Object, _projectService.Object, _projectModelService.Object)
            {
                Project = "Project 1",
                Name = "Tag"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Failed updating model Tag. Make sure the project and model names are correct.", resultMessage);
        }
    }
}
