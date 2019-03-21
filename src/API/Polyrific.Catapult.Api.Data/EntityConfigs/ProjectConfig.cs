// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.Constants;

namespace Polyrific.Catapult.Api.Data.EntityConfigs
{
    public class ProjectConfig : BaseEntityConfig<Project>
    {
        public override void Configure(EntityTypeBuilder<Project> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.Status).IsRequired().HasDefaultValue(ProjectStatusFilterType.Active);
        }
    }
}
