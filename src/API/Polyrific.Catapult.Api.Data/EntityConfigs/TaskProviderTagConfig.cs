// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Data.EntityConfigs
{
    public class TaskProviderTagConfig : BaseEntityConfig<TaskProviderTag>
    {
        public override void Configure(EntityTypeBuilder<TaskProviderTag> builder)
        {
            base.Configure(builder);

            builder.HasOne(pt => pt.TaskProvider)
                .WithMany(p => p.Tags)
                .HasForeignKey(pt => pt.TaskProviderId);

            builder.HasOne(pt => pt.Tag)
                .WithMany(p => p.ProviderTags)
                .HasForeignKey(pt => pt.TagId);

            builder.HasData(
                new TaskProviderTag { Id = 1, TaskProviderId = 1, TagId = 1, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d07" },
                new TaskProviderTag { Id = 2, TaskProviderId = 1, TagId = 7, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d08" },
                new TaskProviderTag { Id = 3, TaskProviderId = 1, TagId = 8, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d09" },
                new TaskProviderTag { Id = 4, TaskProviderId = 1, TagId = 9, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d0a" },
                new TaskProviderTag { Id = 5, TaskProviderId = 1, TagId = 10, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d0b" },
                new TaskProviderTag { Id = 6, TaskProviderId = 1, TagId = 11, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d0c" },
                new TaskProviderTag { Id = 7, TaskProviderId = 1, TagId = 12, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d0d" },
                new TaskProviderTag { Id = 8, TaskProviderId = 1, TagId = 13, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d0e" },
                new TaskProviderTag { Id = 9, TaskProviderId = 1, TagId = 14, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d0f" },
                new TaskProviderTag { Id = 10, TaskProviderId = 2, TagId = 2, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d10" },
                new TaskProviderTag { Id = 11, TaskProviderId = 2, TagId = 15, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d11" },
                new TaskProviderTag { Id = 12, TaskProviderId = 2, TagId = 16, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d12" },
                new TaskProviderTag { Id = 13, TaskProviderId = 2, TagId = 17, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d13" },
                new TaskProviderTag { Id = 14, TaskProviderId = 3, TagId = 3, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d14" },
                new TaskProviderTag { Id = 15, TaskProviderId = 3, TagId = 7, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d15" },
                new TaskProviderTag { Id = 16, TaskProviderId = 3, TagId = 8, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d16" },
                new TaskProviderTag { Id = 17, TaskProviderId = 4, TagId = 4, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d17" },
                new TaskProviderTag { Id = 18, TaskProviderId = 4, TagId = 7, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d18" },
                new TaskProviderTag { Id = 19, TaskProviderId = 4, TagId = 18, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d19" },
                new TaskProviderTag { Id = 20, TaskProviderId = 4, TagId = 19, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d1a" },
                new TaskProviderTag { Id = 21, TaskProviderId = 5, TagId = 5, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d1b" },
                new TaskProviderTag { Id = 22, TaskProviderId = 5, TagId = 7, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d1c" },
                new TaskProviderTag { Id = 23, TaskProviderId = 5, TagId = 8, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d1d" },
                new TaskProviderTag { Id = 24, TaskProviderId = 5, TagId = 14, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d1f" },
                new TaskProviderTag { Id = 25, TaskProviderId = 6, TagId = 6, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d20" },
                new TaskProviderTag { Id = 26, TaskProviderId = 6, TagId = 8, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d21" },
                new TaskProviderTag { Id = 27, TaskProviderId = 6, TagId = 12, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d22" },
                new TaskProviderTag { Id = 28, TaskProviderId = 6, TagId = 20, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d23" },
                new TaskProviderTag { Id = 29, TaskProviderId = 6, TagId = 21, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d24" },
                new TaskProviderTag { Id = 30, TaskProviderId = 6, TagId = 22, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d25" },
                new TaskProviderTag { Id = 31, TaskProviderId = 6, TagId = 23, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d26" },
                new TaskProviderTag { Id = 32, TaskProviderId = 6, TagId = 24, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d27" },
                new TaskProviderTag { Id = 33, TaskProviderId = 7, TagId = 25, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d28" },
                new TaskProviderTag { Id = 34, TaskProviderId = 7, TagId = 26, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d29" },
                new TaskProviderTag { Id = 35, TaskProviderId = 7, TagId = 27, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d2a" },
                new TaskProviderTag { Id = 36, TaskProviderId = 7, TagId = 28, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d2b" },
                new TaskProviderTag { Id = 37, TaskProviderId = 7, TagId = 29, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d2c" },
                new TaskProviderTag { Id = 38, TaskProviderId = 7, TagId = 30, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d2d" }
                );
        }
    }
}
