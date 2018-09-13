// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.ExternalService;

namespace Polyrific.Catapult.Shared.Service
{
    public interface IExternalServiceService
    {
        /// <summary>
        /// Create a new external service
        /// </summary>
        /// <param name="dto">The create external service dto</param>
        /// <returns>The created external service</returns>
        Task<ExternalServiceDto> CreateExternalService(CreateExternalServiceDto dto);

        /// <summary>
        /// Get the list of external services related to current user
        /// </summary>
        /// <returns>List of the external service dto</returns>
        Task<List<ExternalServiceDto>> GetExternalServices();

        /// <summary>
        /// Get a single external service
        /// </summary>
        /// <param name="serviceId">Id of the external service</param>        
        /// <returns>An external service dto</returns>
        Task<ExternalServiceDto> GetExternalService(int serviceId);

        /// <summary>
        /// Get a single external service by name
        /// </summary>
        /// <param name="name">Name of the external service</param>        
        /// <returns>An external service dto</returns>
        Task<ExternalServiceDto> GetExternalServiceByName(string name);

        /// <summary>
        /// Update an external service
        /// </summary>
        /// <param name="serviceId">Id of the external service</param>
        /// <param name="dto">the updated external service</param>        
        /// <returns></returns>
        Task UpdateExternalService(int serviceId, UpdateExternalServiceDto dto);

        /// <summary>
        /// Delete an external service
        /// </summary>
        /// <param name="serviceId">Id of the external service</param>        
        /// <returns></returns>
        Task DeleteExternalService(int serviceId);
    }
}
