// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.Version;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Shared.ApiClient
{
    public class VersionService : BaseService, IVersionService
    {
        public VersionService(IApiClient api) : base(api)
        {
        }

        public async Task<VersionDto> GetApiVersion()
        {
            return await Api.Get<VersionDto>("version");
        }
    }
}
