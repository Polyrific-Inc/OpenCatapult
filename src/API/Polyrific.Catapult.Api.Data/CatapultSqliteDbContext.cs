// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.EntityFrameworkCore;

namespace Polyrific.Catapult.Api.Data
{
    public class CatapultSqliteDbContext : CatapultDbContext
    {
        public CatapultSqliteDbContext(DbContextOptions<CatapultSqliteDbContext> options) : base(options)
        {
        }
    }
}
