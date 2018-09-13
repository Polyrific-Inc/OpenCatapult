// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Data.EntityConfigs
{
    public class ProjectDataModelConfig : BaseEntityConfig<ProjectDataModel>
    {
        public override void Configure(EntityTypeBuilder<ProjectDataModel> builder)
        {
            base.Configure(builder);
            builder.HasMany(m => m.Properties).WithOne(p => p.ProjectDataModel).HasForeignKey(pm => pm.ProjectDataModelId).IsRequired();
        }
    }
}