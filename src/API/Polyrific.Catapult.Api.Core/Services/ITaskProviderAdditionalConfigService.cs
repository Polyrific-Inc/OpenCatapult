// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Services
{
    public interface ITaskProviderAdditionalConfigService
    {
        /// <summary>
        /// Get additional configs by task provider id
        /// </summary>
        /// <param name="taskProviderId">Id of the task provider</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Collection of the Provider additional configs</returns>
        Task<List<TaskProviderAdditionalConfig>> GetByTaskProvider(int taskProviderId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get additional configs by provider name
        /// </summary>
        /// <param name="taskProviderName">Name of the task provider</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Collection of the Provider additional configs</returns>
        Task<List<TaskProviderAdditionalConfig>> GetByTaskProviderName(string taskProviderName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Add range of additional configs to provider
        /// </summary>
        /// <param name="taskProviderId">Id of the task provider</param>
        /// <param name="additionalConfigs">Range of additional configs</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Collection of the new additional configs ids</returns>
        Task<List<int>> AddAdditionalConfigs(int taskProviderId, List<TaskProviderAdditionalConfig> additionalConfigs, CancellationToken cancellationToken = default(CancellationToken));
    }
}
