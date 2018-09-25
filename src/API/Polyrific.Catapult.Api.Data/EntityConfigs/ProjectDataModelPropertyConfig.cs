// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Data.EntityConfigs
{
    public class ProjectDataModelPropertyConfig : BaseEntityConfig<ProjectDataModelProperty>
    {
        public override void Configure(EntityTypeBuilder<ProjectDataModelProperty> builder)
        {
            base.Configure(builder);

            builder.Ignore(e => e.RelatedProjectDataModelName);
        }
    }
}
