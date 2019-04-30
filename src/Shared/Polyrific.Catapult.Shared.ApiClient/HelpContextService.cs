// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.HelpContext;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Shared.ApiClient
{
    public class HelpContextService : BaseService, IHelpContextService
    {
        public HelpContextService(IApiClient api) : base(api)
        {
        }

        public async Task<List<HelpContextDto>> GetHelpContextsBySection(string section)
        {
            var path = $"help-context/section/{section}";

            return await Api.Get<List<HelpContextDto>>(path);
        }
    }
}
