// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Polyrific.Catapult.Api.Data;

namespace Polyrific.Catapult.Api.Infrastructure
{
    public static class DbContextInjection
    {
        /// <summary>
        /// Register DbContext to the service collection
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="connectionString">Connection string for the database</param>
        /// <param name="dbProvider">Database provider (either `mssql` or `sqlite`)</param>
        public static void RegisterDbContext(this IServiceCollection services, string connectionString, string dbProvider = "mssql")
        {
            if (string.IsNullOrEmpty(dbProvider))
                dbProvider = "mssql";

            if (dbProvider.Equals("sqlite", StringComparison.InvariantCultureIgnoreCase))
            {
                services.AddDbContext<CatapultSqliteDbContext>(options =>
                {
                    options.UseSqlite(connectionString);
                });
            }
            else
            {
                services.AddDbContext<CatapultDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });
            }
        }
    }
}
