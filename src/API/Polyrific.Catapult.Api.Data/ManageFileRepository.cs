// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;

namespace Polyrific.Catapult.Api.Data
{
    public class ManagedFileRepository : BaseRepository<ManagedFile>, IManagedFileRepository
    {
        public ManagedFileRepository(CatapultDbContext dbContext) : base(dbContext)
        {
        }

        public ManagedFileRepository(CatapultSqliteDbContext dbContext) : base(dbContext)
        {
        }
    }
}
