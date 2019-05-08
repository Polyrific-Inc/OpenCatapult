// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;

namespace Polyrific.Catapult.Api.Data
{
    public class ExternalServiceTypeRepository : BaseRepository<ExternalServiceType>, IExternalServiceTypeRepository
    {
        public ExternalServiceTypeRepository(CatapultDbContext dbContext) : base(dbContext)
        {
        }

        public ExternalServiceTypeRepository(CatapultSqliteDbContext dbContext) : base(dbContext)
        {
        }
    }
}
