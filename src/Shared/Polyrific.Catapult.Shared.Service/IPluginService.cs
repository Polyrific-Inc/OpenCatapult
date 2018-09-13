// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.Plugin;

namespace Polyrific.Catapult.Shared.Service
{
    public interface IPluginService
    {
        /// <summary>
        /// Register a plugin
        /// </summary>
        /// <param name="dto">DTO of the new plugin</param>
        /// <returns>Plugin DTO</returns>
        Task<PluginDto> AddPlugin(NewPluginDto dto);

        /// <summary>
        /// Delete a registered plugin
        /// </summary>
        /// <param name="id">Id of the plugin</param>
        /// <returns></returns>
        Task DeletePlugin(int id);

        /// <summary>
        /// Get all registered plugin
        /// </summary>
        /// <param name="type">Type of the plugin</param>
        /// <returns></returns>
        Task<List<PluginDto>> GetPlugins(string type = "all");

        /// <summary>
        /// Get a plugin by id
        /// </summary>
        /// <param name="id">Id of the plugin</param>
        /// <returns></returns>
        Task<PluginDto> GetPluginById(int id);

        /// <summary>
        /// Get a plugin by name
        /// </summary>
        /// <param name="name">Name of the plugin</param>
        /// <returns></returns>
        Task<PluginDto> GetPluginByName(string name);
    }
}
