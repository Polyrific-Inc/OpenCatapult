// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.DependencyInjection;
using Polyrific.Catapult.Api.Core.Security;

namespace Polyrific.Catapult.Api.SecretVault
{
    public static class SecretVaultInjection
    {
        public static void AddCatapultSecretVault(this IServiceCollection services)
        {
            services.AddDataProtection();
            services.AddTransient<ISecretVault, SecretVault>();
        }
    }
}
