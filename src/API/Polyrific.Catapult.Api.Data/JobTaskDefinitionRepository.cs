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
    public class JobTaskDefinitionRepository : BaseRepository<JobTaskDefinition>, IJobTaskDefinitionRepository
    {
        public JobTaskDefinitionRepository(CatapultDbContext dbContext) : base(dbContext)
        {
        }

        public JobTaskDefinitionRepository(CatapultSqliteDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<int>> CreateRange(List<JobTaskDefinition> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            foreach (var entity in entities)
            {
                entity.Created = DateTime.UtcNow;
            }

            Db.Set<JobTaskDefinition>().AddRange(entities);
            await Db.SaveChangesAsync(cancellationToken);

            return entities.Select(e => e.Id).ToList();
        }

        public int GetMaxTaskSequence(int jobDefinitionId)
        {
            return Db.JobTaskDefinitions.Where(t => t.JobDefinitionId == jobDefinitionId).Max(t => t.Sequence) ?? 0;
        }
    }
}
