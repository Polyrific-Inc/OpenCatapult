// Copyright(c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;

namespace Polyrific.Catapult.Api.Data
{
    public class TagRepository : BaseRepository<Tag>, ITagRepository
    {
        public TagRepository(CatapultDbContext dbContext) : base(dbContext)
        {
        }
    }
}
