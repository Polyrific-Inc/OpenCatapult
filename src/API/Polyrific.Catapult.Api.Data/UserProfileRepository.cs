// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Data.Identity;

namespace Polyrific.Catapult.Api.Data
{
    public class UserProfileRepository : BaseRepository<UserProfile>, IRepository<UserProfile>
    {
        public UserProfileRepository(CatapultDbContext dbContext) : base(dbContext)
        {
        }

        public UserProfileRepository(CatapultSqliteDbContext dbContext) : base(dbContext)
        {
        }
    }
}
