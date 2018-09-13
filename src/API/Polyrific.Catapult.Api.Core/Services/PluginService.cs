// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Specifications;

namespace Polyrific.Catapult.Api.Core.Services
{
    public class PluginService : IPluginService
    {
        private readonly IPluginRepository _pluginRepository;

        public PluginService(IPluginRepository pluginRepository)
        {
            _pluginRepository = pluginRepository;
        }

        public async Task<Plugin> AddPlugin(string name, string type, string author, string version, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var plugin = new Plugin
            {
                Name = name,
                Type = type,
                Author = author,
                Version = version
            };

            var id = await _pluginRepository.Create(plugin, cancellationToken);

            return await _pluginRepository.GetById(id, cancellationToken);
        }

        public async Task DeletePlugin(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _pluginRepository.Delete(id, cancellationToken);
        }

        public async Task<Plugin> GetPluginById(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _pluginRepository.GetById(id, cancellationToken);
        }

        public async Task<Plugin> GetPluginByName(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var spec = new PluginFilterSpecification(name, null);

            return await _pluginRepository.GetSingleBySpec(spec, cancellationToken);
        }

        public async Task<List<Plugin>> GetPlugins(string type = "all", CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var spec = new PluginFilterSpecification(null, type != "all" ? type : null);
            var plugins = await _pluginRepository.GetBySpec(spec, cancellationToken);

            return plugins.ToList();
        }
    }
}
