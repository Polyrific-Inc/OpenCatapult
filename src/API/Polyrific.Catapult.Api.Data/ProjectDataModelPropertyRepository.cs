// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Linq;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;

namespace Polyrific.Catapult.Api.Data
{
    public class ProjectDataModelPropertyRepository : BaseRepository<ProjectDataModelProperty>, IProjectDataModelPropertyRepository
    {
        public ProjectDataModelPropertyRepository(CatapultDbContext dbContext) : base(dbContext)
        {
        }

        public ProjectDataModelPropertyRepository(CatapultSqliteDbContext dbContext) : base(dbContext)
        {
        }

        public int GetMaxPropertySequence(int modelId)
        {
            return Db.ProjectDataModelProperties.Where(t => t.ProjectDataModelId == modelId).Max(t => t.Sequence) ?? 0;
        }
    }
}
