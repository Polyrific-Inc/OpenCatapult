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
using Polyrific.Catapult.Shared.Dto.User;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Controllers
{
    public class AccountControllerTests
    {
        private readonly Mock<IUserService> _userService;
        private readonly IMapper _mapper;
        private readonly Mock<INotificationProvider> _notificationProvider;
        private readonly Mock<ILogger<AccountController>> _logger;

        public AccountControllerTests()
        {
            _userService = new Mock<IUserService>();
            
            _mapper = AutoMapperUtils.GetMapper();
            
            _notificationProvider = new Mock<INotificationProvider>();

            _logger = LoggerMock.GetLogger<AccountController>();
        }

        [Fact]
        public async void RegisterUser_ReturnsRegisteredUser()
        {
            _userService.Setup(s => s.GeneratePassword(It.IsAny<int>())).ReturnsAsync("0123456789");
            _userService
                .Setup(s => s.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string email, string firstName, string lastName, string password, CancellationToken cancellationToken) => 
                    new User
                    {
                        Id = 1,
                        Email = email,
                        UserName = email,
                        FirstName = firstName,
                        LastName = lastName
                    });
            _userService.Setup(s => s.GenerateConfirmationToken(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync("xxx");
            _userService.Setup(s => s.GetUserById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(new User
            {
                Id = 1
            });
            _notificationProvider.Setup(n => n.SendNotification(It.IsAny<SendNotificationRequest>(), It.IsAny<Dictionary<string, string>>()))
                .Returns(Task.CompletedTask);

            var httpContext = new DefaultHttpContext()
            {
                Request = {Scheme = "https", Host = new HostString("localhost")}
            };
            var controller =
                new AccountController(_userService.Object, _mapper, _notificationProvider.Object, _logger.Object)
                {
                    ControllerContext = new ControllerContext {HttpContext = httpContext}
                };

            var dto = new RegisterUserDto
            {
                Email = "user@example.com"
            };
            var result = await controller.RegisterUser(dto);
            
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<UserDto>(okActionResult.Value);
            Assert.Equal("1", returnValue.Id);
            _notificationProvider.Verify(n => n.SendNotification(It.IsAny<SendNotificationRequest>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }

        [Fact]
        public async void ConfirmEmail_ReturnSuccessMessage()
        {
            _userService.Setup(s => s.ConfirmEmail(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var controller = new AccountController(_userService.Object, _mapper, _notificationProvider.Object,
                _logger.Object);

            var result = await controller.ConfirmEmail(1, "xxx");

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Email confirmed.", okActionResult.Value);
        }

        [Fact]
        public async void GetUsers_ReturnUserList()
        {
            _userService.Setup(s => s.GetUsers(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<User>
                {
                    new User("test@example.com")
                });

            var controller = new AccountController(_userService.Object, _mapper, _notificationProvider.Object,
                _logger.Object);

            var result = await controller.GetUsers(null, null);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<UserDto>>(okActionResult.Value);
            Assert.NotEmpty(returnValue);
        }

        [Fact]
        public async void GetUser_ReturnUser()
        {
            _userService.Setup(s => s.GetUserById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                (int id, CancellationToken cancellationToken) => new User
                {
                    Id = id
                });
            
            var httpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new[]
                {
                    new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "1"),
                        new Claim(ClaimTypes.Role, UserRole.Administrator)
                    })
                })
            };

            var controller =
                new AccountController(_userService.Object, _mapper, _notificationProvider.Object, _logger.Object)
                {
                    ControllerContext = new ControllerContext {HttpContext = httpContext}
                };

            var result = await controller.GetUser(2);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<UserDto>(okActionResult.Value);
            Assert.Equal("2", returnValue.Id);
        }

        [Fact]
        public async void GetCurrentUser_ReturnUser()
        {
            _userService.Setup(s => s.GetUserById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                (int id, CancellationToken cancellationToken) => new User
                {
                    Id = id
                });
            
            var httpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new[]
                {
                    new ClaimsIdentity(new[] {new Claim(ClaimTypes.NameIdentifier, "1")})
                })
            };

            var controller =
                new AccountController(_userService.Object, _mapper, _notificationProvider.Object, _logger.Object)
                {
                    ControllerContext = new ControllerContext {HttpContext = httpContext}
                };

            var result = await controller.GetCurrentUser();

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<UserDto>(okActionResult.Value);
            Assert.Equal("1", returnValue.Id);
        }

        [Fact]
        public async void GetUserByName_ReturnUser()
        {
            _userService.Setup(s => s.GetUser(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string username, CancellationToken cancellationToken) => new User(username));

            var httpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new[]
                {
                    new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, "admin@example.com"),
                        new Claim(ClaimTypes.Role, UserRole.Administrator) 
                    })
                })
            };

            var controller =
                new AccountController(_userService.Object, _mapper, _notificationProvider.Object, _logger.Object)
                {
                    ControllerContext = new ControllerContext {HttpContext = httpContext}
                };

            var result = await controller.GetUserByName("test@example.com");

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<UserDto>(okActionResult.Value);
            Assert.Equal("test@example.com", returnValue.UserName);
        }

        [Fact]
        public async void GetUserByName_ReturnUnauthorized()
        {
            _userService.Setup(s => s.GetUser(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string username, CancellationToken cancellationToken) => new User(username));

            var httpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new[]
                {
                    new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, "basic@example.com"),
                        new Claim(ClaimTypes.Role, UserRole.Basic), 
                    })
                })
            };

            var controller =
                new AccountController(_userService.Object, _mapper, _notificationProvider.Object, _logger.Object)
                {
                    ControllerContext = new ControllerContext {HttpContext = httpContext}
                };

            var result = await controller.GetUserByName("test@example.com");

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async void GetUserByEmail_ReturnUser()
        {
            _userService.Setup(s => s.GetUserEmail(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ClaimsPrincipal principal, CancellationToken cancellationToken) => principal.Identity.Name);
            _userService.Setup(s => s.GetUserByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string email, CancellationToken cancellationToken) => new User(email));

            var httpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new[]
                {
                    new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, "admin@example.com"),
                        new Claim(ClaimTypes.Role, UserRole.Administrator), 
                    })
                })
            };

            var controller =
                new AccountController(_userService.Object, _mapper, _notificationProvider.Object, _logger.Object)
                {
                    ControllerContext = new ControllerContext {HttpContext = httpContext}
                };

            var result = await controller.GetUserByEmail("test@example.com");

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<UserDto>(okActionResult.Value);
            Assert.Equal("test@example.com", returnValue.UserName);
        }

        [Fact]
        public async void UpdateUser_ReturnSuccess()
        {
            var httpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new[]
                {
                    new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "1"),
                        new Claim(ClaimTypes.Role, UserRole.Administrator)
                    })
                })
            };

            var controller =
                new AccountController(_userService.Object, _mapper, _notificationProvider.Object, _logger.Object)
                {
                    ControllerContext = new ControllerContext {HttpContext = httpContext}
                };

            var result = await controller.UpdateUser(2, new UpdateUserDto() { Id = 2});

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void UpdateUser_ReturnBadRequest()
        {
            var httpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new[]
                {
                    new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "1"),
                        new Claim(ClaimTypes.Role, UserRole.Administrator)
                    })
                })
            };

            var controller =
                new AccountController(_userService.Object, _mapper, _notificationProvider.Object, _logger.Object)
                {
                    ControllerContext = new ControllerContext {HttpContext = httpContext}
                };

            var result = await controller.UpdateUser(2, new UpdateUserDto() { Id = 3 });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void SuspendUser_ReturnSuccess()
        {
            _userService.Setup(s => s.Suspend(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var controller = new AccountController(_userService.Object, _mapper, _notificationProvider.Object,
                _logger.Object);

            var result = await controller.SuspendUser(1);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User suspended", okActionResult.Value);
        }

        [Fact]
        public async void ReactivateUser_ReturnSuccess()
        {
            _userService.Setup(s => s.Reactivate(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var controller = new AccountController(_userService.Object, _mapper, _notificationProvider.Object,
                _logger.Object);

            var result = await controller.ReactivateUser(1);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User activated", okActionResult.Value);
        }

        [Fact]
        public async void UpdatePassword_ReturnSuccess()
        {
            _userService
                .Setup(s => s.UpdatePassword(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var httpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new[]
                {
                    new ClaimsIdentity(new[] {new Claim(ClaimTypes.NameIdentifier, "1")})
                })
            };

            var controller =
                new AccountController(_userService.Object, _mapper, _notificationProvider.Object, _logger.Object)
                {
                    ControllerContext = new ControllerContext {HttpContext = httpContext}
                };

            var result = await controller.UpdatePassword(new UpdatePasswordDto());

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Password updated", okActionResult.Value);
        }

        [Fact]
        public async void ResetPassword_GET_ReturnSuccess()
        {
            _userService.Setup(s => s.GetResetPasswordToken(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("xxx");
            _userService.Setup(s => s.GetUserById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => new User() {Id = id});
            _notificationProvider.Setup(n => n.SendNotification(It.IsAny<SendNotificationRequest>(), It.IsAny<Dictionary<string, string>>()))
                .Returns(Task.CompletedTask);

            var controller = new AccountController(_userService.Object, _mapper, _notificationProvider.Object,
                _logger.Object);

            var result = await controller.ResetPassword(1);

            Assert.IsType<OkResult>(result);
            _notificationProvider.Verify(n => n.SendNotification(It.IsAny<SendNotificationRequest>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }

        [Fact]
        public async void ResetPassword_POST_ReturnSuccess()
        {
            _userService
                .Setup(s => s.ResetPassword(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var controller = new AccountController(_userService.Object, _mapper, _notificationProvider.Object,
                _logger.Object);

            var result = await controller.ResetPassword(1, new ResetPasswordDto());

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Reset password success", okActionResult.Value);
        }

        [Fact]
        public async void RemoveUser_ReturnsNoContent()
        {
            _userService.Setup(s => s.DeleteUser(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var controller = new AccountController(_userService.Object, _mapper, _notificationProvider.Object,
                _logger.Object);

            var result = await controller.RemoveUser(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void SetUserRole_ReturnsSuccess()
        {
            _userService
                .Setup(s => s.SetUserRole(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var controller = new AccountController(_userService.Object, _mapper, _notificationProvider.Object,
                _logger.Object);

            var result = await controller.SetUserRole(1, new SetUserRoleDto() {UserId = 1});

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<SetUserRoleDto>(okActionResult.Value);
        }

        [Fact]
        public async void SetUserRole_ReturnsBadRequest()
        {
            _userService
                .Setup(s => s.SetUserRole(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var controller = new AccountController(_userService.Object, _mapper, _notificationProvider.Object,
                _logger.Object);

            var result = await controller.SetUserRole(1, new SetUserRoleDto() {UserId = 2});

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User Id doesn't match.", badRequestResult.Value);
        }
    }
}
