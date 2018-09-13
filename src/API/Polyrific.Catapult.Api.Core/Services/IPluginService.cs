// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Services
{
    public interface IPluginService
    {
        /// <summary>
        /// Register a plugin
        /// </summary>
        /// <param name="name">Name of the plugin</param>
        /// <param name="type">Type of the plugin</param>
        /// <param name="author">Author of the plugin</param>
        /// <param name="version">Version of the plugin</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Plugin object</returns>
        Task<Plugin> AddPlugin(string name, string type, string author, string version,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete a registered plugin
        /// </summary>
        /// <param name="id">Id of the plugin</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task DeletePlugin(int id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get all registered plugin
        /// </summary>
        /// <param name="type">Type of the plugin</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task<List<Plugin>> GetPlugins(string type = "all", CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a plugin by id
        /// </summary>
        /// <param name="id">Id of the plugin</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task<Plugin> GetPluginById(int id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a plugin by name
        /// </summary>
        /// <param name="name">Name of the plugin</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task<Plugin> GetPluginByName(string name, CancellationToken cancellationToken = default(CancellationToken));
    }
}
