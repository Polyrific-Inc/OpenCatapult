// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polyrific.Catapult.Engine.Core.JobLogger;

namespace Polyrific.Catapult.Engine.SignalRLogger
{
    public static class SignalRLogWriterExtensions
    {
        public static IServiceCollection AddSignalRLogger(this IServiceCollection services, IConfiguration configuration, string configurationSectionName)
        {
            var section = configuration.GetSection(configurationSectionName);
            var policyOptions = section.Get<SignalRClientOption>();

            services.AddSingleton(policyOptions);
            services.AddTransient<IJobLogWriter, SignalRJobLogWriter>();
            return services;
        }
    }
}
