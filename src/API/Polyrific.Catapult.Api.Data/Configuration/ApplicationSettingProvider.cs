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
        private string _dbProvider;

        public ApplicationSettingProvider(Action<DbContextOptionsBuilder> options)
        {
            _options = options;
        }

        public override void Load()
        {
            //if (string.IsNullOrEmpty(_dbProvider))
            //    _dbProvider = "mssql";

            //CatapultDbContext context;
            //if (_dbProvider.Equals("sqlite", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    var builder = new DbContextOptionsBuilder<CatapultSqliteDbContext>();
            //    _options(builder);
            //    context = new CatapultSqliteDbContext(builder.Options);
            //}
            //else
            //{
            //    var builder = new DbContextOptionsBuilder<CatapultDbContext>();
            //    _options(builder);
            //    context = new CatapultDbContext(builder.Options);
            //}

            var builder = new DbContextOptionsBuilder<CatapultDbContext>();
            _options(builder);
            using (var context = new CatapultDbContext(builder.Options))
            {
                var items = context.ApplicationSettings
                    .AsNoTracking()
                    .ToList();

                foreach (var item in items)
                {
                    Data.Add(item.Key, item.Value);
                }
            }
        }
    }
}
