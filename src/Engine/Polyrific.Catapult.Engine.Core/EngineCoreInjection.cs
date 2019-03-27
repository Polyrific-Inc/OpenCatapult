// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.DependencyInjection;
using Polyrific.Catapult.Engine.Core.JobTasks;

namespace Polyrific.Catapult.Engine.Core
{
    public static class EngineCoreInjection
    {
        public static void AddEngineCore(this IServiceCollection services)
        {
            services.AddTransient<ICatapultEngine, CatapultEngine>();
            services.AddTransient<ICatapultEngineConfig, CatapultEngineConfig>();
            services.AddTransient<ITaskRunner, TaskRunner>();

            services.AddTransient<IBuildTask, BuildTask>();
            services.AddTransient<ICloneTask, CloneTask>();
            services.AddTransient<IDeployTask, DeployTask>();
            services.AddTransient<IDeployDbTask, DeployDbTask>();
            services.AddTransient<IGenerateTask, GenerateTask>();
            services.AddTransient<IMergeTask, MergeTask>();
            services.AddTransient<IPublishArtifactTask, PublishArtifactTask>();
            services.AddTransient<IPushTask, PushTask>();
            services.AddTransient<ITestTask, TestTask>();
            services.AddTransient<IDeleteRepositoryTask, DeleteRepositoryTask>();
            services.AddTransient<IDeleteHostingTask, DeleteHostingTask>();
            services.AddTransient<ICustomTask, CustomTask>();
            services.AddTransient<JobTaskService, JobTaskService>();
        }
    }
}
