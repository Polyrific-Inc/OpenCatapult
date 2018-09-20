// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Data.EntityConfigs
{
    public class ExternalServiceConfig : BaseEntityConfig<ExternalService>
    {
        public override void Configure(EntityTypeBuilder<ExternalService> builder)
        {
            base.Configure(builder);

            builder.Ignore(e => e.ConfigString);
            builder.Property(e => e.Name).IsRequired();
        }
    }
}
