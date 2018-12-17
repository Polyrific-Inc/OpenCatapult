// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Core.Repositories
{
    public interface IJobTaskDefinitionRepository : IRepository<JobTaskDefinition>
    {
        /// <summary>
        /// Create a range of job task definitions
        /// </summary>
        /// <param name="entities">A range of job task definitions</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task<List<int>> CreateRange(List<JobTaskDefinition> entities, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get the highest task sequence in a job definition
        /// </summary>
        /// <param name="jobDefinitionId">The Id of the job definition</param>
        /// <returns>The sequence no.</returns>
        int GetMaxTaskSequence(int jobDefinitionId);
    }
}
