// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Security;
using Polyrific.Catapult.Api.Core.Specifications;

namespace Polyrific.Catapult.Api.Core.Services
{
    public class ExternalServiceService : IExternalServiceService
    {
        private readonly IExternalServiceRepository _repository;
        private readonly ISecretVault _secretVault;

        public ExternalServiceService(IExternalServiceRepository repository, ISecretVault secretVault)
        {
            _repository = repository;
            _secretVault = secretVault;
        }

        public async Task<int> AddExternalService(string name, string description, int typeId, string configString, int userId, bool isGlobal, CancellationToken cancellationToken = default(CancellationToken))
        {
            var externalServiceByNameSpec = new ExternalServiceFilterSpecification(0, name);
            if (await _repository.CountBySpec(externalServiceByNameSpec, cancellationToken) > 0)
            {
                throw new DuplicateExternalServiceException(name);
            }

            await _secretVault.Add(name, configString, cancellationToken);

            var newExternalService = new ExternalService
            {
                Name = name,
                Description = description,
                ExternalServiceTypeId = typeId,
                UserId = userId,
                IsGlobal = isGlobal
            };

            return await _repository.Create(newExternalService, cancellationToken);
        }

        public async Task DeleteExternalService(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var externalService = await _repository.GetById(id);

            if (externalService != null)
            {
                await _repository.Delete(id);
                await _secretVault.Delete(externalService.Name, cancellationToken);
            }
        }

        public async Task<ExternalService> GetExternalService(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var externalServiceByNameSpec = new ExternalServiceFilterSpecification(id, null);
            externalServiceByNameSpec.Includes.Add(x => x.ExternalServiceType);
            var externalService = await _repository.GetSingleBySpec(externalServiceByNameSpec, cancellationToken);

            if (externalService != null)
                externalService.ConfigString = await _secretVault.Get(externalService.Name, cancellationToken);

            return externalService;
        }

        public async Task<ExternalService> GetExternalServiceByName(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var externalServiceByNameSpec = new ExternalServiceFilterSpecification(0, name);
            externalServiceByNameSpec.Includes.Add(x => x.ExternalServiceType);
            var externalService = await _repository.GetSingleBySpec(externalServiceByNameSpec, cancellationToken);

            if (externalService != null)
                externalService.ConfigString = await _secretVault.Get(externalService.Name, cancellationToken);

            return externalService;
        }

        public async Task<List<ExternalService>> GetExternalServices(int userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var externalServiceByUserSpec = new ExternalServiceFilterSpecification(userId);
            externalServiceByUserSpec.Includes.Add(x => x.ExternalServiceType);
            var result = await _repository.GetBySpec(externalServiceByUserSpec, cancellationToken);

            return result.ToList();
        }

        public async Task UpdateExternalService(ExternalService externalService, CancellationToken cancellationToken = default(CancellationToken))
        {
            var entity = await _repository.GetById(externalService.Id);

            if (entity != null)
            {
                entity.Description = externalService.Description;
                entity.IsGlobal = externalService.IsGlobal;

                if (!string.IsNullOrEmpty(externalService.ConfigString))
                {
                    await _secretVault.Update(entity.Name, externalService.ConfigString, cancellationToken);
                }

                await _repository.Update(entity, cancellationToken);
            }
        }
    }
}
