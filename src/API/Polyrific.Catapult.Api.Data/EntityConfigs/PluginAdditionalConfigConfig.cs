// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Data.EntityConfigs
{
    public class PluginAdditionalConfigConfig : BaseEntityConfig<PluginAdditionalConfig>
    {
        public override void Configure(EntityTypeBuilder<PluginAdditionalConfig> builder)
        {
            base.Configure(builder);

            builder.HasData(
                new PluginAdditionalConfig { Id = 1, PluginId = 1, Name = "ConnectionString", Label = "Connection String", Type = "string", IsRequired = true, IsSecret = true, Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), ConcurrencyStamp = "c48cafcc-b3e9-4375-a2c2-f30404382258" },

                new PluginAdditionalConfig { Id = 2, PluginId = 3, Name = "CsprojLocation", Label = "Csproj Location", Type = "string", IsRequired = false, IsSecret = false, Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), ConcurrencyStamp = "c48cafcc-b3e9-4375-a2c2-f30404382259" },
                new PluginAdditionalConfig { Id = 3, PluginId = 3, Name = "Configuration", Label = "Configuration", Type = "string", IsRequired = false, IsSecret = false, Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), ConcurrencyStamp = "c48cafcc-b3e9-4375-a2c2-f3040438225a" },

                new PluginAdditionalConfig { Id = 4, PluginId = 5, Name = "StartupProjectName", Label = "Startup Project Name", Type = "string", IsRequired = false, IsSecret = false, Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), ConcurrencyStamp = "c48cafcc-b3e9-4375-a2c2-f3040438225b" },
                new PluginAdditionalConfig { Id = 5, PluginId = 5, Name = "DatabaseProjectName", Label = "Database Project Name", Type = "string", IsRequired = false, IsSecret = false, Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), ConcurrencyStamp = "c48cafcc-b3e9-4375-a2c2-f3040438225c" },
                new PluginAdditionalConfig { Id = 6, PluginId = 5, Name = "ConnectionString", Label = "Connection String", Type = "string", IsRequired = true, IsSecret = true, Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), ConcurrencyStamp = "c48cafcc-b3e9-4375-a2c2-f30404382262" },
                new PluginAdditionalConfig { Id = 7, PluginId = 5, Name = "Configuration", Label = "Configuration", Type = "string", IsRequired = false, IsSecret = false, Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), ConcurrencyStamp = "c48cafcc-b3e9-4375-a2c2-f3040438225d" },

                new PluginAdditionalConfig { Id = 8, PluginId = 6, Name = "SubscriptionId", Label = "Subscription Id", Type = "string", IsRequired = true, IsSecret = false, Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), ConcurrencyStamp = "c48cafcc-b3e9-4375-a2c2-f3040438225e" },
                new PluginAdditionalConfig { Id = 9, PluginId = 6, Name = "ResourceGroupName", Label = "Resource Group", Type = "string", IsRequired = true, IsSecret = false, Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), ConcurrencyStamp = "c48cafcc-b3e9-4375-a2c2-f3040438225f" },
                new PluginAdditionalConfig { Id = 10, PluginId = 6, Name = "AppServiceName", Label = "App Service", Type = "string", IsRequired = true, IsSecret = false, Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), ConcurrencyStamp = "c48cafcc-b3e9-4375-a2c2-f30404382260" },
                new PluginAdditionalConfig { Id = 11, PluginId = 6, Name = "DeploymentSlot", Label = "Deployment Slot", Type = "string", IsRequired = false, IsSecret = false, Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), ConcurrencyStamp = "c48cafcc-b3e9-4375-a2c2-f30404382261" }
            );
        }
    }
}
