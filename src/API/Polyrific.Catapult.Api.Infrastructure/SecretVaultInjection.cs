// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.DependencyInjection;
using Polyrific.Catapult.Shared.SecretVault;
using System;

namespace Polyrific.Catapult.Api.Infrastructure
{
    public static class SecretVaultInjection
    {
        public static void RegisterSecretVault(this IServiceCollection services, string provider)
        {
            switch (provider)
            {
                case "catapult":
                    services.AddCatapultSecretVault();
                    break;
                default:
                    throw new ArgumentException($"Secret vault provider {provider} is not supported");
            }
        }
    }
}
