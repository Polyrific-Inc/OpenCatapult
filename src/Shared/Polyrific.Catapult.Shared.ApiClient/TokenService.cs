// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.CatapultEngine;
using Polyrific.Catapult.Shared.Dto.User;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Shared.ApiClient
{
    public class TokenService : BaseService, ITokenService
    {
        public TokenService(IApiClient api) : base(api)
        {
        }

        public async Task<string> RequestToken(RequestTokenDto dto)
        {
            var path = $"token";

            return await Api.Post(path, dto);
        }

        public async Task<string> RequestEngineToken(int engineId, RequestEngineTokenDto dto)
        {
            var path = $"token/engine/{engineId}";

            return await Api.Post(path, dto);
        }

        public async Task<string> RefreshToken()
        {
            return await Api.Get<string>("token/refresh");
        }
    }
}
