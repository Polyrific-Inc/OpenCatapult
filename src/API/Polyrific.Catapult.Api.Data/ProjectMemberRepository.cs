// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Specifications;
using Polyrific.Catapult.Api.Data.Identity;

namespace Polyrific.Catapult.Api.Data
{
    public class ProjectMemberRepository : BaseRepository<ProjectMember>, IProjectMemberRepository
    {
        private readonly IMapper _mapper;

        public ProjectMemberRepository(CatapultDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }

        public override async Task<IEnumerable<ProjectMember>> GetBySpec(ISpecification<ProjectMember> spec, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes.Where(p => p.Body.Type != typeof(User))
                .Aggregate(Db.Set<ProjectMember>().AsQueryable(),
                    (current, include) => current.Include(include));
            
            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings.Where(i => i != "User")
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

            var result = await secondaryResult
                .Where(spec.Criteria)
                .ToListAsync(cancellationToken);
            
            var userInclude = spec.Includes.FirstOrDefault(p => p.Body.Type == typeof(User));
            var userIncludeString = spec.IncludeStrings.FirstOrDefault(i => i == "User");

            var userIds = result.Select(m => m.UserId).Distinct().ToArray();
            if (userInclude != null || userIncludeString != null)
            {
                var users = _mapper.Map<List<User>>(await Db.Set<ApplicationUser>().Where(u => userIds.Contains(u.Id)).ToListAsync());
                foreach (var item in result)
                    item.User = users.FirstOrDefault(u => u.Id == item.UserId);
            }

            return result;
        }

        public override async Task<ProjectMember> GetSingleBySpec(ISpecification<ProjectMember> spec, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes.Where(p => p.Body.Type != typeof(User))
                .Aggregate(Db.Set<ProjectMember>().AsQueryable(),
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings.Where(i => i != "User")
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

            var result = await secondaryResult
                .FirstOrDefaultAsync(spec.Criteria, cancellationToken);
            
            if (result != null)
            {
                var userInclude = spec.Includes.FirstOrDefault(p => p.Body.Type == typeof(User));
                var userIncludeString = spec.IncludeStrings.FirstOrDefault(i => i == "User");

                var userId = result.UserId;
                if (userInclude != null || userIncludeString != null)
                {
                    result.User = _mapper.Map<User>(await Db.Set<ApplicationUser>().FindAsync(userId));
                }
            }

            return result;
        }
    }
}
