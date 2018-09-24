// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.DependencyInjection;
using Polyrific.Catapult.Api.Core.Services;

namespace Polyrific.Catapult.Api
{
    public static class ServiceInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IJobDefinitionService, JobDefinitionService>();
            services.AddTransient<IJobQueueService, JobQueueService>();
            services.AddTransient<IJobCounterService, JobCounterService>();
            services.AddTransient<IProjectDataModelService, ProjectDataModelService>();
            services.AddTransient<IProjectMemberService, ProjectMemberService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICatapultEngineService, CatapultEngineService>();
            services.AddTransient<IExternalServiceService, ExternalServiceService>();
            services.AddTransient<IExternalServiceTypeService, ExternalServiceTypeService>();
            services.AddTransient<IPluginService, PluginService>();
            services.AddTransient<IPluginAdditionalConfigService, PluginAdditionalConfigService>();
        }
    }
}
