// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Moq;
using Polyrific.Catapult.Cli.Commands;
using Polyrific.Catapult.Cli.Commands.Property;
using Polyrific.Catapult.Cli.UnitTests.Commands.Utilities;
using Polyrific.Catapult.Shared.Dto.Project;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;
using Polyrific.Catapult.Shared.Service;
using Xunit;
using Xunit.Abstractions;

namespace Polyrific.Catapult.Cli.UnitTests.Commands
{
    public class PropertyCommandTests
    {
        private readonly IConsole _console;
        private readonly Mock<IProjectService> _projectService;
        private readonly Mock<IProjectDataModelService> _projectModelService;
        private readonly ITestOutputHelper _output;

        public PropertyCommandTests(ITestOutputHelper output)
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

            var models = new List<ProjectDataModelDto>
            {
                new ProjectDataModelDto
                {
                    Id = 1,
                    ProjectId = 1,
                    Name = "Product"
                }
            };

            var properties = new List<ProjectDataModelPropertyDto>
            {
                new ProjectDataModelPropertyDto
                {
                    Id = 1,
                    ProjectDataModelId = 1,
                    Name = "Name"
                }
            };

            _console = new TestConsole(output);

            _projectService = new Mock<IProjectService>();
            _projectService.Setup(p => p.GetProjectByName(It.IsAny<string>())).ReturnsAsync((string name) => projects.FirstOrDefault(p => p.Name == name));

