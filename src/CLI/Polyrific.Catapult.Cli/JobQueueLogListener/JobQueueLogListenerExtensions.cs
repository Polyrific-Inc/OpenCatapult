// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Polyrific.Catapult.Cli
{
    public static class JobQueueLogListenerExtensions
    {
        public static IServiceCollection AddJobQueueLogListener(
            this IServiceCollection services,
            IConfiguration configuration,
            string configurationSectionName = "CliConfig")
        {
            services.AddTransient<IJobQueueLogListener, SignalRJobQueueLogListener>();

            return services;
        }
    }
}
