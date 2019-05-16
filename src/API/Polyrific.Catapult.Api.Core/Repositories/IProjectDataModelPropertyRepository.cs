// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Repositories
{
    public interface IProjectDataModelPropertyRepository : IRepository<ProjectDataModelProperty>
    {
        /// <summary>
        /// Get the highest property sequence in a data model
        /// </summary>
        /// <param name="modelId">The Id of the data model</param>
        /// <returns>The sequence no.</returns>
        int GetMaxPropertySequence(int modelId);
    }
}
