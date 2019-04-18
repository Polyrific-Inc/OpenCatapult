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


            builder.HasData(
                new HelpContext
                {
                    Id = 1,
                    Section = HelpContextSection.Project,
                    SubSection = null,
                    Text = "Project is a unit of work which your team will work with to create an application",
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d65"
                },
                new HelpContext
                {
                    Id = 2,
                    Section = HelpContextSection.Project,
                    SubSection = "Create Project",
                    Text = "Start to create your own project which have its own model and job definition",
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d66"
                },
                new HelpContext
                {
                    Id = 3,
                    Section = HelpContextSection.Project,
                    SubSection = "Project List",
                    Text = "The list of project is shown based on your access depending on your project membership. An administrator will have access to all projects.",
                    Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                    ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d67"
                }
            );
        }
    }
}
