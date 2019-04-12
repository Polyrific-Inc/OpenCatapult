// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Data.EntityConfigs
{
    public class ProviderTagConfig : BaseEntityConfig<ProviderTag>
    {
        public override void Configure(EntityTypeBuilder<ProviderTag> builder)
        {
            base.Configure(builder);

            builder.HasOne(pt => pt.Provider)
                .WithMany(p => p.Tags)
                .HasForeignKey(pt => pt.ProviderId);

            builder.HasOne(pt => pt.Tag)
                .WithMany(p => p.ProviderTags)
                .HasForeignKey(pt => pt.TagId);

            builder.HasData(
                new ProviderTag { Id = 1, ProviderId = 1, TagId = 1, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d07" },
                new ProviderTag { Id = 2, ProviderId = 1, TagId = 7, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d08" },
                new ProviderTag { Id = 3, ProviderId = 1, TagId = 8, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d09" },
                new ProviderTag { Id = 4, ProviderId = 1, TagId = 9, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d0a" },
                new ProviderTag { Id = 5, ProviderId = 1, TagId = 10, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d0b" },
                new ProviderTag { Id = 6, ProviderId = 1, TagId = 11, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d0c" },
                new ProviderTag { Id = 7, ProviderId = 1, TagId = 12, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d0d" },
                new ProviderTag { Id = 8, ProviderId = 1, TagId = 13, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d0e" },
                new ProviderTag { Id = 9, ProviderId = 1, TagId = 14, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d0f" },
                new ProviderTag { Id = 10, ProviderId = 2, TagId = 2, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d10" },
                new ProviderTag { Id = 11, ProviderId = 2, TagId = 15, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d11" },
                new ProviderTag { Id = 12, ProviderId = 2, TagId = 16, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d12" },
                new ProviderTag { Id = 13, ProviderId = 2, TagId = 17, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d13" },
                new ProviderTag { Id = 14, ProviderId = 3, TagId = 3, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d14" },
                new ProviderTag { Id = 15, ProviderId = 3, TagId = 7, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d15" },
                new ProviderTag { Id = 16, ProviderId = 3, TagId = 8, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d16" },
                new ProviderTag { Id = 17, ProviderId = 4, TagId = 4, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d17" },
                new ProviderTag { Id = 18, ProviderId = 4, TagId = 7, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d18" },
                new ProviderTag { Id = 19, ProviderId = 4, TagId = 18, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d19" },
                new ProviderTag { Id = 20, ProviderId = 4, TagId = 19, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d1a" },
                new ProviderTag { Id = 21, ProviderId = 5, TagId = 5, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d1b" },
                new ProviderTag { Id = 22, ProviderId = 5, TagId = 7, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d1c" },
                new ProviderTag { Id = 23, ProviderId = 5, TagId = 8, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d1d" },
                new ProviderTag { Id = 24, ProviderId = 5, TagId = 14, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d1f" },
                new ProviderTag { Id = 25, ProviderId = 6, TagId = 6, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d20" },
                new ProviderTag { Id = 26, ProviderId = 6, TagId = 8, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d21" },
                new ProviderTag { Id = 27, ProviderId = 6, TagId = 12, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d22" },
                new ProviderTag { Id = 28, ProviderId = 6, TagId = 20, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d23" },
                new ProviderTag { Id = 29, ProviderId = 6, TagId = 21, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d24" },
                new ProviderTag { Id = 30, ProviderId = 6, TagId = 22, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d25" },
                new ProviderTag { Id = 31, ProviderId = 6, TagId = 23, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d26" },
                new ProviderTag { Id = 32, ProviderId = 6, TagId = 24, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d27" },
                new ProviderTag { Id = 33, ProviderId = 7, TagId = 25, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d28" },
                new ProviderTag { Id = 34, ProviderId = 7, TagId = 26, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d29" },
                new ProviderTag { Id = 35, ProviderId = 7, TagId = 27, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d2a" },
                new ProviderTag { Id = 36, ProviderId = 7, TagId = 28, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d2b" },
                new ProviderTag { Id = 37, ProviderId = 7, TagId = 29, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d2c" },
                new ProviderTag { Id = 38, ProviderId = 7, TagId = 30, Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "21222bae-5e15-432c-ae4f-e671cb116d2d" }
                );
        }
    }
}
