// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.Provider;

namespace Polyrific.Catapult.Shared.Service
{
    public interface IProviderService
    {
        /// <summary>
        /// Register a provider
        /// </summary>
        /// <param name="dto">DTO of the new provider</param>
        /// <returns>Provider DTO</returns>
        Task<ProviderDto> AddProvider(NewProviderDto dto);

        /// <summary>
        /// Delete a registered provider
        /// </summary>
        /// <param name="id">Id of the provider</param>
        /// <returns></returns>
        Task DeleteProvider(int id);

        /// <summary>
        /// Get all registered provider
        /// </summary>
        /// <param name="type">Type of the provider</param>
        /// <returns></returns>
        Task<List<ProviderDto>> GetProviders(string type = "all");

        /// <summary>
        /// Get a provider by id
        /// </summary>
        /// <param name="id">Id of the provider</param>
        /// <returns></returns>
        Task<ProviderDto> GetProviderById(int id);

        /// <summary>
        /// Get a provider by name
        /// </summary>
        /// <param name="name">Name of the provider</param>
        /// <returns></returns>
        Task<ProviderDto> GetProviderByName(string name);

        /// <summary>
        /// Get provider additional configs by provider name
        /// </summary>
        /// <param name="providerName">Name of the provider</param>
        /// <returns></returns>
        Task<List<ProviderAdditionalConfigDto>> GetProviderAdditionalConfigByProviderName(string providerName);
    }
}
