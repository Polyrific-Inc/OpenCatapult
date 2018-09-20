// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.ExternalServiceType;

namespace Polyrific.Catapult.Shared.Service
{
    public interface IExternalServiceTypeService
    {
        /// <summary>
        /// Get the list of external service types related to current user
        /// </summary>
        /// <returns>List of the external service type entity</returns>
        Task<List<ExternalServiceTypeDto>> GetExternalServiceTypes();

        /// <summary>
        /// Get a single external service type
        /// </summary>
        /// <param name="id">Id of the external service type</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>An external service type entity</returns>
        Task<ExternalServiceTypeDto> GetExternalServiceType(int id);

        /// <summary>
        /// Get a single external service type by name
        /// </summary>
        /// <param name="name">Name of the external service type</param>
        /// <returns>An external service type entity</returns>
        Task<ExternalServiceTypeDto> GetExternalServiceTypeByName(string name);
    }
}
