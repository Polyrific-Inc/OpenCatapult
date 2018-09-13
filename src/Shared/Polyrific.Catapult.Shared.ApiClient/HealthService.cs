// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Shared.Service;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Shared.ApiClient
{
    public class HealthService : BaseService, IHealthService
    {
        public HealthService(IApiClient api) : base(api)
        {
        }

        public async Task<bool> CheckHealth()
        {
            var path = "health";

            return await Api.Head(path);
        }

        public async Task<bool> CheckHealthSecure()
        {
            var path = "health/secure";

            return await Api.Head(path);
        }
    }
}