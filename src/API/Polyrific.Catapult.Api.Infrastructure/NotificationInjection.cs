// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polyrific.Catapult.Shared.Common.Notification;
using Polyrific.Catapult.Shared.SmtpEmailNotification;

namespace Polyrific.Catapult.Api.Infrastructure
{
    public static class NotificationInjection
    {
        public static void AddNotifications(this IServiceCollection services, IConfiguration configuration)
        {
            var providerString = configuration["NotificationProviders"];
            var providers = providerString.Split(',');
            
            if (providers.Contains(NotificationProvider.SmtpEmail))
            {
                services.AddSmtpEmailSender(configuration);
            }

            NotificationConfig.InitConfigFile().Wait();
            services.AddTransient<NotificationConfig>();
            services.AddTransient<INotificationProvider, NotificationProvider>();
        }
    }
}
