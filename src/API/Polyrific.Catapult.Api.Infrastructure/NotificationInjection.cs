// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polyrific.Catapult.Shared.Common;
using Polyrific.Catapult.Shared.EmailNotification;

namespace Polyrific.Catapult.Api.Infrastructure
{
    public static class NotificationInjection
    {
        public static void AddNotifications(this IServiceCollection services, IConfiguration configuration)
        {
            var providerString = configuration["NotificationProviders"];
            var providers = providerString.Split(',');
            
            if (providers.Contains("email"))
            {
                services.AddEmailSender(configuration);
            }

            services.AddTransient<NotificationProvider>();
        }
    }
}
