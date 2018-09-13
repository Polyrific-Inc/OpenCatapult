// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Common;
using Polyrific.Catapult.Shared.Dto.Constants;
using System;
using System.Security.Cryptography;

namespace Polyrific.Catapult.Api.Data.Identity
{
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users");

            builder.HasMany<ProjectMember>().WithOne().HasForeignKey(pm => pm.UserId).IsRequired();

            builder.HasData(CreateInitialUser());
        }

        private ApplicationUser CreateInitialUser()
        {
            var user = new ApplicationUser(1, "admin@opencatapult.net")
            {
                EmailConfirmed = true,

                // ideally these values don't need to be set here,
                // it's just a workaround because of a bug in ef core 2.1 which prevents migrations to work as expected
                PasswordHash = "AQAAAAEAACcQAAAAEKBBPo49hQnfSTCnZPTPvpdvqOA5YKXoS8XT6S4hbX9vVTzjKzgXGmUUKWnpOvyjhA==",
                ConcurrencyStamp = "6e60fade-1c1f-4f6a-ab7e-768358780783",
                SecurityStamp = "D4ZMGAXVOVP33V5FMDWVCZ7ZMH5R2JCK"
            };

            //SetPasswordHash(user, "opencatapult");

            return user;
        }

        private void SetPasswordHash(ApplicationUser user, string plainPassword)
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            var passwordHash = passwordHasher.HashPassword(user, plainPassword);
            user.PasswordHash = passwordHash;
        }

        private string GenerateSecurityStamp()
        {
            byte[] bytes = new byte[20];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Base32.ToBase32(bytes);
        }
        
    }
    
    public class ApplicationRoleConfig : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.ToTable("Roles");

            builder.HasData(
                new ApplicationRole(1, UserRole.Administrator){ConcurrencyStamp = "f8025fee-dec6-4528-9514-58339adc3383"},
                new ApplicationRole(2, UserRole.Basic) {ConcurrencyStamp = "c7cbed51-e910-4c2d-ab17-b27d3001ea47"},
                new ApplicationRole(3, UserRole.Guest) {ConcurrencyStamp = "18f44ef4-86b2-4ebb-a400-b2615c9715e0" },
                new ApplicationRole(4, UserRole.Engine) {ConcurrencyStamp = "0c810611-1e85-47cc-a7a1-7c57ff3e29bb" }
            );
        }
    }

    public class ApplicationUserRoleConfig : IEntityTypeConfiguration<ApplicationUserRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
        {
            builder.ToTable("UserRoles");

            builder.HasData(new ApplicationUserRole(1, 1));
        }
    }

    public class ApplicationUserClaimConfig : IEntityTypeConfiguration<ApplicationUserClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserClaim> builder)
        {
            builder.ToTable("UserClaims");
        }
    }

    public class ApplicationUserLoginConfig : IEntityTypeConfiguration<ApplicationUserLogin>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserLogin> builder)
        {
            builder.ToTable("UserLogins");
        }
    }

    public class ApplicationRoleClaimConfig : IEntityTypeConfiguration<ApplicationRoleClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
        {
            builder.ToTable("RoleClaims");
        }
    }

    public class ApplicationUserTokenConfig : IEntityTypeConfiguration<ApplicationUserToken>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserToken> builder)
        {
            builder.ToTable("UserTokens");
        }
    }

    public class UserProfileConfig : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.HasOne(profile => profile.ApplicationUser)
                .WithOne(user => user.UserProfile)
                .HasForeignKey<UserProfile>(profile => profile.ApplicationUserId)
                .IsRequired(false);

            builder.Property(p => p.IsActive).HasDefaultValue(true);

            builder.HasData(new UserProfile
            {
                Id = 1,
                ApplicationUserId = 1,
                ConcurrencyStamp = "99aa6fde-2675-4aa9-a60d-e45ba72fb9d0",
                Created = new DateTime(2018, 8, 23, 10, 4, 6, 797, DateTimeKind.Utc),
                IsActive = true
            });
        }
    }

    public class CatapultEngineProfileConfig : IEntityTypeConfiguration<CatapultEngineProfile>
    {
        public void Configure(EntityTypeBuilder<CatapultEngineProfile> builder)
        {
            builder.HasOne(profile => profile.CatapultEngine)
                .WithOne(user => user.CatapultEngineProfile)
                .HasForeignKey<CatapultEngineProfile>(profile => profile.CatapultEngineId)
                .IsRequired(false);

            builder.Property(p => p.IsActive).HasDefaultValue(true);
        }
    }
}