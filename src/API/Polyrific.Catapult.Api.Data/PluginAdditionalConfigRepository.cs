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
    public class PluginAdditionalConfigRepository : BaseRepository<PluginAdditionalConfig>, IPluginAdditionalConfigRepository
    {
        public PluginAdditionalConfigRepository(CatapultDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<int>> AddRange(List<PluginAdditionalConfig> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            foreach (var entity in entities)
            {
                entity.Created = DateTime.UtcNow;
            }

            Db.Set<PluginAdditionalConfig>().AddRange(entities);
            await Db.SaveChangesAsync(cancellationToken);

            return entities.Select(e => e.Id).ToList();
        }
    }
}
