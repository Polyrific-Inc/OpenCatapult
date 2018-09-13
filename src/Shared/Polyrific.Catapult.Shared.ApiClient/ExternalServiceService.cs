// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.ExternalService;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Shared.ApiClient
{
    public class ExternalServiceService : BaseService, IExternalServiceService
    {
        public ExternalServiceService(IApiClient api) : base(api)
        {
        }

        public async Task<ExternalServiceDto> CreateExternalService(CreateExternalServiceDto dto)
        {
            var path = "service";

            return await Api.Post<CreateExternalServiceDto, ExternalServiceDto>(path, dto);
        }

        public async Task DeleteExternalService(int serviceId)
        {
            var path = $"service/{serviceId}";

            await Api.Delete(path);
        }

        public async Task<ExternalServiceDto> GetExternalService(int serviceId)
        {
            var path = $"service/{serviceId}";

            return await Api.Get<ExternalServiceDto>(path);
        }

        public async Task<ExternalServiceDto> GetExternalServiceByName(string name)
        {
            var path = $"service/name/{name}";

            return await Api.Get<ExternalServiceDto>(path);
        }

        public async Task<List<ExternalServiceDto>> GetExternalServices()
        {
            var path = $"service";

            return await Api.Get<List<ExternalServiceDto>>(path);
        }

        public async Task UpdateExternalService(int serviceId, UpdateExternalServiceDto dto)
        {
            var path = $"service/{serviceId}";

            await Api.Put<UpdateExternalServiceDto>(path, dto);
        }
    }
}
