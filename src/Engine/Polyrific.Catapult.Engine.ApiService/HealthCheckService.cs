// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;
using Polyrific.Catapult.Engine.Core.Services;

namespace Polyrific.Catapult.Engine.ApiService
{
    public class HealthCheckService : BaseService, IHealthCheckService
    {
        public HealthCheckService(ApiClient api) : base(api)
        {
        }

        public async Task<bool> IsApiHealthy()
        {
            return await Api.Head("/health");
        }
    }
}