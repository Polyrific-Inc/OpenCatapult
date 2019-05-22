﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Specifications
{
    public class ApplicationSettingFilterSpecification : BaseSpecification<ApplicationSetting>
    {
        public ApplicationSettingFilterSpecification()
            : base(m => true)
        {
        }
    }
}
