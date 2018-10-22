// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Data.EntityConfigs
{
    public class ProjectMemberConfig : BaseEntityConfig<ProjectMember>
    {
        public override void Configure(EntityTypeBuilder<ProjectMember> builder)
        {
            base.Configure(builder);
            builder.Ignore(m => m.User);
        }
    }
}
