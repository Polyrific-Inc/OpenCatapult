// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;

namespace Polyrific.Catapult.Api.Data
{
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        public ProjectRepository(CatapultDbContext dbContext) : base(dbContext)
        {
        }

        public ProjectRepository(CatapultSqliteDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Remove all data model properties to avoid FK conflict of "RelatedProjectDataModel"
            var properties = Db.Set<ProjectDataModelProperty>().Where(p => p.ProjectDataModel.ProjectId == id);
            Db.ProjectDataModelProperties.RemoveRange(properties);

            var dbSet = Db.Set<Project>();
            var entity = await dbSet.FindAsync(id);
            dbSet.Remove(entity);
            await Db.SaveChangesAsync(cancellationToken);
        }
    }
}
