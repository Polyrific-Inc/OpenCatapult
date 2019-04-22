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

        public async Task<TaskProviderDto> AddProvider(NewTaskProviderDto dto)
        {
            var path = "task-provider";

            return await Api.Post<NewTaskProviderDto, TaskProviderDto>(path, dto);
        }

        public async Task DeleteProvider(int id)
        {
            var path = $"task-provider/{id}";

            await Api.Delete(path);
        }

        public async Task<List<TaskProviderAdditionalConfigDto>> GetProviderAdditionalConfigByProviderName(string providerName)
        {
            var path = $"task-provider/name/{providerName}/config";

            return await Api.Get<List<TaskProviderAdditionalConfigDto>>(path);
        }

        public async Task<TaskProviderDto> GetProviderById(int id)
        {
            var path = $"task-provider/{id}";

            return await Api.Get<TaskProviderDto>(path);
        }

        public async Task<TaskProviderDto> GetProviderByName(string name)
        {
            var path = $"task-provider/name/{name}";

            return await Api.Get<TaskProviderDto>(path);
        }

        public async Task<List<TaskProviderDto>> GetProviders(string type = "all")
        {
            var path = "task-provider";
            if (type != "all")
                path += $"/type/{type}";

            return await Api.Get<List<TaskProviderDto>>(path);
        }
    }
}
