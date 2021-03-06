﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Data.Identity;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IMapper _mapper;

        public UserRepository(UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager, 
            SignInManager<ApplicationUser> signInManager, 
            IRepository<UserProfile> userProfileRepository, 
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userProfileRepository = userProfileRepository;
            _mapper = mapper;
        }

        public async Task ConfirmEmail(int userId, string token, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (!result.Succeeded)
                    result.ThrowErrorException();
            }
        }

        public Task<int> CountBySpec(ISpecification<User> spec, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> Create(User entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = _mapper.Map<ApplicationUser>(entity);
            
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                result.ThrowErrorException();

            var userProfile = _mapper.Map<UserProfile>(entity);
            userProfile.ApplicationUser = user;
            userProfile.IsActive = true;
            await _userProfileRepository.Create(userProfile, cancellationToken);

            return user.Id;
        }

        public async Task<int> Create(User entity, string password, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = _mapper.Map<ApplicationUser>(entity);
            
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                result.ThrowErrorException();

            var userProfile = _mapper.Map<UserProfile>(entity);
            userProfile.ApplicationUser = user;
            userProfile.IsActive = true;
            await _userProfileRepository.Create(userProfile, cancellationToken);

            return user.Id;
        }

        public async Task Delete(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.Users.Include(u => u.UserProfile).FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user != null)
            {
                if (user.UserProfile != null)
                    await _userProfileRepository.Delete(user.UserProfile.Id, cancellationToken);

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                    result.ThrowErrorException();
            }
        }

        public async Task<string> GenerateConfirmationToken(int userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
                return await _userManager.GenerateEmailConfirmationTokenAsync(user);

            return "";
        }
        
        public async Task<User> GetById(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.Users.Include(u => u.UserProfile).Include("Roles.Role").FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user != null)
                return _mapper.Map<User>(user);

            return await Task.FromResult((User) null);
        }

        public async Task<User> GetByPrincipal(ClaimsPrincipal principal, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var appUser = await _userManager.FindByNameAsync(principal.Identity.Name);

            return _mapper.Map<User>(appUser);
        }

        public Task<IEnumerable<User>> GetBySpec(ISpecification<User> spec, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public Task<User> GetSingleBySpec(ISpecification<User> spec, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> GetUser(string userName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var appUser = await _userManager.Users.Include(u => u.UserProfile).Include("Roles.Role").FirstOrDefaultAsync(u => u.UserName == userName || u.Email == userName);

            return _mapper.Map<User>(appUser);
        }

        public async Task Update(User entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var user = await _userManager.Users.Include(u => u.UserProfile).FirstOrDefaultAsync(u => u.Id == entity.Id);
                if (user != null)
                {
                    // update username
                    if (user.UserName.ToLower() != entity.UserName?.ToLower())
                    {
                        var userWithExistingEmail = await _userManager.FindByEmailAsync(entity.UserName);
                        if (userWithExistingEmail != null && userWithExistingEmail.Id != user.Id)
                            throw new DuplicateUserNameException(entity.UserName);

                        user.UserName = entity.UserName;

                        await _userManager.UpdateNormalizedUserNameAsync(user);
                        await _userManager.UpdateAsync(user);
                    }

                    // update userprofile
                    if (user.UserProfile != null)
                    {
                        user.UserProfile.FirstName = entity.FirstName;
                        user.UserProfile.LastName = entity.LastName;
                        user.UserProfile.ExternalAccountIds = entity.ExternalAccountIds != null ? JsonConvert.SerializeObject(entity.ExternalAccountIds) : null;
                        await _userProfileRepository.Update(user.UserProfile, cancellationToken);
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException && ex.InnerException.Message.StartsWith("Cannot insert duplicate key row"))
                {
                    throw new DuplicateUserNameException(entity.UserName);
                }

                throw;
            }
        }

        public async Task UpdateAvatar(int userId, int? managedFileId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.Users.Include(u => u.UserProfile).FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null && user.UserProfile != null)
            {
                user.UserProfile.AvatarFileId = managedFileId;
                await _userProfileRepository.Update(user.UserProfile, cancellationToken);
            }
        }

        public async Task<Core.Entities.SignInResult> ValidateUserPassword(string userName, string password, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByNameAsync(userName);
            if (user != null && user.EmailConfirmed)
            {
                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
                return _mapper.Map<Core.Entities.SignInResult>(result);
            }

            return new Core.Entities.SignInResult
            {
                Succeeded = false
            };
        }

        public async Task SetUserRole(int userId, string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null)
                    throw new InvalidRoleException(roleName);

                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains(roleName))
                {
                    var result = await _userManager.RemoveFromRolesAsync(user, roles);
                    if (!result.Succeeded)
                        result.ThrowErrorException();

                    result = await _userManager.AddToRoleAsync(user, roleName);
                    if (!result.Succeeded)
                        result.ThrowErrorException();
                }
            }
        }

        public async Task<string> GetUserRole(string userName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) 
                return "";

            var roles = await _userManager.GetRolesAsync(user);
            return roles.Any() ? roles.First() : "";
        }

        public async Task<List<User>> GetUsers(bool? isActive, string role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var users = await _userManager.Users.Include(u => u.UserProfile).Include("Roles.Role")
                .Where(u => u.UserProfile != null && (u.UserProfile.IsActive == isActive || isActive == null) &&
                    (role == null || u.Roles.Any(r => r.Role.Name == role))).ToListAsync();

            return _mapper.Map<List<User>>(users);
        }

        public async Task Suspend(int userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.Users.Include(u => u.UserProfile).FirstOrDefaultAsync(u => u.Id == userId);
            user.UserProfile.IsActive = false;

            await _userProfileRepository.Update(user.UserProfile, cancellationToken);
        }

        public async Task Reactivate(int userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.Users.Include(u => u.UserProfile).FirstOrDefaultAsync(u => u.Id == userId);
            user.UserProfile.IsActive = true;

            await _userProfileRepository.Update(user.UserProfile, cancellationToken);
        }

        public async Task<string> GetResetPasswordToken(int userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task ResetPassword(int userId, string token, string newPassword, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
                result.ThrowErrorException();
        }

        public async Task UpdatePassword(int userId, string oldPassword, string newPassword, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!result.Succeeded)
                result.ThrowErrorException();
        }

        public async Task<List<User>> GetUsersByIds(int[] ids, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var users = await _userManager.Users.Include(u => u.UserProfile).Where(u => ids.Contains(u.Id)).ToListAsync(cancellationToken);

            return _mapper.Map<List<User>>(users);
        }

        public async Task<string> GetAuthenticatorKey(int userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(userId.ToString());
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            return unformattedKey;
        }

        public async Task ResetAuthenticatorKey(int userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(userId.ToString());
            await _userManager.SetTwoFactorEnabledAsync(user, false);
            await _userManager.ResetAuthenticatorKeyAsync(user);
        }

        public async Task DisableTwoFactor(int userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(userId.ToString());
            await _userManager.SetTwoFactorEnabledAsync(user, false);
        }

        public async Task<bool> VerifyTwoFactorToken(string userName, string verificationCode, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByNameAsync(userName);
            var result = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (result)
                await _userManager.SetTwoFactorEnabledAsync(user, true);

            return result;
        }

        public async Task<bool> RedeemTwoFactorRecoveryCode(string userName, string recoveryCode, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByNameAsync(userName);
            var result = await _userManager.RedeemTwoFactorRecoveryCodeAsync(user, recoveryCode);

            return result.Succeeded;
        }

        public async Task<User2faInfo> GetUser2faInfo(int userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(userId.ToString());

            var result = new User2faInfo();
            result.Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            result.RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user);
            result.HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null;

            return result;
        }

        public async Task<string[]> GenerateNewTwoFactorRecoveryCodes(int userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(userId.ToString());
            return (await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10)).ToArray();
        }


    }
}
