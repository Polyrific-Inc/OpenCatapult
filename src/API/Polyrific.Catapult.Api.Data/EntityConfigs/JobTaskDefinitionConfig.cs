// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Data.EntityConfigs
{
    public class JobTaskDefinitionConfig : BaseEntityConfig<JobTaskDefinition>
    {
        public override void Configure(EntityTypeBuilder<JobTaskDefinition> builder)
        {
            base.Configure(builder);

            builder.Ignore(p => p.Valid);
            builder.Ignore(p => p.ValidationError);
        }
    }
}
