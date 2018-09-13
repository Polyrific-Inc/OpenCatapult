// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Polyrific.Catapult.Api.Data;
using Polyrific.Catapult.Api.Data.Identity;

namespace Polyrific.Catapult.Api.Infrastructure
{
    public static class IdentityInjection
    {
        public static void AddAppIdentity(this IServiceCollection services)
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