// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Shared.Dto.Constants;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task ConfirmEmail(int userId, string token, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _userRepository.ConfirmEmail(userId, token, cancellationToken);
        }

        public async Task<User> CreateUser(string email, string firstName, string lastName, string password, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = new User
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };

            try
            {
                var id = await _userRepository.Create(user, password, cancellationToken);
                if (id > 0)
                    user.Id = id;
            }
            catch (Exception ex)
            {
                throw new UserCreationFailedException(user.UserName, ex);
            }

            return user;
        }

        public async Task DeleteUser(int userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await _userRepository.Delete(userId, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new UserDeletionFailedException(userId, ex);
            }
        }

        public Task<string> GenerateConfirmationToken(int userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return _userRepository.GenerateConfirmationToken(userId, cancellationToken);
        }

        public async Task<User> GetUser(string userName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _userRepository.GetByUserName(userName, cancellationToken);
        }

        public async Task<User> GetUserById(int userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _userRepository.GetById(userId, cancellationToken);
        }

        public async Task<User> GetUserByEmail(string email, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _userRepository.GetByEmail(email, cancellationToken);
        }

        public async Task<int> GetUserId(ClaimsPrincipal principal, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userRepository.GetByPrincipal(principal, cancellationToken);

            return user?.Id ?? 0;
        }

        public async Task<string> GetUserEmail(ClaimsPrincipal principal, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userRepository.GetByPrincipal(principal, cancellationToken);

            return user?.Email;
        }

        public async Task<string> GetUserRole(string userName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _userRepository.GetUserRole(userName, cancellationToken);
        }

        public async Task<List<User>> GetUsers(string status = null, string role = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            bool? isActive;

            switch (status?.ToLower())
            {
                case UserStatus.Active:
                    isActive = true;
                    break;
                case UserStatus.Suspended:
                    isActive = false;
                    break;
                case "":
                case null:
                case UserStatus.All:
                    isActive = null;
                    break;
                default:
                    throw new UserStatusNotFoundException(status);
            }

            if (role == UserRole.All)
                role = null;

            return await _userRepository.GetUsers(isActive, role);
        }

        public async Task SetUserRole(string userId, string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _userRepository.SetUserRole(userId, roleName, cancellationToken);
        }

        public async Task Suspend(int userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _userRepository.Suspend(userId, cancellationToken);
        }

        public async Task Reactivate(int userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _userRepository.Reactivate(userId, cancellationToken);
        }

        public async Task ResetPassword(int userId, string token, string newPassword, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _userRepository.ResetPassword(userId, token, newPassword, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new ResetPasswordFailedException(userId, ex);
            }
        }

        public async Task<string> GetResetPasswordToken(int userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _userRepository.GetResetPasswordToken(userId, cancellationToken);
        }

        public async Task UpdatePassword(int userId, string oldPassword, string newPassword, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _userRepository.UpdatePassword(userId, oldPassword, newPassword, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new UpdatePasswordFailedException(userId, ex);
            }
        }

        public async Task UpdateUser(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _userRepository.Update(user, cancellationToken);
        }

        public async Task<bool> ValidateUserPassword(string userName, string password, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _userRepository.ValidateUserPassword(userName, password, cancellationToken);
        }
    }
}
