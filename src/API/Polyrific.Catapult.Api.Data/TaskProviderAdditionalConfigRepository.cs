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
    public class TaskProviderAdditionalConfigRepository : BaseRepository<TaskProviderAdditionalConfig>, ITaskProviderAdditionalConfigRepository
    {
        public TaskProviderAdditionalConfigRepository(CatapultDbContext dbContext) : base(dbContext)
        {
        }

        public TaskProviderAdditionalConfigRepository(CatapultSqliteDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<int>> AddRange(List<TaskProviderAdditionalConfig> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            foreach (var entity in entities)
            {
                entity.Created = DateTime.UtcNow;
            }

            Db.Set<TaskProviderAdditionalConfig>().AddRange(entities);
            await Db.SaveChangesAsync(cancellationToken);

            return entities.Select(e => e.Id).ToList();
        }
    }
}
