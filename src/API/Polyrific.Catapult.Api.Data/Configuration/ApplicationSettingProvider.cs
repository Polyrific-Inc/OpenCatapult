// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Polyrific.Catapult.Api.Data.Configuration
{
    public class ApplicationSettingProvider : ConfigurationProvider
    {
        private readonly Action<DbContextOptionsBuilder> _options;

        public ApplicationSettingProvider(Action<DbContextOptionsBuilder> options)
        {
            _options = options;
        }

        public override void Load()
        {
            try
            {
                var builder = new DbContextOptionsBuilder<CatapultDbContext>();
                _options(builder);

                using (var context = new CatapultDbContext(builder.Options))
                {
                    var items = context.ApplicationSettings
                        .AsNoTracking()
                        .ToList();

                    Data.Clear();

                    foreach (var item in items)
                    {
                        Data.Add(item.Key, item.Value);
                    }
                }
            }
            catch (Exception)
            {
                // TODO: currently we get an error during migration because it need real db connection. Need to find a way to handle this better
            }
        }
    }
}
