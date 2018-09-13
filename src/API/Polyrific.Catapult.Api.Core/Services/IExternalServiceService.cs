// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Core.Services
{
    public interface IExternalServiceService
    {
        /// <summary>
        /// Add a new external service
        /// </summary>
        /// <param name="name">Name of the external service</param>
        /// <param name="description">Description of the external service</param>
        /// <param name="type">Type of the external service</param>
        /// <param name="configString">Configuration string of the external service</param>
        /// <param name="userId">Id of the user</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Id of the new external service</returns>
        Task<int> AddExternalService(string name, string description, string type, string configString, int userId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get the list of external services related to current user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>List of the external service entity</returns>
        Task<List<ExternalService>> GetExternalServices(int userId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a single external service
        /// </summary>
        /// <param name="id">Id of the external service</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>An external service entity</returns>
        Task<ExternalService> GetExternalService(int id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a single external service by name
        /// </summary>
        /// <param name="name">Name of the external service</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>An external service entity</returns>
        Task<ExternalService> GetExternalServiceByName(string name, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update an external service
        /// </summary>
        /// <param name="externalService">the updated external service</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task UpdateExternalService(ExternalService externalService, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete an external service
        /// </summary>
        /// <param name="id">Id of the external service</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task DeleteExternalService(int id, CancellationToken cancellationToken = default(CancellationToken));
    }
}
