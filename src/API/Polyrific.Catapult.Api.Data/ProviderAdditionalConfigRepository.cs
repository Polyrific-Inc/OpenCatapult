// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;

namespace Polyrific.Catapult.Api.Data
{
    public class ProviderAdditionalConfigRepository : BaseRepository<ProviderAdditionalConfig>, IProviderAdditionalConfigRepository
    {
        public ProviderAdditionalConfigRepository(CatapultDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<int>> AddRange(List<ProviderAdditionalConfig> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            foreach (var entity in entities)
            {
                entity.Created = DateTime.UtcNow;
            }

            Db.Set<ProviderAdditionalConfig>().AddRange(entities);
            await Db.SaveChangesAsync(cancellationToken);

            return entities.Select(e => e.Id).ToList();
        }
    }
}
