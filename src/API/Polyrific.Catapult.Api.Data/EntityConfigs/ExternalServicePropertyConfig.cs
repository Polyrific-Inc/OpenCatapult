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
                new ExternalServiceProperty { Id = 1, ExternalServiceTypeId = 2, Name = "RemoteCredentialType", Description = "Remote Credential Type (\"userPassword\" or \"authToken\")", AllowedValues = "userPassword,authToken", IsRequired = true, Sequence = 1, Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d65" },
                new ExternalServiceProperty { Id = 2, ExternalServiceTypeId = 2, Name = "RemoteUsername", Description = "Remote Username", Sequence = 2, Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), ConcurrencyStamp = "4bd86c55-ffc1-4c49-a4e4-c1ee809f311d",
                    AdditionalLogic= "{\"HideCondition\": { \"PropertyName\": \"RemoteCredentialType\", \"PropertyValue\": \"authToken\" }, \"RequiredCondition\": { \"PropertyName\": \"RemoteCredentialType\", \"PropertyValue\": \"userPassword\" } }"
                },
                new ExternalServiceProperty { Id = 3, ExternalServiceTypeId = 2, Name = "RemotePassword", Description = "Remote Password", IsSecret = true, Sequence = 3, Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), ConcurrencyStamp = "c1eeaa4b-bdc2-4ef9-a52d-393fe9dca59a",
                    AdditionalLogic = "{\"HideCondition\": { \"PropertyName\": \"RemoteCredentialType\", \"PropertyValue\": \"authToken\" }, \"RequiredCondition\": { \"PropertyName\": \"RemoteCredentialType\", \"PropertyValue\": \"userPassword\" } }"
                },
                new ExternalServiceProperty { Id = 4, ExternalServiceTypeId = 2, Name = "RepoAuthToken", Description = "Repository Auth Token", IsSecret = true, Sequence = 4, Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), ConcurrencyStamp = "416fcf67-35cf-4ea3-b534-dade4a81da88",
                    AdditionalLogic = "{\"HideCondition\": { \"PropertyName\": \"RemoteCredentialType\", \"PropertyValue\": \"userPassword\" }, \"RequiredCondition\": { \"PropertyName\": \"RemoteCredentialType\", \"PropertyValue\": \"authToken\" } }"
                },
                
                new ExternalServiceProperty { Id = 5, ExternalServiceTypeId = 3, Name = "ApplicationId", Description = "Application Id", IsRequired = true, Sequence = 1, Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), ConcurrencyStamp = "416fcf67-35cf-4ea3-b534-dade4a81da89" },
                new ExternalServiceProperty { Id = 6, ExternalServiceTypeId = 3, Name = "ApplicationKey", Description = "Application Key", IsRequired = true, IsSecret = true, Sequence = 2, Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), ConcurrencyStamp = "416fcf67-35cf-4ea3-b534-dade4a81da8a" },
                new ExternalServiceProperty { Id = 7, ExternalServiceTypeId = 3, Name = "TenantId", Description = "Tenant Id", IsRequired = true, Sequence = 3, Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), ConcurrencyStamp = "416fcf67-35cf-4ea3-b534-dade4a81da8b" }
            );
        }
    }
}
