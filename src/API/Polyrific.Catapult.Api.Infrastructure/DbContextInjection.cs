// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Polyrific.Catapult.Api.Data;

namespace Polyrific.Catapult.Api.Infrastructure
{
    public static class DbContextInjection
    {
        public static void RegisterDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<CatapultDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }
    }
}