// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polyrific.Catapult.Cli.Infrastructure;

namespace Polyrific.Catapult.Cli
{
    public static class TokenStoreExtensions
    {
        public static IServiceCollection AddTokenStore(
            this IServiceCollection services,
            IConfiguration configuration,
            string configurationSectionName = "CliConfig")
        {
            var section = configuration.GetSection(configurationSectionName);
            services.Configure<CatapultCliConfig>(configuration);
            var catapultCliConfig = section.Get<CatapultCliConfig>();

            var tokenStore = new TokenStore(catapultCliConfig);
            services.AddSingleton<ITokenStore>(tokenStore);
            services.AddSingleton(catapultCliConfig);

            var token = tokenStore.GetSavedToken().Result;
            if (!string.IsNullOrEmpty(token))
            {
                section[ApiServiceInjection.AuthorizationTokenKey] = token;
            }

            return services;
        }
    }
}
