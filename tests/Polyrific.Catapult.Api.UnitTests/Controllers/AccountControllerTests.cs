// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Api.AutoMapperProfiles;
using Polyrific.Catapult.Api.Controllers;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.UnitTests.Utilities;
using Polyrific.Catapult.Shared.Common.Notification;
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

            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<UserAutoMapperProfile>();
            });
            _mapper = Mapper.Instance;
            
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
            Assert.IsType<RegisterUserResultDto>(okActionResult.Value);
            _notificationProvider.Verify(n => n.SendNotification(It.IsAny<SendNotificationRequest>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }
    }
}
