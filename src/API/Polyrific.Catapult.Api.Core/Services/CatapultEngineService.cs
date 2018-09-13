// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Shared.Dto.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Core.Services
{
    public class CatapultEngineService : ICatapultEngineService
    {
        private readonly ICatapultEngineRepository _catapultEngineRepository;

        public CatapultEngineService(ICatapultEngineRepository catapultEngineRepository)
        {
            _catapultEngineRepository = catapultEngineRepository;
        }

        public async Task ConfirmRegistration(int catapultEngineId, string token, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _catapultEngineRepository.ConfirmRegistration(catapultEngineId, token, cancellationToken);
        }

        public async Task<CatapultEngine> CreateCatapultEngine(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var currentEngine = await _catapultEngineRepository.GetByCatapultEngineName(name, cancellationToken);
            if (currentEngine != null)
            {
                throw new DuplicateCatapultEngineException(name);
            }

            var catapultEngine = new CatapultEngine
            {
                Name = name
            };

            try
            {
                var id = await _catapultEngineRepository.Create(catapultEngine, cancellationToken);
                if (id > 0)
                    catapultEngine.Id = id;
            }
            catch (Exception ex)
            {
                throw new CatapultEngineCreationFailedException(name, ex);
            }

            return catapultEngine;
        }

        public async Task DeleteCatapultEngine(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await _catapultEngineRepository.Delete(id, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new CatapultEngineDeletionFailedException(id, ex);
            }
        }

        public async Task<string> GenerateConfirmationToken(int catapultEngineId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _catapultEngineRepository.GenerateConfirmationToken(catapultEngineId, cancellationToken);
        }

        public async Task<CatapultEngine> GetCatapultEngine(int catapultEngineId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _catapultEngineRepository.GetById(catapultEngineId, cancellationToken);
        }

        public async Task<CatapultEngine> GetCatapultEngine(string catapultEngineName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _catapultEngineRepository.GetByCatapultEngineName(catapultEngineName, cancellationToken);
        }

        public async Task<int> GetCatapultEngineId(ClaimsPrincipal principal, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var catapultEngine = await _catapultEngineRepository.GetByPrincipal(principal, cancellationToken);

            return catapultEngine?.Id ?? 0;
        }
        
        public async Task<List<CatapultEngine>> GetCatapultEngines(string status, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            bool? isActive;
            DateTime? minLastSeen = null;
            switch (status)
            {
                case null:
                case "":
                case EngineStatus.All:
                    isActive = null;
                    break;
                case EngineStatus.Active:
                    isActive = true;
                    break;
                case EngineStatus.Running:
                    isActive = null;
                    minLastSeen = DateTime.UtcNow.AddMinutes(-1);
                    break;
                case EngineStatus.Suspended:
                    isActive = false;
                    break;
                default:
                    throw new FilterTypeNotFoundException(status);
            }

            return await _catapultEngineRepository.GetAll(isActive, minLastSeen, cancellationToken);
        }

        public async Task<string> GetCatapultEngineRole(int catapultEngineId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _catapultEngineRepository.GetCatapultEngineRole(catapultEngineId, cancellationToken);
        }

        public async Task Reactivate(int catapultEngineId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _catapultEngineRepository.Reactivate(catapultEngineId, cancellationToken);
        }

        public async Task Suspend(int catapultEngineId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _catapultEngineRepository.Suspend(catapultEngineId, cancellationToken);
        }

        public async Task<bool> ValidateCatapultEnginePassword(string catapultEngineName, string password, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _catapultEngineRepository.ValidateCatapultEnginePassword(catapultEngineName, password, cancellationToken);
        }
    }
}