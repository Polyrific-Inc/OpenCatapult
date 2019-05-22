// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.Constants;

namespace Polyrific.Catapult.Api.Data.EntityConfigs
{
    public class ApplicationSettingConfig : BaseEntityConfig<ApplicationSetting>
    {
        public override void Configure(EntityTypeBuilder<ApplicationSetting> builder)
        {
            base.Configure(builder);

            builder.HasData(new ApplicationSetting {
                Id = 1, Key = ApplicationSettings.EnableTwoFactorAuth,
                Label = "Enable two factor authentication?",
                Value = "false", DataType = ApplicationSettingDataTypes.Bool,
                Created = new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc),
                ConcurrencyStamp = "504200ee-f48a-4efa-be48-e09d16ee8d65" });
        }
    }
}
