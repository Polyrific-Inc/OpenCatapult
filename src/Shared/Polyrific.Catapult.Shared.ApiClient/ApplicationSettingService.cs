using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.ApplicationSetting;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Shared.ApiClient
{
    public class ApplicationSettingService : BaseService, IApplicationSettingService
    {
        public ApplicationSettingService(IApiClient api) : base(api)
        {
        }

        public async Task<List<ApplicationSettingDto>> GetApplicationSettings()
        {
            var path = $"applicationsetting";

            return await Api.Get<List<ApplicationSettingDto>>(path);
        }

        public async Task<ApplicationSettingValueDto> GetApplicationSettingValue()
        {
            var path = $"applicationsetting/value";

            return await Api.Get<ApplicationSettingValueDto>(path);
        }

        public async Task UpdateApplicationSetting(UpdateApplicationSettingDto dto)
        {
            var path = $"applicationsetting";

            await Api.Put(path, dto);
        }
    }
}
