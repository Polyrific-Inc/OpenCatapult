// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polyrific.Catapult.Shared.ApiClient;
using Polyrific.Catapult.Shared.ApiClient.Framework;
using Polyrific.Catapult.Shared.ApiClient.Options;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Engine.Infrastructure
{
    public static class ApiServiceInjection
    {
        public static void AddCatapultApi(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddPolicies(configuration)
                .AddHttpClient<IApiClient, ApiClient, ApiClientOptions>(configuration, "EngineConfig");

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IHealthService, HealthService>();
            services.AddTransient<IJobDefinitionService, JobDefinitionService>();
            services.AddTransient<IJobQueueService, JobQueueService>();
            services.AddTransient<IProjectDataModelService, ProjectDataModelService>();
            services.AddTransient<IProjectMemberService, ProjectMemberService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IExternalServiceService, ExternalServiceService>();
        }
    }
}
