// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Data.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IMapper _mapper;

        public UserRepository(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IRepository<UserProfile> userProfileRepository, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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

            var user = await _userManager.Users.Include(u => u.UserProfile).FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user != null)
                return _mapper.Map<User>(user);

            return await Task.FromResult((User) null);
        }

        public async Task<User> GetByEmail(string email, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.Users.Include(u => u.UserProfile).FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
            if (user != null)
                return _mapper.Map<User>(user);

            return await Task.FromResult((User)null);
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

        public async Task<User> GetByUserName(string userName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var appUser = await _userManager.Users.Include(u => u.UserProfile).FirstOrDefaultAsync(u => u.UserName == userName);

            return _mapper.Map<User>(appUser);
        }

        public async Task Update(User entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.Users.Include(u => u.UserProfile).FirstOrDefaultAsync(u => u.Id == entity.Id);
            if (user != null && user.UserProfile != null)
            {
                user.UserProfile.FirstName = entity.FirstName;
                user.UserProfile.LastName = entity.LastName;
                await _userProfileRepository.Update(user.UserProfile, cancellationToken);
            }
        }

        public async Task<bool> ValidateUserPassword(string userName, string password, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByNameAsync(userName);
            if (user != null && user.EmailConfirmed)
                return await _userManager.CheckPasswordAsync(user, password);

            return false;
        }

        public async Task SetUserRole(string userId, string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(userId);
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

            var users = await _userManager.Users.Include(u => u.UserProfile).Where(u => u.UserProfile.IsActive == isActive || isActive == null).ToListAsync();
            if (!string.IsNullOrEmpty(role))
                users = users.Where(u => _userManager.IsInRoleAsync(u, role).Result).ToList();

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
    }
}
