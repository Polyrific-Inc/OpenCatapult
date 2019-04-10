// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Data.EntityConfigs
{
    public class PluginTagConfig : BaseEntityConfig<PluginTag>
    {
        public override void Configure(EntityTypeBuilder<PluginTag> builder)
        {
            base.Configure(builder);

            builder.HasOne(pt => pt.Plugin)
                .WithMany(p => p.Tags)
                .HasForeignKey(pt => pt.PluginId);

            builder.HasOne(pt => pt.Tag)
                .WithMany(p => p.PluginTags)
                .HasForeignKey(pt => pt.TagId);

            builder.HasData(
                new PluginTag { Id = 1, PluginId = 1, TagId = 1, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d07" },
                new PluginTag { Id = 2, PluginId = 1, TagId = 7, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d08" },
                new PluginTag { Id = 3, PluginId = 1, TagId = 8, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d09" },
                new PluginTag { Id = 4, PluginId = 1, TagId = 9, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d0a" },
                new PluginTag { Id = 5, PluginId = 1, TagId = 10, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d0b" },
                new PluginTag { Id = 6, PluginId = 1, TagId = 11, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d0c" },
                new PluginTag { Id = 7, PluginId = 1, TagId = 12, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d0d" },
                new PluginTag { Id = 8, PluginId = 1, TagId = 13, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d0e" },
                new PluginTag { Id = 9, PluginId = 1, TagId = 14, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d0f" },
                new PluginTag { Id = 10, PluginId = 2, TagId = 2, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d10" },
                new PluginTag { Id = 11, PluginId = 2, TagId = 15, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d11" },
                new PluginTag { Id = 12, PluginId = 2, TagId = 16, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d12" },
                new PluginTag { Id = 13, PluginId = 2, TagId = 17, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d13" },
                new PluginTag { Id = 14, PluginId = 3, TagId = 3, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d14" },
                new PluginTag { Id = 15, PluginId = 3, TagId = 7, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d15" },
                new PluginTag { Id = 16, PluginId = 3, TagId = 8, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d16" },
                new PluginTag { Id = 17, PluginId = 4, TagId = 4, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d17" },
                new PluginTag { Id = 18, PluginId = 4, TagId = 7, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d18" },
                new PluginTag { Id = 19, PluginId = 4, TagId = 18, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d19" },
                new PluginTag { Id = 20, PluginId = 4, TagId = 19, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d1a" },
                new PluginTag { Id = 21, PluginId = 5, TagId = 5, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d1b" },
                new PluginTag { Id = 22, PluginId = 5, TagId = 7, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d1c" },
                new PluginTag { Id = 23, PluginId = 5, TagId = 8, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d1d" },
                new PluginTag { Id = 24, PluginId = 5, TagId = 14, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d1f" },
                new PluginTag { Id = 25, PluginId = 6, TagId = 6, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d20" },
                new PluginTag { Id = 26, PluginId = 6, TagId = 8, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d21" },
                new PluginTag { Id = 27, PluginId = 6, TagId = 12, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d22" },
                new PluginTag { Id = 28, PluginId = 6, TagId = 20, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d23" },
                new PluginTag { Id = 29, PluginId = 6, TagId = 21, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d24" },
                new PluginTag { Id = 30, PluginId = 6, TagId = 22, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d25" },
                new PluginTag { Id = 31, PluginId = 6, TagId = 23, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d26" },
                new PluginTag { Id = 32, PluginId = 6, TagId = 24, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d27" },
                new PluginTag { Id = 33, PluginId = 7, TagId = 25, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d28" },
                new PluginTag { Id = 34, PluginId = 7, TagId = 26, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d29" },
                new PluginTag { Id = 35, PluginId = 7, TagId = 27, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d2a" },
                new PluginTag { Id = 36, PluginId = 7, TagId = 28, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d2b" },
                new PluginTag { Id = 37, PluginId = 7, TagId = 29, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d2c" },
                new PluginTag { Id = 38, PluginId = 7, TagId = 30, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d2d" }
                );
        }
    }
}
