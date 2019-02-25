// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Api.Controllers;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.UnitTests.Utilities;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Controllers
{
    public class ProjectDataModelControllerTests
    {
        private readonly Mock<IProjectDataModelService> _projectDataModelService;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ProjectDataModelController>> _logger;

        public ProjectDataModelControllerTests()
        {
            _projectDataModelService = new Mock<IProjectDataModelService>();

            _mapper = AutoMapperUtils.GetMapper();

            _logger = LoggerMock.GetLogger<ProjectDataModelController>();
        }

        [Fact]
        public async void GetProjectDataModels_ReturnsProjectDataModelList()
        {
            _projectDataModelService.Setup(s => s.GetProjectDataModels(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProjectDataModel>
                {
                    new ProjectDataModel
                    {
                        Id = 1,
                        Name = "Product"
                    }
                });

            var controller = new ProjectDataModelController(_projectDataModelService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetProjectDataModels(1);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ProjectDataModelDto>>(okActionResult.Value);
            Assert.NotEmpty(returnValue);
        }

        [Fact]
        public async void CreateProjectDataModel_ReturnsCreatedProjectDataModel()
        {
            _projectDataModelService
                .Setup(s => s.AddProjectDataModel(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
            _projectDataModelService.Setup(s => s.GetProjectDataModelById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) =>
                    new ProjectDataModel
                    {
                        Id = id,
                        Name = "Product"
                    });

            var controller = new ProjectDataModelController(_projectDataModelService.Object, _mapper, _logger.Object);

            var dto = new CreateProjectDataModelDto
            {
                Name = "Product"
            };
            var result = await controller.CreateProjectDataModel(1, dto);

            var okActionResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<ProjectDataModelDto>(okActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void GetProjectDataModelById_ReturnsProjectDataModel()
        {
            _projectDataModelService.Setup(s => s.GetProjectDataModelById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) =>
                    new ProjectDataModel
                    {
                        Id = id,
                        Name = "Product"
                    });

            var controller = new ProjectDataModelController(_projectDataModelService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetProjectDataModel(1, 1);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProjectDataModelDto>(okActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void GetProjectDataModelByName_ReturnsProjectDataModel()
        {
            _projectDataModelService.Setup(s => s.GetProjectDataModelByName(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int projectId, string name, CancellationToken cancellationToken) =>
                    new ProjectDataModel
                    {
                        Id = 1,
                        Name = name,
                        ProjectId = projectId
                    });

            var controller = new ProjectDataModelController(_projectDataModelService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetProjectDataModel(1, "Product");

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProjectDataModelDto>(okActionResult.Value);
            Assert.Equal("Product", returnValue.Name);
            Assert.Equal(1, returnValue.ProjectId);
        }

        [Fact]
        public async void UpdateProjectDataModel_ReturnsSuccess()
        {
            _projectDataModelService.Setup(s => s.UpdateDataModel(It.IsAny<ProjectDataModel>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var controller = new ProjectDataModelController(_projectDataModelService.Object, _mapper, _logger.Object);

            var result = await controller.UpdateProjectDataModel(1, 1, new UpdateProjectDataModelDto()
            {
                Id = 1
            });

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void UpdateProjectDataModel_ReturnsBadRequest()
        {
            var controller = new ProjectDataModelController(_projectDataModelService.Object, _mapper, _logger.Object);

            var result = await controller.UpdateProjectDataModel(1, 1, new UpdateProjectDataModelDto());

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Model Id doesn't match.", badRequestResult.Value);
        }

        [Fact]
        public async void DeleteProjectDataModel_ReturnsNoContent()
        {
            _projectDataModelService.Setup(s => s.DeleteDataModel(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var controller = new ProjectDataModelController(_projectDataModelService.Object, _mapper, _logger.Object);

            var result = await controller.DeleteProjectDataModel(1, 1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void DeleteProjectDataModels_ReturnsNoContent()
        {
            _projectDataModelService.Setup(s => s.DeleteDataModels(It.IsAny<int>(), It.IsAny<int[]>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var controller = new ProjectDataModelController(_projectDataModelService.Object, _mapper, _logger.Object);

            var result = await controller.DeleteProjectDataModels(1, new int[] { 1 });

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void GetProjectDataModelProperties_ReturnsProjectDataModelPropertyList()
        {
            _projectDataModelService.Setup(s => s.GetDataModelProperties(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProjectDataModelProperty>
                {
                    new ProjectDataModelProperty
                    {
                        Id = 1,
                        Name = "Title"
                    }
                });

            var controller = new ProjectDataModelController(_projectDataModelService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetProjectDataModelProperties(1, 1);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ProjectDataModelPropertyDto>>(okActionResult.Value);
            Assert.NotEmpty(returnValue);
        }

        [Fact]
        public async void CreateProjectDataModelProperty_ReturnsCreatedProjectDataModelProperty()
        {
            _projectDataModelService
                .Setup(s => s.AddDataModelProperty(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), 
                    It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            _projectDataModelService.Setup(s => s.GetProjectDataModelPropertyByName(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int projectDataModelId, string name, CancellationToken cancellationToken) =>
                    new ProjectDataModelProperty
                    {
                        Id = 1,
                        Name = name,
                        ProjectDataModelId = projectDataModelId
                    });

            var controller = new ProjectDataModelController(_projectDataModelService.Object, _mapper, _logger.Object);

            var dto = new CreateProjectDataModelPropertyDto
            {
                Name = "Title"
            };
            var result = await controller.CreateProjectDataModelProperty(1, 1, dto);

            var okActionResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<ProjectDataModelPropertyDto>(okActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void GetProjectDataModelPropertyById_ReturnsProjectDataModelProperty()
        {
            _projectDataModelService.Setup(s => s.GetProjectDataModelPropertyById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) =>
                    new ProjectDataModelProperty
                    {
                        Id = id,
                        Name = "Title"
                    });

            var controller = new ProjectDataModelController(_projectDataModelService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetProjectDataModelProperty(1, 1, 1);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProjectDataModelPropertyDto>(okActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void GetProjectDataModelPropertyByName_ReturnsProjectDataModelProperty()
        {
            _projectDataModelService.Setup(s => s.GetProjectDataModelPropertyByName(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int projectDataModelId, string name, CancellationToken cancellationToken) =>
                    new ProjectDataModelProperty
                    {
                        Id = 1,
                        Name = name,
                        ProjectDataModelId = projectDataModelId
                    });

            var controller = new ProjectDataModelController(_projectDataModelService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetProjectDataModelProperty(1, 1, "Title");

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProjectDataModelPropertyDto>(okActionResult.Value);
            Assert.Equal("Title", returnValue.Name);
            Assert.Equal(1, returnValue.ProjectDataModelId);
        }

        [Fact]
        public async void UpdateProjectDataModelProperty_ReturnsSuccess()
        {
            _projectDataModelService.Setup(s => s.UpdateDataModelProperty(It.IsAny<ProjectDataModelProperty>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var controller = new ProjectDataModelController(_projectDataModelService.Object, _mapper, _logger.Object);

            var result = await controller.UpdateProjectDataModelProperty(1, 1, 1, new UpdateProjectDataModelPropertyDto()
            {
                Id = 1
            });

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void UpdateProjectDataModelProperty_ReturnsBadRequest()
        {
            var controller = new ProjectDataModelController(_projectDataModelService.Object, _mapper, _logger.Object);

            var result = await controller.UpdateProjectDataModelProperty(1, 1, 1, new UpdateProjectDataModelPropertyDto());

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Property Id doesn't match.", badRequestResult.Value);
        }

        [Fact]
        public async void DeleteProjectDataModelProperty_ReturnsNoContent()
        {
            _projectDataModelService.Setup(s => s.DeleteDataModelProperty(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var controller = new ProjectDataModelController(_projectDataModelService.Object, _mapper, _logger.Object);

            var result = await controller.DeleteProjectDataModelProperty(1, 1, 1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
