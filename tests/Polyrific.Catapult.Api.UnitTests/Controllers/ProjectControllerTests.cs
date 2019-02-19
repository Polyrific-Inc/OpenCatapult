// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Api.Controllers;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.UnitTests.Utilities;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.Project;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Controllers
{
    public class ProjectControllerTests
    {
        private readonly Mock<IProjectService> _projectService;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ProjectController>> _logger;

        public ProjectControllerTests()
        {
            _projectService = new Mock<IProjectService>();

            _mapper = AutoMapperUtils.GetMapper();

            _logger = LoggerMock.GetLogger<ProjectController>();
        }

        [Fact]
        public async void GetProjects_ReturnsProjectList()
        {
            _projectService.Setup(s => s.GetProjectsByUser(1, It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<(Project, ProjectMemberRole)>
                {
                    (new Project
                    {
                        Id = 1,
                        Name = "Project01"
                    }, new ProjectMemberRole
                    {
                        Id = 1,
                        Name = MemberRole.Owner
                    })
                });

            var httpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new[]
                {
                    new ClaimsIdentity(new[] {new Claim(ClaimTypes.NameIdentifier, "1")})
                })
            };

            var controller = new ProjectController(_projectService.Object, _mapper,
                _logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };

            var result = await controller.GetProjects();

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ProjectDto>>(okActionResult.Value);
            Assert.NotEmpty(returnValue);
        }

        [Fact]
        public async void GetProjectById_ReturnsProject()
        {
            _projectService.Setup(s => s.GetProjectById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) =>
                    new Project
                    {
                        Id = id,
                        Name = "Project01"
                    });

            var controller = new ProjectController(_projectService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetProject(1);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProjectDto>(okActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void GetProjectByName_ReturnsProject()
        {
            _projectService.Setup(s => s.GetProjectByName(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string serviceName, CancellationToken cancellationToken) =>
                    new Project
                    {
                        Id = 1,
                        Name = serviceName
                    });

            var controller = new ProjectController(_projectService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetProjectByName("Project01");

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProjectDto>(okActionResult.Value);
            Assert.Equal("Project01", returnValue.Name);
        }

        [Fact]
        public async void CreateProject_ReturnsCreatedProject()
        {
            _projectService
                .Setup(s => s.CreateProject(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), 
                    It.IsAny<List<(int,int)>>(), It.IsAny<List<ProjectDataModel>>(), It.IsAny<List<JobDefinition>>(),
                    It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string projectName, string displayName, string client, List<(int, int)> projectMembers, 
                List<ProjectDataModel> models, List<JobDefinition> jobs, int currentUserId, CancellationToken cancellationToken) =>
                    new Project
                    {
                        Id = 1,
                        Name = projectName,
                        Client = client,
                        Models = models,
                        Jobs = jobs
                    });

            var httpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new[]
                {
                    new ClaimsIdentity(new[] {new Claim(ClaimTypes.NameIdentifier, "1")})
                })
            };

            var controller = new ProjectController(_projectService.Object, _mapper, _logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };

            var dto = new NewProjectDto
            {
                Name = "Project01",
                Members = new List<Shared.Dto.NewProjectMemberDto>(),
                Models = new List<Shared.Dto.ProjectDataModel.CreateProjectDataModelWithPropertiesDto>(),
                Jobs = new List<Shared.Dto.JobDefinition.CreateJobDefinitionWithTasksDto>()
            };
            var result = await controller.CreateProject(dto);

            var createAtRouteActionResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<ProjectDto>(createAtRouteActionResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Project01", returnValue.Name);
        }

        [Fact]
        public async void UpdateProject_ReturnsSuccess()
        {
            _projectService.Setup(s => s.UpdateProject(It.IsAny<Project>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var controller = new ProjectController(_projectService.Object, _mapper, _logger.Object);

            var result = await controller.UpdateProject(1, new UpdateProjectDto
            {
                Id = 1
            });

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void UpdateProject_ReturnsBadRequest()
        {
            var controller = new ProjectController(_projectService.Object, _mapper, _logger.Object);

            var result = await controller.UpdateProject(1, new UpdateProjectDto());

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Project Id doesn't match.", badRequestResult.Value);
        }

        [Fact]
        public async void CloneProject_ReturnsCreatedProject()
        {
            _projectService
                .Setup(s => s.CloneProject(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
                    It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, string newProjectName, string displayName, string client, int ownerUserId, bool includeMembers, 
                    bool includeJobDefinitions, CancellationToken cancellationToken) =>
                    new Project
                    {
                        Id = 2,
                        Name = newProjectName
                    });

            var httpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new[]
                {
                    new ClaimsIdentity(new[] {new Claim(ClaimTypes.NameIdentifier, "1")})
                })
            };

            var controller = new ProjectController(_projectService.Object, _mapper, _logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };

            var dto = new CloneProjectOptionDto
            {
                NewProjectName = "Project02"
            };
            var result = await controller.CloneProject(1, dto);

            var createAtRouteActionResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<ProjectDto>(createAtRouteActionResult.Value);
            Assert.Equal(2, returnValue.Id);
            Assert.Equal("Project02", returnValue.Name);
        }

        [Fact]
        public async void DeleteProject_ReturnsNoContent()
        {
            _projectService.Setup(s => s.DeleteProject(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var controller = new ProjectController(_projectService.Object, _mapper, _logger.Object);

            var result = await controller.DeleteProject(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void ArchiveProject_ReturnsNoContent()
        {
            _projectService.Setup(s => s.ArchiveProject(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var controller = new ProjectController(_projectService.Object, _mapper, _logger.Object);

            var result = await controller.ArchiveProject(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void RestoreProject_ReturnsSuccess()
        {
            _projectService.Setup(s => s.RestoreProject(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var controller = new ProjectController(_projectService.Object, _mapper, _logger.Object);

            var result = await controller.RestoreProject(1);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void ExportProject_ReturnsSuccess()
        {
            _projectService.Setup(s => s.ExportProject(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("yamlText");

            var controller = new ProjectController(_projectService.Object, _mapper, _logger.Object);

            var result = await controller.ExportProject(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("yamlText", okResult.Value);
        }
    }
}
