// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.ExternalServiceType;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Shared.ApiClient
{
    public class ExternalServiceTypeService : BaseService, IExternalServiceTypeService
    {
        public ExternalServiceTypeService(IApiClient api) : base(api)
        {
        }

        public async Task<ExternalServiceTypeDto> GetExternalServiceType(int id)
        {
            var path = $"serviceType/{id}";

            return await Api.Get<ExternalServiceTypeDto>(path);
        }

        public async Task<ExternalServiceTypeDto> GetExternalServiceTypeByName(string name)
        {
            var path = $"serviceType/name/{name}";

            return await Api.Get<ExternalServiceTypeDto>(path);
        }

        public async Task<List<ExternalServiceTypeDto>> GetExternalServiceTypes()
        {
            var path = "serviceType";

            return await Api.Get<List<ExternalServiceTypeDto>>(path);
        }
    }
}
