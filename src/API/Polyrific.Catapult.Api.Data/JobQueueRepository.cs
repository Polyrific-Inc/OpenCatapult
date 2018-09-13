// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;

namespace Polyrific.Catapult.Api.Data
{
    public class JobQueueRepository : BaseRepository<JobQueue>, IJobQueueRepository
    {
        public JobQueueRepository(CatapultDbContext dbContext) : base(dbContext)
        {
        }
    }
}