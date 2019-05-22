// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;

namespace Polyrific.Catapult.Api.Data
{
    public class ApplicationSettingRepository : BaseRepository<ApplicationSetting>, IApplicationSettingRepository
    {
        public ApplicationSettingRepository(CatapultDbContext dbContext) : base(dbContext)
        {
        }

        public ApplicationSettingRepository(CatapultSqliteDbContext dbContext) : base(dbContext)
        {
        }
    }
}
