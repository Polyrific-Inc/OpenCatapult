// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polyrific.Catapult.Shared.Common.Interface;

namespace Polyrific.Catapult.Shared.SmtpEmailNotification
{
    public static class SmtpEmailSenderInjection
    {
        public static void AddSmtpEmailSender(this IServiceCollection services, IConfiguration configuration, string configurationSectionName = "SmtpSetting")
        {
            var section = configuration.GetSection(configurationSectionName);
            var smtpSetting = section.Get<SmtpSetting>();
            services.AddSingleton(smtpSetting);
            services.AddTransient<INotificationSender, SmtpEmailSender>();
        }
    }
}
