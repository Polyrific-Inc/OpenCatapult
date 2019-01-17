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

            // Additional configs for AspNetCoreMvc plugin
            builder.HasData(
                new PluginAdditionalConfig { Id = 1, PluginId = 1, Name = "AdminEmail", Label = "Admin Email", Type = "string", IsRequired = true, IsSecret = false, Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), ConcurrencyStamp = "c48cafcc-b3e9-4375-a2c2-f30404382258" }
            );

            // Additional configs for DotNetCoreBuildProvider plugin
            builder.HasData(
                new PluginAdditionalConfig { Id = 2, PluginId = 3, Name = "CsprojLocation", Label = "Csproj Location", Type = "string", IsRequired = false, IsSecret = false, Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), ConcurrencyStamp = "c48cafcc-b3e9-4375-a2c2-f30404382259" },
                new PluginAdditionalConfig { Id = 3, PluginId = 3, Name = "Configuration", Label = "Configuration", Type = "string", IsRequired = false, IsSecret = false, Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), ConcurrencyStamp = "c48cafcc-b3e9-4375-a2c2-f3040438225a" }
            );

            // Additional configs for EntityFrameworkCore plugin
            builder.HasData(
                new PluginAdditionalConfig { Id = 4, PluginId = 5, Name = "StartupProjectName", Label = "Startup Project Name", Type = "string", IsRequired = false, IsSecret = false, Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), ConcurrencyStamp = "c48cafcc-b3e9-4375-a2c2-f3040438225b" },
                new PluginAdditionalConfig { Id = 5, PluginId = 5, Name = "DatabaseProjectName", Label = "Database Project Name", Type = "string", IsRequired = false, IsSecret = false, Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), ConcurrencyStamp = "c48cafcc-b3e9-4375-a2c2-f3040438225c" },
                new PluginAdditionalConfig { Id = 6, PluginId = 5, Name = "ConnectionString", Label = "Connection String", Type = "string", IsRequired = true, IsSecret = true, IsInputMasked = false, Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), ConcurrencyStamp = "c48cafcc-b3e9-4375-a2c2-f30404382262" }
            );

            // Additional configs for AzureAppService plugin
            builder.HasData(
                new PluginAdditionalConfig { Id = 7, PluginId = 6, Name = "SubscriptionId", Label = "Subscription Id", Type = "string", IsRequired = true, IsSecret = false, Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), ConcurrencyStamp = "c48cafcc-b3e9-4375-a2c2-f3040438225e" },
                new PluginAdditionalConfig { Id = 8, PluginId = 6, Name = "ResourceGroupName", Label = "Resource Group", Type = "string", IsRequired = true, IsSecret = false, Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), ConcurrencyStamp = "c48cafcc-b3e9-4375-a2c2-f3040438225f" },
                new PluginAdditionalConfig { Id = 9, PluginId = 6, Name = "AppServiceName", Label = "App Service", Type = "string", IsRequired = true, IsSecret = false, Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), ConcurrencyStamp = "c48cafcc-b3e9-4375-a2c2-f30404382260" },
                new PluginAdditionalConfig { Id = 10, PluginId = 6, Name = "DeploymentSlot", Label = "Deployment Slot", Type = "string", IsRequired = false, IsSecret = false, Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), ConcurrencyStamp = "c48cafcc-b3e9-4375-a2c2-f30404382261" },
                new PluginAdditionalConfig { Id = 11, PluginId = 6, Name = "ConnectionString", Label = "Connection String", Type = "string", IsRequired = false, IsSecret = true, IsInputMasked = false, Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), ConcurrencyStamp = "c48cafcc-b3e9-4375-a2c2-f30404382263" },
                new PluginAdditionalConfig { Id = 12, PluginId = 6, Name = "Region", Label = "Default Region", Type = "string", IsRequired = false, IsSecret = false, Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), ConcurrencyStamp = "c48cafcc-b3e9-4375-a2c2-f30404382264" },
                new PluginAdditionalConfig { Id = 13, PluginId = 6, Name = "AppServicePlan", Label = "Default App Service Plan", Type = "string", IsRequired = false, IsSecret = false, Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), ConcurrencyStamp = "c48cafcc-b3e9-4375-a2c2-f30404382265" }
            );
        }
    }
}
