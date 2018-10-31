// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Data.Identity;
using Polyrific.Catapult.Shared.Dto.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Data
{
    public class CatapultEngineRepository : ICatapultEngineRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<CatapultEngineProfile> _profileRepository;
        private readonly IMapper _mapper;

        public CatapultEngineRepository(UserManager<ApplicationUser> userManager, IRepository<CatapultEngineProfile> profileRepository, IMapper mapper)
        {
            _userManager = userManager;
            _profileRepository = profileRepository;
            _mapper = mapper;
        }

        public async Task ConfirmRegistration(int userId, string token, CancellationToken cancellationToken = default(CancellationToken))
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

        public Task<int> CountBySpec(ISpecification<CatapultEngine> spec, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
        
        public async Task<int> Create(CatapultEngine entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            entity.Created = DateTime.UtcNow;
            var user = _mapper.Map<ApplicationUser>(entity);
            user.Email = $"{user.UserName}@opencatapult.net"; // dummy email to avoid error

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                result.ThrowErrorException();
            
            await _userManager.AddToRoleAsync(user, UserRole.Engine);

            var profile = _mapper.Map<CatapultEngineProfile>(entity);
            profile.CatapultEngine = user;
            await _profileRepository.Create(profile, cancellationToken);

            return user.Id;
        }

        public async Task Delete(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.Users.Include(u => u.CatapultEngineProfile).FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user != null)
            {
                if (user.CatapultEngineProfile != null)
                    await _profileRepository.Delete(user.CatapultEngineProfile.Id, cancellationToken);

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

        public async Task<CatapultEngine> GetById(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.Users.Include(u => u.CatapultEngineProfile).FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user != null)
                return _mapper.Map<CatapultEngine>(user);

            return await Task.FromResult((CatapultEngine)null);
        }

        public async Task<CatapultEngine> GetByPrincipal(ClaimsPrincipal principal, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var appCatapultEngine = await _userManager.GetUserAsync(principal);

            return _mapper.Map<CatapultEngine>(appCatapultEngine);
        }

        public Task<IEnumerable<CatapultEngine>> GetBySpec(ISpecification<CatapultEngine> spec, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<CatapultEngine> GetSingleBySpec(ISpecification<CatapultEngine> spec, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<CatapultEngine> GetByCatapultEngineName(string engineName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var appCatapultEngine = await _userManager.Users.Include(u => u.CatapultEngineProfile).FirstOrDefaultAsync(u => u.UserName == engineName, cancellationToken);

            return _mapper.Map<CatapultEngine>(appCatapultEngine);
        }

        public async Task Update(CatapultEngine entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.Users.Include(u => u.CatapultEngineProfile).FirstOrDefaultAsync(u => u.Id == entity.Id, cancellationToken);
            if (user != null && user.CatapultEngineProfile != null)
            {
                _mapper.Map(entity, user.CatapultEngineProfile);
                user.CatapultEngineProfile.Updated = DateTime.UtcNow;
                user.CatapultEngineProfile.ConcurrencyStamp = Guid.NewGuid().ToString();
                await _profileRepository.Update(user.CatapultEngineProfile, cancellationToken);
            }
        }

        public async Task<bool> ValidateCatapultEnginePassword(string userName, string password, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
                return await _userManager.CheckPasswordAsync(user, password);

            return false;
        }

        public async Task<List<CatapultEngine>> GetAll(bool? isActive, DateTime? minLastSeen, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var catapultEngines = await _userManager.Users.Include(u => u.CatapultEngineProfile)
                .Where(e => (isActive == null || e.CatapultEngineProfile.IsActive == isActive) &&
                (minLastSeen == null || e.CatapultEngineProfile.LastSeen > minLastSeen)).ToListAsync(cancellationToken);
            return _mapper.Map<List<CatapultEngine>>(catapultEngines.Where(e => e.IsCatapultEngine ?? false));
        }

        public async Task Suspend(int catapultEngineId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var engine = await _userManager.Users.Include(u => u.CatapultEngineProfile).FirstOrDefaultAsync(u => u.Id == catapultEngineId);
            engine.CatapultEngineProfile.IsActive = false;

            await _profileRepository.Update(engine.CatapultEngineProfile, cancellationToken);
        }

        public async Task Reactivate(int catapultEngineId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var engine = await _userManager.Users.Include(u => u.CatapultEngineProfile).FirstOrDefaultAsync(u => u.Id == catapultEngineId);
            engine.CatapultEngineProfile.IsActive = true;

            await _profileRepository.Update(engine.CatapultEngineProfile, cancellationToken);
        }

        public async Task<string> GetCatapultEngineRole(int catapultEngineId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(catapultEngineId.ToString());
            if (user == null)
                return "";

            var roles = await _userManager.GetRolesAsync(user);
            return roles.Any() ? roles.First() : "";
        }
    }
}
