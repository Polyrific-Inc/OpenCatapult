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
using Polyrific.Catapult.Shared.Common.Notification;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.ProjectMember;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Controllers
{
    public class ProjectMemberControllerTests
    {
        private readonly Mock<IProjectMemberService> _projectMemberService;
        private readonly Mock<IUserService> _userService;
        private readonly Mock<INotificationProvider> _notificationProvider;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ProjectMemberController>> _logger;

        public ProjectMemberControllerTests()
        {
            _projectMemberService = new Mock<IProjectMemberService>();
            _userService = new Mock<IUserService>();
            _notificationProvider = new Mock<INotificationProvider>();

            _mapper = AutoMapperUtils.GetMapper();

            _logger = LoggerMock.GetLogger<ProjectMemberController>();
        }

        [Fact]
        public async void GetProjectMembers_ReturnsProjectMemberList()
        {
            _projectMemberService.Setup(s => s.GetProjectMembers(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProjectMember>
                {
                    new ProjectMember
                    {
                        Id = 1,
                        UserId = 1,
                        ProjectMemberRoleId = 1
                    }
                });

            var httpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new[]
                {
                    new ClaimsIdentity(new[] {new Claim(ClaimTypes.NameIdentifier, "1")})
                })
            };

            var controller = new ProjectMemberController(_projectMemberService.Object, _userService.Object, _notificationProvider.Object, _mapper,
                _logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };

            var result = await controller.GetProjectMembers(1, 1);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ProjectMemberDto>>(okActionResult.Value);
            Assert.NotEmpty(returnValue);
        }

        [Fact]
        public async void CreateProjectMember_ReturnsCreatedProjectMember()
        {
            _projectMemberService
                .Setup(s => s.AddProjectMember(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
            _projectMemberService.Setup(s => s.GetProjectMemberById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) =>
                    new ProjectMember
                    {
                        Id = id,
                        UserId = 1,
                        ProjectMemberRoleId = 1
                    });

            var controller = new ProjectMemberController(_projectMemberService.Object, _userService.Object, _notificationProvider.Object, _mapper, _logger.Object);

            var dto = new NewProjectMemberDto
            {
                ProjectId = 1,
                UserId = 1,
                ProjectMemberRoleId = 1
            };
            var result = await controller.CreateProjectMember(1, dto);

            var createAtRouteActionResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<ProjectMemberDto>(createAtRouteActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void CreateProjectMemberNewUser_ReturnsCreatedProjectMember()
        {
            _projectMemberService
                .Setup(s => s.AddProjectMember(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((1, 1));
            _projectMemberService.Setup(s => s.GetProjectMemberById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) =>
                    new ProjectMember
                    {
                        Id = id,
                        UserId = 1,
                        ProjectMemberRoleId = 1
                    });

            var httpContext = new DefaultHttpContext()
            {
                Request = { Scheme = "https", Host = new HostString("localhost") }
            };

            var controller = new ProjectMemberController(_projectMemberService.Object, _userService.Object, _notificationProvider.Object, _mapper, _logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };

            var dto = new NewProjectMemberDto
            {
                ProjectId = 1,
                ProjectMemberRoleId = 1
            };
            var result = await controller.CreateProjectMember(1, dto);

            var createAtRouteActionResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<ProjectMemberDto>(createAtRouteActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void CreateProject_ReturnsBadRequest()
        {

            var controller = new ProjectMemberController(_projectMemberService.Object, _userService.Object, _notificationProvider.Object, _mapper, _logger.Object);

            var dto = new NewProjectMemberDto
            {
                ProjectMemberRoleId = 1
            };
            var result = await controller.CreateProjectMember(1, dto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Project Id doesn't match.", badRequestResult.Value);
        }

        [Fact]
        public async void GetProjectMemberById_ReturnsProjectMember()
        {
            _projectMemberService.Setup(s => s.GetProjectMemberById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) =>
                    new ProjectMember
                    {
                        Id = id,
                        UserId = 1,
                        ProjectMemberRoleId = 1
                    });

            var controller = new ProjectMemberController(_projectMemberService.Object, _userService.Object, _notificationProvider.Object, _mapper,
                _logger.Object);

            var result = await controller.GetProjectMember(1, 1);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProjectMemberDto>(okActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void GetProjectMemberByName_ReturnsProjectMember()
        {
            _projectMemberService.Setup(s => s.GetProjectMemberByUserId(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int projectId, int userId, CancellationToken cancellationToken) =>
                    new ProjectMember
                    {
                        Id = 1,
                        ProjectId = projectId,
                        UserId = userId,
                        ProjectMemberRoleId = 1
                    });

            var controller = new ProjectMemberController(_projectMemberService.Object, _userService.Object, _notificationProvider.Object, _mapper,
                _logger.Object);

            var result = await controller.GetProjectMemberByUserId(1, 1);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProjectMemberDto>(okActionResult.Value);
            Assert.Equal(1, returnValue.ProjectId);
            Assert.Equal(1, returnValue.UserId);
        }

        [Fact]
        public async void UpdateProjectMember_ReturnsSuccess()
        {
            _projectMemberService.Setup(s => s.UpdateProjectMemberRole(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var controller = new ProjectMemberController(_projectMemberService.Object, _userService.Object, _notificationProvider.Object, _mapper, _logger.Object);

            var result = await controller.UpdateProjectMember(1, 1, new UpdateProjectMemberDto
            {
                Id = 1
            });

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void UpdateProjectMember_ReturnsBadRequest()
        {
            var controller = new ProjectMemberController(_projectMemberService.Object, _userService.Object, _notificationProvider.Object, _mapper, _logger.Object);

            var result = await controller.UpdateProjectMember(1, 1, new UpdateProjectMemberDto());

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Member Id doesn't match.", badRequestResult.Value);
        }

        [Theory]
        [InlineData(UserRole.Administrator)]
        [InlineData(UserRole.Basic)]
        public async void DeleteProjectMember_ReturnsNoContent(string role)
        {
            _projectMemberService.Setup(s => s.GetProjectMemberById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) =>
                    new ProjectMember
                    {
                        Id = id,
                        UserId = 1,
                        ProjectMemberRoleId = 1
                    });

            _projectMemberService.Setup(s => s.RemoveProjectMember(1, 1, role != UserRole.Administrator ? 1 : 0, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var httpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new[]
                {
                    new ClaimsIdentity(new[] {new Claim(ClaimTypes.NameIdentifier, "1")}),
                    new ClaimsIdentity(new[] {new Claim(ClaimTypes.Role, role) })
                })
            };

            var controller = new ProjectMemberController(_projectMemberService.Object, _userService.Object, _notificationProvider.Object, _mapper, _logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };

            var result = await controller.RemoveProjectMember(1, 1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
