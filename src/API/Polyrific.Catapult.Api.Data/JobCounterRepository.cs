﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;

namespace Polyrific.Catapult.Api.Data
{
    public class JobCounterRepository : BaseRepository<JobCounter>, IJobCounterRepository
    {
        public JobCounterRepository(CatapultDbContext dbContext) : base(dbContext)
        {
        }

        public JobCounterRepository(CatapultSqliteDbContext dbContext) : base(dbContext)
        {
        }
    }
}
