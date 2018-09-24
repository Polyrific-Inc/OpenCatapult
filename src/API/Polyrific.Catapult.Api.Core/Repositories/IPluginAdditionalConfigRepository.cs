// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Repositories
{
    public interface IPluginAdditionalConfigRepository : IRepository<PluginAdditionalConfig>
    {
        /// <summary>
        /// Add range of plugin additional configs
        /// </summary>
        /// <param name="entities">Additional configs</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task<List<int>> AddRange(List<PluginAdditionalConfig> entities, CancellationToken cancellationToken = default(CancellationToken));
    }
}
