// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Specifications;

namespace Polyrific.Catapult.Api.Core.Services
{
    public class ExternalServiceTypeService : IExternalServiceTypeService
    {
        private readonly IExternalServiceTypeRepository _externalServiceTypeRepository;

        public ExternalServiceTypeService(IExternalServiceTypeRepository externalServiceTypeRepository)
        {
            _externalServiceTypeRepository = externalServiceTypeRepository;
        }

        public async Task<ExternalServiceType> GetExternalServiceType(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var spec = new ExternalServiceTypeFilterSpecification(id);
            spec.Includes.Add(x => x.ExternalServiceProperties);
            return await _externalServiceTypeRepository.GetSingleBySpec(spec, cancellationToken);
        }

        public async Task<ExternalServiceType> GetExternalServiceTypeByName(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var spec = new ExternalServiceTypeFilterSpecification(name);
            spec.Includes.Add(x => x.ExternalServiceProperties);
            return await _externalServiceTypeRepository.GetSingleBySpec(spec, cancellationToken);
        }

        public async Task<List<ExternalServiceType>> GetExternalServiceTypes(bool includeProperties = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            var getAllSpec = new ExternalServiceTypeFilterSpecification();
            if (includeProperties)
                getAllSpec.Includes.Add(s => s.ExternalServiceProperties);

            return (await _externalServiceTypeRepository.GetBySpec(getAllSpec, cancellationToken)).ToList();
        }
    }
}
