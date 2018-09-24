// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Specifications;

namespace Polyrific.Catapult.Api.Core.Services
{
    public class PluginAdditionalConfigService : IPluginAdditionalConfigService
    {
        private readonly IPluginRepository _pluginRepository;
        private readonly IPluginAdditionalConfigRepository _pluginAdditionalConfigRepository;

        public PluginAdditionalConfigService(IPluginRepository pluginRepository, IPluginAdditionalConfigRepository pluginAdditionalConfigRepository)
        {
            _pluginRepository = pluginRepository;
            _pluginAdditionalConfigRepository = pluginAdditionalConfigRepository;
        }

        public async Task<List<int>> AddAdditionalConfigs(int pluginId, List<PluginAdditionalConfig> additionalConfigs, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var plugin = await _pluginRepository.GetById(pluginId, cancellationToken);
            if (plugin == null)
            {
                throw new PluginNotFoundException(pluginId);
            }
            
            additionalConfigs.ForEach(j => j.PluginId = pluginId);

            return await _pluginAdditionalConfigRepository.AddRange(additionalConfigs, cancellationToken);
        }

        public async Task<List<PluginAdditionalConfig>> GetByPlugin(int pluginId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var spec = new PluginAdditionalConfigFilterSpecification(pluginId);
            var result = await _pluginAdditionalConfigRepository.GetBySpec(spec, cancellationToken);

            return result.ToList();
        }
    }
}
