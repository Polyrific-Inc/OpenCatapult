// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.EntityFrameworkCore;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Data
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly CatapultDbContext Db;

        protected BaseRepository(CatapultDbContext dbContext)
        {
            Db = dbContext;
        }

        public async Task<int> CountBySpec(ISpecification<TEntity> spec, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(Db.Set<TEntity>().AsQueryable(),
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            return await secondaryResult.CountAsync(spec.Criteria, cancellationToken);
        }

        public async Task<int> Create(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            entity.Created = DateTime.UtcNow;
            Db.Set<TEntity>().Add(entity);
            await Db.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        public async Task Delete(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var dbSet = Db.Set<TEntity>();
            var entity = await dbSet.FindAsync(id);
            dbSet.Remove(entity);
            await Db.SaveChangesAsync(cancellationToken);
        }

        public async Task<TEntity> GetById(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await Db.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetBySpec(ISpecification<TEntity> spec, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(Db.Set<TEntity>().AsQueryable(),
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            // add order by to query
            if (spec.OrderBy != null)
            {
                secondaryResult = secondaryResult.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                secondaryResult = secondaryResult.OrderByDescending(spec.OrderByDescending);
            }

            // return the result of the query using the specification's criteria expression
            return await secondaryResult
                .Where(spec.Criteria)
                .ToListAsync(cancellationToken);
        }

        public async Task<TEntity> GetSingleBySpec(ISpecification<TEntity> spec, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(Db.Set<TEntity>().AsQueryable(),
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            // add order by to query
            if (spec.OrderBy != null)
            {
                secondaryResult = secondaryResult.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                secondaryResult = secondaryResult.OrderByDescending(spec.OrderByDescending);
            }

            // return the result of the query using the specification's criteria expression
            return await secondaryResult
                .FirstOrDefaultAsync(spec.Criteria, cancellationToken);
        }

        public async Task Update(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            entity.Updated = DateTime.UtcNow;
            entity.ConcurrencyStamp = Guid.NewGuid().ToString();
            Db.Entry(entity).State = EntityState.Modified;
            await Db.SaveChangesAsync(cancellationToken);
        }
    }
}