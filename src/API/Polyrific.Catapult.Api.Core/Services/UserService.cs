// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Shared.Dto.Constants;

namespace Polyrific.Catapult.Api.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private static char[] punctuations = "!@#$%^&*()_-+=[{]};:>|./?".ToCharArray();

        private static char[] startingChars = new char[] { '<', '&' };

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

        public Task<string> GeneratePassword(int length = 10)
        {
            if (length < 1 || length > 128)
            {
                throw new ArgumentException("Length should be betwen 1 and 128");
            }
            
            string password;
            char[] cBuf;

            do
            {
                cBuf = GetRandomLowerCaseString(length);

                // make sure password contains digit
                var rand = new Random();
                int digitIdx = rand.Next(0, length - 1);
                cBuf[digitIdx] = (char)('0' + rand.Next(0, 9));

                // make sure password contain upperCase
                int ucLetterIdx;
                do
                {
                    ucLetterIdx = rand.Next(0, length - 1);
                } while (ucLetterIdx == digitIdx);
                cBuf[ucLetterIdx] = (char)('A' + rand.Next(0, 25));

                // make sure password contain nonalphanumeric
                int nonAlphanumericIdx;
                do
                {
                    nonAlphanumericIdx = rand.Next(0, length - 1);
                } while (nonAlphanumericIdx == digitIdx || nonAlphanumericIdx == ucLetterIdx);
                cBuf[nonAlphanumericIdx] = punctuations[rand.Next(0, punctuations.Length)];

                password = new string(cBuf);
            }
            while (IsDangerousString(password, out var index));

            return Task.FromResult(password);
        }

        private static bool IsDangerousString(string s, out int matchIndex)
        {
            //bool inComment = false;
            matchIndex = 0;

            for (int i = 0; ;)
            {

                // Look for the start of one of our patterns
                int n = s.IndexOfAny(startingChars, i);

                // If not found, the string is safe
                if (n < 0) return false;

                // If it's the last char, it's safe
                if (n == s.Length - 1) return false;

                matchIndex = n;

                switch (s[n])
                {
                    case '<':
                        // If the < is followed by a letter or '!', it's unsafe (looks like a tag or HTML comment)
                        if (IsAtoZ(s[n + 1]) || s[n + 1] == '!' || s[n + 1] == '/' || s[n + 1] == '?') return true;
                        break;
                    case '&':
                        // If the & is followed by a #, it's unsafe (e.g. &#83;)
                        if (s[n + 1] == '#') return true;
                        break;
                }

                // Continue searching
                i = n + 1;
            }
        }

        private static bool IsAtoZ(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        private static char[] GetRandomLowerCaseString(int length)
        {
            var chars = "abcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            return Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray();
        }
    }
}
