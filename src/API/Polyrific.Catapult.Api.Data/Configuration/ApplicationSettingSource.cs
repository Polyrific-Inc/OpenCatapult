// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Polyrific.Catapult.Api.Core.Services;

namespace Polyrific.Catapult.Api.Data.Configuration
{
    public class ApplicationSettingSource : IConfigurationSource
    {
        private readonly Action<DbContextOptionsBuilder> _optionsAction;

        public ApplicationSettingSource(Action<DbContextOptionsBuilder> optionsAction)
        {
            _optionsAction = optionsAction;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ApplicationSettingProvider(_optionsAction);
        }
    }
}
