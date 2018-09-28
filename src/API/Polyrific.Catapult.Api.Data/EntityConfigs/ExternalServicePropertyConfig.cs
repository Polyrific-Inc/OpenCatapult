// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Data.EntityConfigs
{
    public class ExternalServicePropertyConfig : BaseEntityConfig<ExternalServiceProperty>
    {
        public override void Configure(EntityTypeBuilder<ExternalServiceProperty> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Name).IsRequired();

            builder.HasData(
                new ExternalServiceProperty { Id = 1, ExternalServiceTypeId = 1, Name = "RemoteUrl", Description = "Remote Url", IsRequired = true, Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), ConcurrencyStamp = "bb36270b-654f-42bc-8508-c8bd0acafb5b" },
                new ExternalServiceProperty { Id = 2, ExternalServiceTypeId = 1, Name = "RemoteCredentialType", Description = "Remote Credential Type (\"userPassword\" or \"authToken\")", AllowedValues = "userPassword,authToken", IsRequired = true, Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d65" },
                new ExternalServiceProperty { Id = 3, ExternalServiceTypeId = 1, Name = "RemoteUsername", Description = "Remote Username", Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), ConcurrencyStamp = "4bd86c55-ffc1-4c49-a4e4-c1ee809f311d" },
                new ExternalServiceProperty { Id = 4, ExternalServiceTypeId = 1, Name = "RemotePassword", Description = "Remote Password", IsSecret = true, Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), ConcurrencyStamp = "c1eeaa4b-bdc2-4ef9-a52d-393fe9dca59a" },
                new ExternalServiceProperty { Id = 5, ExternalServiceTypeId = 1, Name = "RepoAuthToken", Description = "Repository Auth Token", IsSecret = true, Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), ConcurrencyStamp = "416fcf67-35cf-4ea3-b534-dade4a81da88" },
                
                new ExternalServiceProperty { Id = 6, ExternalServiceTypeId = 2, Name = "ApplicationId", Description = "Application Id", IsRequired = true, Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), ConcurrencyStamp = "416fcf67-35cf-4ea3-b534-dade4a81da89" },
                new ExternalServiceProperty { Id = 7, ExternalServiceTypeId = 2, Name = "ApplicationKey", Description = "Application Key", IsRequired = true, IsSecret = true, Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), ConcurrencyStamp = "416fcf67-35cf-4ea3-b534-dade4a81da8a" },
                new ExternalServiceProperty { Id = 8, ExternalServiceTypeId = 2, Name = "TenantId", Description = "Tenant Id", IsRequired = true, Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), ConcurrencyStamp = "416fcf67-35cf-4ea3-b534-dade4a81da8b" }
            );
        }
    }
}
