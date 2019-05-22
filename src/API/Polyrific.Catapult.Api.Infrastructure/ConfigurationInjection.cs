// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Polyrific.Catapult.Api.Data.Configuration;

namespace Polyrific.Catapult.Api.Infrastructure
{
    public static class ConfigurationExtensions
    {
        public static IConfigurationBuilder AddDbConfiguration(this IConfigurationBuilder builder, IConfiguration config)
        {
            var dbProvider = config["DatabaseProvider"];
            var connectionString = config.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(dbProvider))
                dbProvider = "mssql";

            if (dbProvider.Equals("sqlite", StringComparison.InvariantCultureIgnoreCase))
            {
                builder.AddApplicationSettingProvider(options => options.UseSqlite(connectionString));
            }
            else
            {
                builder.AddApplicationSettingProvider(options => options.UseSqlServer(connectionString));
            }

            return builder;
        }

        public static IConfigurationBuilder AddApplicationSettingProvider(
            this IConfigurationBuilder configuration, Action<DbContextOptionsBuilder> setup)
        {
            configuration.Add(new ApplicationSettingSource(setup));
            return configuration;
        }
    }
}
