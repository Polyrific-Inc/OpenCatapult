// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polyrific.Catapult.Engine.SignalRLogger;

namespace Polyrific.Catapult.Engine.Infrastructure
{
    public static class LoggerInjection
    {
        public static void AdJobLogWriter(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSignalRLogger(configuration, "EngineConfig");
        }
    }
}
