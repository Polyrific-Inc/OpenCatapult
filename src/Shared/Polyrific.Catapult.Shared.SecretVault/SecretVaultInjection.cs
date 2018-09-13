// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.DependencyInjection;
using Polyrific.Catapult.Shared.Common.Interface;

namespace Polyrific.Catapult.Shared.SecretVault
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