            _projectModelService = new Mock<IProjectDataModelService>();
            _projectModelService.Setup(p => p.GetProjectDataModelByName(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync((int projectId, string name) =>
                models.FirstOrDefault(p => p.ProjectId == projectId && p.Name == name));
            _projectModelService.Setup(p => p.CreateProjectDataModelProperty(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CreateProjectDataModelPropertyDto>())).ReturnsAsync((int projectId, int modelId, CreateProjectDataModelPropertyDto dto) =>
            {
                return new ProjectDataModelPropertyDto
                {
                    Id = 2,
                    Name = dto.Name,
                    Label = dto.Label,
                    RelatedProjectDataModelId = dto.RelatedProjectDataModelId
                };
            });
            _projectModelService.Setup(p => p.GetProjectDataModelProperties(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(properties);
            _projectModelService.Setup(p => p.GetProjectDataModelPropertyByName(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync((int projectId, int modelId, string name) =>
                properties.FirstOrDefault(p => p.ProjectDataModelId == modelId && p.Name == name));
        }

        [Fact]
        public void Property_Execute_ReturnsEmpty()
        {
            var command = new PropertyCommand(_console, LoggerMock.GetLogger<PropertyCommand>().Object);
            var resultMessage = command.Execute();

            Assert.Equal("", resultMessage);
        }

        [Fact]
        public void PropertyAdd_Execute_ReturnsSuccessMessage()
        {
            var command = new AddCommand(_console, LoggerMock.GetLogger<AddCommand>().Object, _projectService.Object, _projectModelService.Object)
            {
                Project = "Project 1",
                Model = "Product",
                Name = "Quantity"
            };

            var resultMessage = command.Execute();

            Assert.StartsWith("Property Quantity was added to model Product:", resultMessage);
        }

        [Fact]
        public void PropertyAdd_Execute_RelationalExistReturnsSuccessMessage()
        {
            var command = new AddCommand(_console, LoggerMock.GetLogger<AddCommand>().Object, _projectService.Object, _projectModelService.Object)
            {
                Project = "Project 1",
                Model = "Product",
                Name = "Parent",
                Relational = "Product"
            };

            var resultMessage = command.Execute();

            Assert.StartsWith("Property Parent was added to model Product:", resultMessage);
            _projectModelService.Verify(s => s.CreateProjectDataModelProperty(1, 1, It.Is<CreateProjectDataModelPropertyDto>(p => p.RelatedProjectDataModelId == 1)), Times.Once);
        }

        [Fact]
        public void PropertyAdd_Execute_ReturnsRelatedModelNotFoundMessage()
        {
            var command = new AddCommand(_console, LoggerMock.GetLogger<AddCommand>().Object, _projectService.Object, _projectModelService.Object)
            {
                Project = "Project 1",
                Model = "Product",
                Name = "Tag",
                Relational = "Tag"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Related model Tag was not found", resultMessage);
        }

        [Fact]
        public void PropertyAdd_Execute_ReturnsNotFoundMessage()
        {
            var command = new AddCommand(_console, LoggerMock.GetLogger<AddCommand>().Object, _projectService.Object, _projectModelService.Object)
            {
                Project = "Project 1",
                Model = "Tag",
                Name = "Quantity"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Failed to add property Quantity. Make sure the project and model names are correct.", resultMessage);
        }

        [Fact]
        public void PropertyList_Execute_ReturnsSuccessMessage()
        {
            var command = new ListCommand(_console, LoggerMock.GetLogger<ListCommand>().Object, _projectService.Object, _projectModelService.Object)
            {
                Project = "Project 1",
                Model = "Product",
            };

            var resultMessage = command.Execute();

            Assert.StartsWith("Found 1 property:", resultMessage);
        }

        [Fact]
        public void PropertyList_Execute_ReturnsNotFoundMessage()
        {
            var command = new ListCommand(_console, LoggerMock.GetLogger<ListCommand>().Object, _projectService.Object, _projectModelService.Object)
            {
                Project = "Project 1",
                Model = "Tag"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Failed to fetch properties. Make sure the project and model names are correct.", resultMessage);
        }

        [Fact]
        public void PropertyRemove_Execute_ReturnsSuccessMessage()
        {
            var console = new TestConsole(_output, "y");
            var command = new RemoveCommand(console, LoggerMock.GetLogger<RemoveCommand>().Object, _projectService.Object, _projectModelService.Object)
            {
                Project = "Project 1",
                Model = "Product",
                Name = "Name",
            };

            var resultMessage = command.Execute();

            Assert.Equal("Property Name has been removed successfully", resultMessage);
        }

        [Fact]
        public void PropertyRemove_Execute_ReturnsNotFoundMessage()
        {
            var console = new TestConsole(_output, "y");
            var command = new RemoveCommand(console, LoggerMock.GetLogger<RemoveCommand>().Object, _projectService.Object, _projectModelService.Object)
            {
                Project = "Project 1",
                Model = "Product",
                Name = "Quantity",
            };

            var resultMessage = command.Execute();

            Assert.Equal("Failed to remove property Quantity. Make sure the project, model, and property names are correct.", resultMessage);
        }

        [Fact]
        public void PropertyUpdate_Execute_ReturnsSuccessMessage()
        {
            var command = new UpdateCommand(_console, LoggerMock.GetLogger<UpdateCommand>().Object, _projectService.Object, _projectModelService.Object)
            {
                Project = "Project 1",
                Model = "Product",
                Name = "Name",
                Rename = "Quantity"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Property Name has been updated successfully", resultMessage);
        }

        [Fact]
        public void PropertyUpdate_Execute_RelationalExistReturnsSuccessMessage()
        {
            var command = new UpdateCommand(_console, LoggerMock.GetLogger<UpdateCommand>().Object, _projectService.Object, _projectModelService.Object)
            {
                Project = "Project 1",
                Model = "Product",
                Name = "Name",
                Relational = "Product"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Property Name has been updated successfully", resultMessage);
            _projectModelService.Verify(s => s.UpdateProjectDataModelProperty(1, 1, 1, It.Is<UpdateProjectDataModelPropertyDto>(p => p.RelatedProjectDataModelId == 1)), Times.Once);
        }

        [Fact]
        public void PropertyUpdate_Execute_ReturnsRelatedModelNotFoundMessage()
        {
            var command = new UpdateCommand(_console, LoggerMock.GetLogger<UpdateCommand>().Object, _projectService.Object, _projectModelService.Object)
            {
                Project = "Project 1",
                Model = "Product",
                Name = "Tag",
                Relational = "Tag"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Related model Tag was not found", resultMessage);
        }

        [Fact]
        public void PropertyUpdate_Execute_ReturnsNotFoundMessage()
        {
            var command = new UpdateCommand(_console, LoggerMock.GetLogger<UpdateCommand>().Object, _projectService.Object, _projectModelService.Object)
            {
                Project = "Project 1",
                Name = "Tag"
            };

            var resultMessage = command.Execute();

            Assert.Equal("Failed to update property Tag. Make sure the project, model, and property names are correct.", resultMessage);
        }
    }
}
