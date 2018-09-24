// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.Plugin;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Shared.ApiClient
{
    public class PluginService : BaseService, IPluginService
    {
        public PluginService(IApiClient api) : base(api)
        {
        }

        public async Task<PluginDto> AddPlugin(NewPluginDto dto)
        {
            var path = "plugin";

            return await Api.Post<NewPluginDto, PluginDto>(path, dto);
        }

        public async Task DeletePlugin(int id)
        {
            var path = $"plugin/{id}";

            await Api.Delete(path);
        }

        public async Task<List<PluginAdditionalConfigDto>> GetPluginAdditionalConfigByPluginName(string pluginName)
        {
            var path = $"plugin/name/{pluginName}/config";

            return await Api.Get<List<PluginAdditionalConfigDto>>(path);
        }

        public async Task<PluginDto> GetPluginById(int id)
        {
            var path = $"plugin/{id}";

            return await Api.Get<PluginDto>(path);
        }

        public async Task<PluginDto> GetPluginByName(string name)
        {
            var path = $"plugin/name/{name}";

            return await Api.Get<PluginDto>(path);
        }

        public async Task<List<PluginDto>> GetPlugins(string type = "all")
        {
            var path = "plugin";
            if (type != "all")
                path += $"/type/{type}";

            return await Api.Get<List<PluginDto>>(path);
        }
    }
}
