// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Shared.Dto.CatapultEngine;
using Polyrific.Catapult.Shared.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Shared.ApiClient
{
    public class CatapultEngineService : BaseService, ICatapultEngineService
    {
        public CatapultEngineService(IApiClient api) : base(api)
        {
        }

        public async Task<CatapultEngineDto> GetCatapultEngineByName(string name)
        {
            var path = $"engine/name/{name}";

            return await Api.Get<CatapultEngineDto>(path);
        }

        public async Task<List<CatapultEngineDto>> GetCatapultEngines(string status)
        {
            var path = $"engine?status={status}";

            return await Api.Get<List<CatapultEngineDto>>(path);
        }

        public async Task Reactivate(int engineId)
        {
            var path = $"engine/{engineId}/activate";

            await Api.Post<object>(path, null);
        }

        public async Task<RegisterCatapultEngineResponseDto> RegisterEngine(RegisterCatapultEngineDto dto)
        {
            var path = $"engine/register";

            return await Api.Post<RegisterCatapultEngineDto, RegisterCatapultEngineResponseDto>(path, dto);
        }

        public async Task RemoveCatapultEngine(int engineId)
        {
            var path = $"engine/{engineId}";

            await Api.Delete(path);
        }

        public async Task Suspend(int engineId)
        {
            var path = $"engine/{engineId}/suspend";

            await Api.Post<object>(path, null);
        }
    }
}
