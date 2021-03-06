﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Api.Controllers;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.UnitTests.Utilities;
using Polyrific.Catapult.Shared.Dto.CatapultEngine;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.User;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Controllers
{
    public class TokenControllerTests
    {
        private readonly Mock<IUserService> _userService;
        private readonly Mock<IProjectService> _projectService;
        private readonly Mock<ICatapultEngineService> _catapultEngineService;
        private readonly Mock<IConfiguration> _configuration;
        private readonly ApplicationSettingValue _applicationSetting;
        private readonly Mock<ILogger<TokenController>> _logger;

        public TokenControllerTests()
        {
            _userService = new Mock<IUserService>();
            _projectService = new Mock<IProjectService>();
            _catapultEngineService = new Mock<ICatapultEngineService>();
            _configuration = new Mock<IConfiguration>();
            _applicationSetting = new ApplicationSettingValue
            {
                EnableTwoFactorAuth = true
            };
            _logger = LoggerMock.GetLogger<TokenController>();
        }

        [Fact]
        public async void RequestToken_ReturnsSuccess()
        {
            _userService.Setup(s => s.ValidateUserPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Api.Core.Entities.SignInResult()
                {
                    Succeeded = true
                });
            _userService.Setup(s => s.GetUser(It.IsAny<string>(),It.IsAny<CancellationToken>()))
                .ReturnsAsync((string email, CancellationToken cancellationToken) => new User
                {
                    Id = 1,
                    Email = email,
                    UserName = email,
                    IsActive = true
                });
            _userService.Setup(s => s.GetUserRole(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(UserRole.Administrator);
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

            _configuration.SetupGet(x => x["Security:Tokens:Key"]).Returns("key12345678910abcdefghijklmnopqrstuvwxyz");
            _configuration.SetupGet(x => x["Security:Tokens:Issuer"]).Returns("issuer");
            _configuration.SetupGet(x => x["Security:Tokens:Audience"]).Returns("audience");


            var controller = new TokenController(_userService.Object, _projectService.Object, _catapultEngineService.Object, _configuration.Object, _applicationSetting, _logger.Object);

            var dto = new RequestTokenDto
            {
                UserName = "test@test.com"
            };

            var result = await controller.RequestToken(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<string>(okResult.Value);
            Assert.NotEmpty(returnValue);
        }

        [Fact]
        public async void RequestToken_ReturnsUserPasswordInvalid()
        {
            _userService.Setup(s => s.ValidateUserPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Api.Core.Entities.SignInResult()
                {
                    Succeeded = false
                });
                        
            var controller = new TokenController(_userService.Object, _projectService.Object, _catapultEngineService.Object, _configuration.Object, _applicationSetting, _logger.Object);

            var dto = new RequestTokenDto
            {
                UserName = "test@test.com"
            };

            var result = await controller.RequestToken(dto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Username or password is invalid", badRequestResult.Value);
        }

        [Fact]
        public async void RequestToken_ReturnsUserIsSuspended()
        {
            _userService.Setup(s => s.ValidateUserPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Api.Core.Entities.SignInResult()
                {
                    Succeeded = true
                });
            _userService.Setup(s => s.GetUser(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string email, CancellationToken cancellationToken) => new User
                {
                    Id = 1,
                    Email = email,
                    UserName = email,
                    IsActive = false
                });

            var controller = new TokenController(_userService.Object, _projectService.Object, _catapultEngineService.Object, _configuration.Object, _applicationSetting, _logger.Object);

            var dto = new RequestTokenDto
            {
                UserName = "test@test.com"
            };

            var result = await controller.RequestToken(dto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User is suspended", badRequestResult.Value);
        }

        [Fact]
        public async void RequestToken_ReturnsRequiresTwoFactor()
        {
            _userService.Setup(s => s.ValidateUserPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Api.Core.Entities.SignInResult()
                {
                    Succeeded = false,
                    RequiresTwoFactor = true
                });

            var controller = new TokenController(_userService.Object, _projectService.Object, _catapultEngineService.Object, _configuration.Object, _applicationSetting, _logger.Object);

            var dto = new RequestTokenDto
            {
                UserName = "test@test.com",
                Password = "test"
            };

            var result = await controller.RequestToken(dto);

            var acceptedResult = Assert.IsType<AcceptedResult>(result);
            Assert.Equal(TokenResponses.RequiresTwoFactor, acceptedResult.Value);
        }

        [Fact]
        public async void RequestToken_AuthenticatorCodeIsNotCorrect()
        {
            _userService.Setup(s => s.ValidateUserPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Api.Core.Entities.SignInResult()
                {
                    Succeeded = false,
                    RequiresTwoFactor = true
                });
            _userService.Setup(s => s.VerifyTwoFactorToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var controller = new TokenController(_userService.Object, _projectService.Object, _catapultEngineService.Object, _configuration.Object, _applicationSetting, _logger.Object);

            var dto = new RequestTokenDto
            {
                UserName = "test@test.com",
                Password = "test",
                AuthenticatorCode = "123"
            };

            var result = await controller.RequestToken(dto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Authenticator Code is not correct", badRequestResult.Value);
        }

        [Fact]
        public async void RequestToken_RecoveryCodeIsNotCorrect()
        {
            _userService.Setup(s => s.ValidateUserPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Api.Core.Entities.SignInResult()
                {
                    Succeeded = false,
                    RequiresTwoFactor = true
                });
            _userService.Setup(s => s.RedeemTwoFactorRecoveryCode(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var controller = new TokenController(_userService.Object, _projectService.Object, _catapultEngineService.Object, _configuration.Object, _applicationSetting, _logger.Object);

            var dto = new RequestTokenDto
            {
                UserName = "test@test.com",
                Password = "test",
                RecoveryCode = "123"
            };

            var result = await controller.RequestToken(dto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Recovery Code is not correct", badRequestResult.Value);
        }

        [Fact]
        public async void RequestToken_AuthenticatorCodeIsCorrect()
        {
            _userService.Setup(s => s.ValidateUserPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Api.Core.Entities.SignInResult()
                {
                    Succeeded = false,
                    RequiresTwoFactor = true
                });
            _userService.Setup(s => s.VerifyTwoFactorToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _userService.Setup(s => s.GetUser(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string email, CancellationToken cancellationToken) => new User
                {
                    Id = 1,
                    Email = email,
                    UserName = email,
                    IsActive = true
                });
            _userService.Setup(s => s.GetUserRole(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(UserRole.Administrator);
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

            _configuration.SetupGet(x => x["Security:Tokens:Key"]).Returns("key12345678910abcdefghijklmnopqrstuvwxyz");
            _configuration.SetupGet(x => x["Security:Tokens:Issuer"]).Returns("issuer");
            _configuration.SetupGet(x => x["Security:Tokens:Audience"]).Returns("audience");

            var controller = new TokenController(_userService.Object, _projectService.Object, _catapultEngineService.Object, _configuration.Object, _applicationSetting, _logger.Object);

            var dto = new RequestTokenDto
            {
                UserName = "test@test.com",
                Password = "test",
                AuthenticatorCode = "123"
            };

            var result = await controller.RequestToken(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<string>(okResult.Value);
            Assert.NotEmpty(returnValue);
        }

        [Fact]
        public async void RequestToken_TwoFactorIsDisabled()
        {
            _applicationSetting.EnableTwoFactorAuth = false;
            _userService.Setup(s => s.ValidateUserPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Api.Core.Entities.SignInResult()
                {
                    Succeeded = false,
                    RequiresTwoFactor = true
                });
            _userService.Setup(s => s.GetUser(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string email, CancellationToken cancellationToken) => new User
                {
                    Id = 1,
                    Email = email,
                    UserName = email,
                    IsActive = true
                });
            _userService.Setup(s => s.GetUserRole(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(UserRole.Administrator);
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

            _configuration.SetupGet(x => x["Security:Tokens:Key"]).Returns("key12345678910abcdefghijklmnopqrstuvwxyz");
            _configuration.SetupGet(x => x["Security:Tokens:Issuer"]).Returns("issuer");
            _configuration.SetupGet(x => x["Security:Tokens:Audience"]).Returns("audience");

            var controller = new TokenController(_userService.Object, _projectService.Object, _catapultEngineService.Object, _configuration.Object, _applicationSetting, _logger.Object);

            var dto = new RequestTokenDto
            {
                UserName = "test@test.com",
                Password = "test",
                AuthenticatorCode = "123"
            };

            var result = await controller.RequestToken(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<string>(okResult.Value);
            Assert.NotEmpty(returnValue);
        }

        [Fact]
        public async void RequestToken_RecoveryCodeIsCorrect()
        {
            _userService.Setup(s => s.ValidateUserPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Api.Core.Entities.SignInResult()
                {
                    Succeeded = false,
                    RequiresTwoFactor = true
                });
            _userService.Setup(s => s.RedeemTwoFactorRecoveryCode(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _userService.Setup(s => s.GetUser(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string email, CancellationToken cancellationToken) => new User
                {
                    Id = 1,
                    Email = email,
                    UserName = email,
                    IsActive = true
                });
            _userService.Setup(s => s.GetUserRole(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(UserRole.Administrator);
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

            _configuration.SetupGet(x => x["Security:Tokens:Key"]).Returns("key12345678910abcdefghijklmnopqrstuvwxyz");
            _configuration.SetupGet(x => x["Security:Tokens:Issuer"]).Returns("issuer");
            _configuration.SetupGet(x => x["Security:Tokens:Audience"]).Returns("audience");

            var controller = new TokenController(_userService.Object, _projectService.Object, _catapultEngineService.Object, _configuration.Object, _applicationSetting, _logger.Object);

            var dto = new RequestTokenDto
            {
                UserName = "test@test.com",
                Password = "test",
                RecoveryCode = "123"
            };

            var result = await controller.RequestToken(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<string>(okResult.Value);
            Assert.NotEmpty(returnValue);
        }

        [Fact]
        public async void RefreshToken_ReturnsSuccess()
        {
            _userService.Setup(s => s.GetUser(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string username, CancellationToken cancellationToken) => new User
                {
                    Id = 1,
                    Email = username,
                    UserName = username,
                    IsActive = true
                });
            _userService.Setup(s => s.GetUserById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => new User
                {
                    Id = id,
                    Email = "test@example.com",
                    UserName = "test@example.com",
                    IsActive = true
                });
            _userService.Setup(s => s.GetUserRole(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(UserRole.Administrator);
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

            _configuration.SetupGet(x => x["Security:Tokens:Key"]).Returns("key12345678910abcdefghijklmnopqrstuvwxyz");
            _configuration.SetupGet(x => x["Security:Tokens:Issuer"]).Returns("issuer");
            _configuration.SetupGet(x => x["Security:Tokens:Audience"]).Returns("audience");

            var httpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new[]
                {
                    new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "1")
                    })
                })
            };

            var controller = new TokenController(_userService.Object, _projectService.Object, _catapultEngineService.Object, _configuration.Object, _applicationSetting, _logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };
            
            var result = await controller.RefreshToken();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<string>(okResult.Value);
            Assert.NotEmpty(returnValue);
        }

        [Fact]
        public async void RefreshToken_ReturnsUserIsSuspended()
        {
            _userService.Setup(s => s.GetUserById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => new User
                {
                    Id = id,
                    Email = "test@example.com",
                    UserName = "test@example.com",
                    IsActive = false
                });

            var httpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new[]
                {
                    new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "1")
                    })
                })
            };

            var controller = new TokenController(_userService.Object, _projectService.Object, _catapultEngineService.Object, _configuration.Object, _applicationSetting, _logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };

            var result = await controller.RefreshToken();

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User is suspended", badRequestResult.Value);
        }

        [Fact]
        public async void RequestEngineToken_ReturnsSuccess()
        {
            _catapultEngineService.Setup(s => s.GetCatapultEngine(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => new CatapultEngine
                {
                    Id = id,
                    Name = "Engine01",
                    IsActive = true
                });
            _catapultEngineService.Setup(s => s.GetCatapultEngineRole(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(UserRole.Engine);

            _configuration.SetupGet(x => x["Security:Tokens:Key"]).Returns("key12345678910abcdefghijklmnopqrstuvwxyz");
            _configuration.SetupGet(x => x["Security:Tokens:Issuer"]).Returns("issuer");
            _configuration.SetupGet(x => x["Security:Tokens:Audience"]).Returns("audience");


            var controller = new TokenController(_userService.Object, _projectService.Object, _catapultEngineService.Object, _configuration.Object, _applicationSetting, _logger.Object);

            var dto = new RequestEngineTokenDto();

            var result = await controller.RequestEngineToken(1, dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<string>(okResult.Value);
            Assert.NotEmpty(returnValue);
        }

        [Fact]
        public async void RequestEngineToken_ReturnsEngineIsSuspended()
        {
            _catapultEngineService.Setup(s => s.GetCatapultEngine(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => new CatapultEngine
                {
                    Id = id,
                    Name = "Engine01", 
                    IsActive = false
                });

            var controller = new TokenController(_userService.Object, _projectService.Object, _catapultEngineService.Object, _configuration.Object, _applicationSetting, _logger.Object);

            var dto = new RequestEngineTokenDto();

            var result = await controller.RequestEngineToken(1, dto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Engine is suspended", badRequestResult.Value);
        }
    }
}
