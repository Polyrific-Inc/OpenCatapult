// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.DependencyInjection;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Data;
using Polyrific.Catapult.Api.Data.Identity;

namespace Polyrific.Catapult.Api.Infrastructure
{
    public static class RepositoryInjection
    {
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IJobDefinitionRepository, JobDefinitionRepository>();
            services.AddScoped<IJobTaskDefinitionRepository, JobTaskDefinitionRepository>();
            services.AddScoped<IJobQueueRepository, JobQueueRepository>();
            services.AddScoped<IJobCounterRepository, JobCounterRepository>();
            services.AddScoped<IProjectDataModelPropertyRepository, ProjectDataModelPropertyRepository>();
            services.AddScoped<IProjectDataModelRepository, ProjectDataModelRepository>();
            services.AddScoped<IProjectMemberRepository, ProjectMemberRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICatapultEngineRepository, CatapultEngineRepository>();
            services.AddScoped<IExternalServiceRepository, ExternalServiceRepository>();
            services.AddScoped<IExternalServiceTypeRepository, ExternalServiceTypeRepository>();
            services.AddScoped<IRepository<UserProfile>, UserProfileRepository>();
            services.AddScoped<IRepository<CatapultEngineProfile>, CatapultEngineProfileRepository>();
            services.AddScoped<IPluginRepository, PluginRepository>();
            services.AddScoped<IPluginAdditionalConfigRepository, PluginAdditionalConfigRepository>();
        }
    }
}
