// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Specifications;

namespace Polyrific.Catapult.Api.Core.Services
{
    public class HelpContextService : IHelpContextService
    {
        private readonly IHelpContextRepository _helpContextRepository;

        public HelpContextService(IHelpContextRepository helpContextRepository)
        {
            _helpContextRepository = helpContextRepository;
        }

        public async Task<List<HelpContext>> GetHelpContextsBySection(string section)
        {
            var spec = new HelpContextFilterSpecification(section);
            var result = await _helpContextRepository.GetBySpec(spec);

            return result.ToList();
        }
    }
}
