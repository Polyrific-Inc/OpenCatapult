// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.Constants;
using System;

namespace Polyrific.Catapult.Api.Data.EntityConfigs
{
    public class ProjectMemberRoleConfig : BaseEntityConfig<ProjectMemberRole>
    {
        public override void Configure(EntityTypeBuilder<ProjectMemberRole> builder)
        {
            base.Configure(builder);

            builder.HasData(
                new ProjectMemberRole { Id = 1, Name = MemberRole.Owner, Created = new DateTime(2018, 8, 15, 13, 38, 58, 310, DateTimeKind.Utc), ConcurrencyStamp = "ebe3a797-1758-4782-a77b-a78cd08433ea" },
                new ProjectMemberRole { Id = 2, Name = MemberRole.Maintainer, Created = new DateTime(2018, 8, 15, 13, 38, 58, 310, DateTimeKind.Utc), ConcurrencyStamp = "49db1ab1-9f16-4db0-b32d-5a916c2d39cd" },
                new ProjectMemberRole { Id = 3, Name = MemberRole.Contributor, Created = new DateTime(2018, 8, 15, 13, 38, 58, 310, DateTimeKind.Utc), ConcurrencyStamp = "82dcaf01-bc5f-4964-b665-56074560861f" },
                new ProjectMemberRole { Id = 4, Name = MemberRole.Member, Created = new DateTime(2018, 8, 15, 13, 38, 58, 310, DateTimeKind.Utc), ConcurrencyStamp = "d25d2b9c-b2dc-4a36-99af-0622de434e83" });
        }
    }
}