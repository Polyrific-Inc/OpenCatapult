// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Polyrific.Catapult.Api.Data;
using Polyrific.Catapult.Api.Data.Identity;

namespace Polyrific.Catapult.Api.Infrastructure
{
    public static class IdentityInjection
    {
        /// <summary>
        /// Add Identity system to the service collection
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="dbProvider">Database provider (either `mssql` or `sqlite`)</param>
        public static void AddAppIdentity(this IServiceCollection services, string dbProvider = "mssql")
        {
            if (string.IsNullOrEmpty(dbProvider))
                dbProvider = "mssql";

            if (dbProvider.Equals("sqlite", StringComparison.InvariantCultureIgnoreCase))
            {
                services.AddIdentityCore<ApplicationUser>(opt =>
                    {
                        opt.User.RequireUniqueEmail = true;
                    })
                    .AddRoles<ApplicationRole>()
                    .AddEntityFrameworkStores<CatapultSqliteDbContext>()
                    .AddDefaultTokenProviders();
            }
            else
            {
                services.AddIdentityCore<ApplicationUser>(opt =>
                    {
                        opt.User.RequireUniqueEmail = true;
                    })
                    .AddRoles<ApplicationRole>()
                    .AddEntityFrameworkStores<CatapultDbContext>()
                    .AddDefaultTokenProviders();
            }
        }
    }
}
