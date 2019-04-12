// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Specifications;

namespace Polyrific.Catapult.Api.Core.Services
{
    public class ProviderAdditionalConfigService : IProviderAdditionalConfigService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IProviderAdditionalConfigRepository _providerAdditionalConfigRepository;

        public ProviderAdditionalConfigService(IProviderRepository providerRepository, IProviderAdditionalConfigRepository providerAdditionalConfigRepository)
        {
            _providerRepository = providerRepository;
            _providerAdditionalConfigRepository = providerAdditionalConfigRepository;
        }

        public async Task<List<int>> AddAdditionalConfigs(int providerId, List<ProviderAdditionalConfig> additionalConfigs, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var provider = await _providerRepository.GetById(providerId, cancellationToken);
            if (provider == null)
            {
                throw new ProviderNotFoundException(providerId);
            }
            
            additionalConfigs.ForEach(j => j.ProviderId = providerId);

            return await _providerAdditionalConfigRepository.AddRange(additionalConfigs, cancellationToken);
        }

        public async Task<List<ProviderAdditionalConfig>> GetByProvider(int providerId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var spec = new ProviderAdditionalConfigFilterSpecification(providerId);
            var result = await _providerAdditionalConfigRepository.GetBySpec(spec, cancellationToken);

            return result.ToList();
        }

        public async Task<List<ProviderAdditionalConfig>> GetByProviderName(string providerName, CancellationToken cancellationToken = default(CancellationToken))
        {
            var spec = new ProviderAdditionalConfigFilterSpecification(providerName);
            var result = await _providerAdditionalConfigRepository.GetBySpec(spec, cancellationToken);

            return result.ToList();
        }
    }
}
