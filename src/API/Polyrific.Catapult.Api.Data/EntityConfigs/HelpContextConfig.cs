// Copyright (c) Polyrific, Inc 2019. All rights reserved.

using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.Constants;

namespace Polyrific.Catapult.Api.Data.EntityConfigs
{
    class HelpContextConfig : BaseEntityConfig<HelpContext>
    {
        public override void Configure(EntityTypeBuilder<HelpContext> builder)
        {
            base.Configure(builder);

            builder.Ignore(p => p.Text);

            builder.HasData(
                /* Project */
                new HelpContext
                {
                    Id = 1,
                    Section = HelpContextSection.Project,
                    SubSection = null,
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d65"
                },
                new HelpContext
                {
                    Id = 2,
                    Section = HelpContextSection.Project,
                    SubSection = "Create Project",
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d66"
                },
                new HelpContext
                {
                    Id = 3,
                    Section = HelpContextSection.Project,
                    SubSection = "Project List",
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d67"
                },

                /*  Data Model */
                new HelpContext
                {
                    Id = 4,
                    Section = HelpContextSection.ProjectModel,
                    SubSection = null,
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d68"
                },
                new HelpContext
                {
                    Id = 5,
                    Section = HelpContextSection.ProjectModel,
                    SubSection = "Properties",
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d69"
                },

                /* Property */
                new HelpContext
                {
                    Id = 6,
                    Section = HelpContextSection.ProjectModelProperty,
                    SubSection = null,
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d70"
                },

                /* Job Definition */
                new HelpContext
                {
                    Id = 7,
                    Section = HelpContextSection.JobDefinition,
                    SubSection = null,
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d71"
                },
                new HelpContext
                {
                    Id = 8,
                    Section = HelpContextSection.JobDefinition,
                    SubSection = "Job Task",
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d72"
                },
                new HelpContext
                {
                    Id = 9,
                    Section = HelpContextSection.JobDefinition,
                    SubSection = "Job Queue",
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d73"
                },

                /* Job Task Definition */
                new HelpContext
                {
                    Id = 10,
                    Section = HelpContextSection.JobTaskDefinition,
                    SubSection = null,
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d74"
                },

                /* Job Queue */
                new HelpContext
                {
                    Id = 11,
                    Section = HelpContextSection.JobQueue,
                    SubSection = null,
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d75"
                },
                new HelpContext
                {
                    Id = 12,
                    Section = HelpContextSection.JobQueue,
                    SubSection = "Logs",
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d76"
                },
                new HelpContext
                {
                    Id = 13,
                    Section = HelpContextSection.JobQueue,
                    SubSection = "Detail",
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d77"
                },

                /* Member */
                new HelpContext
                {
                    Id = 14,
                    Section = HelpContextSection.ProjectMember,
                    SubSection = null,
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d78"
                },
                new HelpContext
                {
                    Id = 15,
                    Section = HelpContextSection.ProjectMember,
                    SubSection = "Project Role",
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d79"
                },

                /* External Service */
                new HelpContext
                {
                    Id = 16,
                    Section = HelpContextSection.ExternalService,
                    SubSection = null,
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d7a"
                },
                new HelpContext
                {
                    Id = 17,
                    Section = HelpContextSection.ExternalService,
                    SubSection = "External Service Type",
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d7b"
                },

                /* Engine */
                new HelpContext
                {
                    Id = 18,
                    Section = HelpContextSection.Engine,
                    SubSection = null,
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d7c"
                },
                new HelpContext
                {
                    Id = 19,
                    Section = HelpContextSection.Engine,
                    SubSection = "Engine Token",
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d7d"
                },

                /* Task Provider */
                new HelpContext
                {
                    Id = 20,
                    Section = HelpContextSection.TaskProvider,
                    SubSection = null,
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d7e"
                },

                /* User */
                new HelpContext
                {
                    Id = 21,
                    Section = HelpContextSection.User,
                    SubSection = null,
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d7f"
                },
                new HelpContext
                {
                    Id = 22,
                    Section = HelpContextSection.User,
                    SubSection = "User Role",
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d80"
                },

                /* User */
                new HelpContext
                {
                    Id = 23,
                    Section = HelpContextSection.UserProfile,
                    SubSection = null,
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d81"
                }
            );
        }
    }
}
