// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;

namespace Polyrific.Catapult.Api.Data
{
    public class TaskProviderRepository : BaseRepository<TaskProvider>, ITaskProviderRepository
    {
        public TaskProviderRepository(CatapultDbContext dbContext) : base(dbContext)
        {
        }

        public TaskProviderRepository(CatapultSqliteDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<int> Create(TaskProvider entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            entity.Created = entity.Created == DateTime.MinValue ? DateTime.UtcNow : entity.Created;
            Db.Set<TaskProvider>().Add(entity);
            await Db.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
