// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Services
{
    public interface IPluginAdditionalConfigService
    {
        /// <summary>
        /// Get additional configs by plugin id
        /// </summary>
        /// <param name="pluginId">Id of the plugin</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Collection of the Plugin additional configs</returns>
        Task<List<PluginAdditionalConfig>> GetByPlugin(int pluginId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Add range of additional configs to plugin
        /// </summary>
        /// <param name="pluginId">Id of the plugin</param>
        /// <param name="additionalConfigs">Range of additional configs</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Collection of the new additional configs ids</returns>
        Task<List<int>> AddAdditionalConfigs(int pluginId, List<PluginAdditionalConfig> additionalConfigs, CancellationToken cancellationToken = default(CancellationToken));
    }
}
