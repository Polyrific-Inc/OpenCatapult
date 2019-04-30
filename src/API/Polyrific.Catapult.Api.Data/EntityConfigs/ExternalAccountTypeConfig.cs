// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Data.EntityConfigs
{
    public class ExternalAccountTypeConfig : BaseEntityConfig<ExternalAccountType>
    {
        public override void Configure(EntityTypeBuilder<ExternalAccountType> builder)
        {
            base.Configure(builder);

            builder.HasData(new ExternalAccountType { Id = 1, Key = Shared.Dto.Constants.ExternalAccountType.GitHub, Label = "GitHub Id", Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d65" });
        }
    }
}
