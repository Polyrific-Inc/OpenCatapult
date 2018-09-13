// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Data.Identity;

namespace Polyrific.Catapult.Api.Data
{
    public class CatapultEngineProfileRepository : BaseRepository<CatapultEngineProfile>, IRepository<CatapultEngineProfile>
    {
        public CatapultEngineProfileRepository(CatapultDbContext dbContext) : base(dbContext)
        {
        }
    }
}
