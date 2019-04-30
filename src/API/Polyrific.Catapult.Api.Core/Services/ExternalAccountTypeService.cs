// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Specifications;

namespace Polyrific.Catapult.Api.Core.Services
{
    public class ExternalAccountTypeService : IExternalAccountTypeService
    {
        private readonly IExternalAccountTypeRepository _externalAccountTypeRepository;

        public ExternalAccountTypeService(IExternalAccountTypeRepository externalAccountTypeRepository)
        {
            _externalAccountTypeRepository = externalAccountTypeRepository;
        }

        public async Task<List<ExternalAccountType>> GetExternalAccountTypes()
        {
            var spec = new ExternalAccountTypeFilterSpecification();
            var result = await _externalAccountTypeRepository.GetBySpec(spec);

            return result.ToList();
        }
    }
}
