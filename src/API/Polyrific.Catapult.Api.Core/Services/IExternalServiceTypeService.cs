// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Services
{
    public interface IExternalServiceTypeService
    {
        /// <summary>
        /// Get the list of external service types related to current user
        /// </summary>
        /// <param name="includeProperties">Indicate whether the properties will be included in the result</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>List of the external service type entity</returns>
        Task<List<ExternalServiceType>> GetExternalServiceTypes(bool includeProperties = false, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a single external service type
        /// </summary>
        /// <param name="id">Id of the external service type</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>An external service type entity</returns>
        Task<ExternalServiceType> GetExternalServiceType(int id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a single external service type by name
        /// </summary>
        /// <param name="name">Name of the external service type</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>An external service type entity</returns>
        Task<ExternalServiceType> GetExternalServiceTypeByName(string name, CancellationToken cancellationToken = default(CancellationToken));
    }
}
