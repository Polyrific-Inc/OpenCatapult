// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;

namespace Polyrific.Catapult.Api.Data
{
    public class ExternalServiceRepository : BaseRepository<ExternalService>, IExternalServiceRepository
    {
        public ExternalServiceRepository(CatapultDbContext dbContext) : base(dbContext)
        {
        }

        public ExternalServiceRepository(CatapultSqliteDbContext dbContext) : base(dbContext)
        {
        }
    }
}
