// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.Identity;
using Moq;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Shared.Dto.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Core.Services
{
    public class UserServiceTests
    {
        private readonly List<User> _data;
        private readonly Mock<IUserRepository> _UserRepository;

        public UserServiceTests()
        {
            _data = new List<User>
            {
                new User
                {
                    Id = 1,
                    UserName = "test",
                    Email = "test@test.com",
                    FirstName = "first",
                    LastName = "last"
                }
            };

            _UserRepository = new Mock<IUserRepository>();
            _UserRepository.Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Callback((int id, CancellationToken cancellationToken) =>
                {
                    var entity = _data.FirstOrDefault(d => d.Id == id);
                    if (entity != null)
                        _data.Remove(entity);
                });
            _UserRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => _data.FirstOrDefault(d => d.Id == id));
            _UserRepository.Setup(r => r.GetByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string email, CancellationToken cancellationToken) => _data.FirstOrDefault(d => d.Email == email));
            _UserRepository.Setup(r => r.GetByUserName(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string name, CancellationToken cancellationToken) => _data.FirstOrDefault(d => d.UserName == name));
            _UserRepository.Setup(r => r.GetUserRole(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(UserRole.Administrator);
            _UserRepository.Setup(r => r.GetUsers(It.IsAny<bool?>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_data);
            _UserRepository.Setup(r => r.GetResetPasswordToken(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("test");
            _UserRepository.Setup(r => r.GetByPrincipal(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ClaimsPrincipal principal, CancellationToken cancellationToken) => _data.FirstOrDefault());
            _UserRepository.Setup(r => r.GenerateConfirmationToken(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("test");
            _UserRepository.Setup(r => r.ValidateUserPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _UserRepository.Setup(r => r.Create(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(2).Callback((User entity, CancellationToken cancellationToken) =>
                {
                    entity.Id = 2;
                    _data.Add(entity);
                });
            _UserRepository
                .Setup(r => r.Create(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(2).Callback(
                    (User entity, string password, CancellationToken cancellationToken) =>
                    {
                        entity.Id = 2;
                        _data.Add(entity);
                    });
            _UserRepository.Setup(r => r.Update(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Callback((User entity, CancellationToken cancellationToken) =>
                {
                    var oldEntity = _data.FirstOrDefault(d => d.Id == entity.Id);
                    if (oldEntity != null)
                    {
                        _data.Remove(oldEntity);
                        _data.Add(entity);
                    }
                });
        }

        [Fact]
        public async void AddUser_ValidItem()
        {
            var UserService = new UserService(_UserRepository.Object);
            var newUser = await UserService.CreateUser("test2@test.com", "first test", "last test", "test*1");

            Assert.NotNull(newUser);
            Assert.True(_data.Count > 1);
        }
        
        [Fact]
        public async void GetUsers_ReturnItems()
        {
            var UserService = new UserService(_UserRepository.Object);
            var Users = await UserService.GetUsers(null);

            Assert.NotEmpty(Users);
        }

        [Fact]
        public async void GetUsers_ReturnsEmpty()
        {
            _data.Clear();

            var UserService = new UserService(_UserRepository.Object);
            var Users = await UserService.GetUsers(UserStatus.Active);

            Assert.Empty(Users);
        }

        [Fact]
        public void GetUsers_UserStatusNotFoundException()
        {
            var UserService = new UserService(_UserRepository.Object);
            var exception = Record.ExceptionAsync(() => UserService.GetUsers("test"));

            Assert.IsType<UserStatusNotFoundException>(exception?.Result);
        }

        [Fact]
        public async void GetUser_ReturnItem()
        {            
            var UserService = new UserService(_UserRepository.Object);
            var projectUser = await UserService.GetUser("test");

            Assert.NotNull(projectUser);
            Assert.Equal(1, projectUser.Id);
        }

        [Fact]
        public async void GetUser_ReturnNull()
        {
            var UserService = new UserService(_UserRepository.Object);
            var User = await UserService.GetUser("testempty");

            Assert.Null(User);
        }

        [Fact]
        public async void GetUserByEmail_ReturnItem()
        {
            var userService = new UserService(_UserRepository.Object);
            var projectUser = await userService.GetUserByEmail("test@test.com");

            Assert.NotNull(projectUser);
            Assert.Equal(1, projectUser.Id);
        }

        [Fact]
        public async void GetUserByEmail_ReturnNull()
        {
            var userService = new UserService(_UserRepository.Object);
            var user = await userService.GetUserByEmail("test@empty.com");

            Assert.Null(user);
        }

        [Fact]
        public async void DeleteUser_ValidItem()
        {
            var UserService = new UserService(_UserRepository.Object);
            await UserService.DeleteUser(1);

            Assert.Empty(_data);
        }

        [Fact]
        public async void GenerateConfirmationToken_ReturnToken()
        {
            var UserService = new UserService(_UserRepository.Object);
            var token = await UserService.GenerateConfirmationToken(1);

            Assert.NotNull(token);
        }

        [Fact]
        public async void ConfirmRegistration_Success()
        {
            var UserService = new UserService(_UserRepository.Object);
            await UserService.ConfirmEmail(1, "test");

            _UserRepository.Verify(r => r.ConfirmEmail(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async void Suspend_Success()
        {
            var UserService = new UserService(_UserRepository.Object);
            await UserService.Suspend(1);

            _UserRepository.Verify(r => r.Suspend(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async void Reactivate_Success()
        {
            var UserService = new UserService(_UserRepository.Object);
            await UserService.Reactivate(1);

            _UserRepository.Verify(r => r.Reactivate(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async void GetUserId_ReturnId()
        {
            var UserService = new UserService(_UserRepository.Object);
            var id = await UserService.GetUserId(new ClaimsPrincipal());

            Assert.NotEqual(0, id);
        }

        [Fact]
        public async void GetUserEmail_ReturnUserEmail()
        {
            var userService = new UserService(_UserRepository.Object);
            var email = await userService.GetUserEmail(new ClaimsPrincipal());

            Assert.NotEqual(string.Empty, email);
        }

        [Fact]
        public async void ValidateUserPassword_ReturnValid()
        {
            var UserService = new UserService(_UserRepository.Object);
            var result = await UserService.ValidateUserPassword("test", "test");

            Assert.True(result);
        }

        [Fact]
        public async void GetUserRole_ReturnAdminRole()
        {
            var UserService = new UserService(_UserRepository.Object);
            var result = await UserService.GetUserRole("test");

            Assert.Equal(UserRole.Administrator, result);
        }

        [Fact]
        public async void SetUserRole_Success()
        {
            var UserService = new UserService(_UserRepository.Object);
            await UserService.SetUserRole("test", UserRole.Basic);

            _UserRepository.Verify(r => r.SetUserRole(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async void ResetPassword_Success()
        {
            var UserService = new UserService(_UserRepository.Object);
            await UserService.ResetPassword(1, "test", "test");

            _UserRepository.Verify(r => r.ResetPassword(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async void UpdatePassword_Success()
        {
            var UserService = new UserService(_UserRepository.Object);
            await UserService.UpdatePassword(1, "test", "test");

            _UserRepository.Verify(r => r.UpdatePassword(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async void GetResetPasswordToken_ReturnToken()
        {
            var UserService = new UserService(_UserRepository.Object);
            var token = await UserService.GetResetPasswordToken(1);
            
            Assert.NotNull(token);
        }

        [Fact]
        public async void GeneratePassword_SatisfyRequirement()
        {
            var UserService = new UserService(_UserRepository.Object);
            var passwordValidator = new PasswordValidator<User>();
            var userManager = GetMockUserManager().Object;
            var user = new User();

            // since the password is generated randomly, it'd be better to assert it several time
            for (var i = 0; i < 50; i++)
            {
                var password = await UserService.GeneratePassword();
                var result = await passwordValidator.ValidateAsync(userManager, user, password);
                Assert.True(result.Succeeded);
            }
        }

        private Mock<UserManager<User>> GetMockUserManager()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            return new Mock<UserManager<User>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
        }
    }
}
