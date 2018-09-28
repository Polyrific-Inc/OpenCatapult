// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Data.EntityConfigs
{
    public class ExternalServiceTypeConfig : BaseEntityConfig<ExternalServiceType>
    {
        public override void Configure(EntityTypeBuilder<ExternalServiceType> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Name).IsRequired();

            builder.HasData(
                new ExternalServiceType { Id = 1, Name = "GitHub", Created = new DateTime(2018, 9, 19, 8, 14, 52, 51, DateTimeKind.Utc), ConcurrencyStamp = "2425fe0d-4e3e-4549-a9a7-60056097ce96" },
                new ExternalServiceType { Id = 2, Name = "AzureAppService", Created = new DateTime(2018, 9, 19, 8, 14, 52, 51, DateTimeKind.Utc), ConcurrencyStamp = "2425fe0d-4e3e-4549-a9a7-60056097ce97" }
            );
        }
    }
}
