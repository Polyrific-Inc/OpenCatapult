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

            var identityBuilder = services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
            }).AddDefaultTokenProviders();

            if (dbProvider.Equals("sqlite", StringComparison.InvariantCultureIgnoreCase))
            {
                identityBuilder.AddEntityFrameworkStores<CatapultSqliteDbContext>();
            }
            else
            {
                identityBuilder.AddEntityFrameworkStores<CatapultDbContext>();
            }
        }
    }
}
