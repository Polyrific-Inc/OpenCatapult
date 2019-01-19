// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.Provider;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Shared.ApiClient
{
    public class ProviderService : BaseService, IProviderService
    {
        public ProviderService(IApiClient api) : base(api)
        {
        }

        public async Task<ProviderDto> AddProvider(NewProviderDto dto)
        {
            var path = "provider";

            return await Api.Post<NewProviderDto, ProviderDto>(path, dto);
        }

        public async Task DeleteProvider(int id)
        {
            var path = $"provider/{id}";

            await Api.Delete(path);
        }

        public async Task<List<ProviderAdditionalConfigDto>> GetProviderAdditionalConfigByProviderName(string providerName)
        {
            var path = $"provider/name/{providerName}/config";

            return await Api.Get<List<ProviderAdditionalConfigDto>>(path);
        }

        public async Task<ProviderDto> GetProviderById(int id)
        {
            var path = $"provider/{id}";

            return await Api.Get<ProviderDto>(path);
        }

        public async Task<ProviderDto> GetProviderByName(string name)
        {
            var path = $"provider/name/{name}";

            return await Api.Get<ProviderDto>(path);
        }

        public async Task<List<ProviderDto>> GetProviders(string type = "all")
        {
            var path = "provider";
            if (type != "all")
                path += $"/type/{type}";

            return await Api.Get<List<ProviderDto>>(path);
        }
    }
}
