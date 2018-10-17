// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Specifications;
using Polyrific.Catapult.Shared.Dto.Constants;

namespace Polyrific.Catapult.Api.Core.Services
{
    public class PluginService : IPluginService
    {
        private readonly IPluginRepository _pluginRepository;
        private readonly IExternalServiceTypeRepository _externalServiceTypeRepository;

        public PluginService(IPluginRepository pluginRepository, IExternalServiceTypeRepository externalServiceTypeRepository)
        {
            _pluginRepository = pluginRepository;
            _externalServiceTypeRepository = externalServiceTypeRepository;
        }

        public async Task<Plugin> AddPlugin(string name, string type, string author, string version, string[] requiredServices, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<ExternalServiceType> serviceTypes = null;
            string requiredServicesString = null;
            if (requiredServices != null && requiredServices.Length > 0)
            {
                requiredServicesString = string.Join(DataDelimiter.Comma.ToString(), requiredServices);
                var serviceTypeSpec = new ExternalServiceTypeFilterSpecification(requiredServices);
                serviceTypes = (await _externalServiceTypeRepository.GetBySpec(serviceTypeSpec, cancellationToken)).ToList();

                var notSupportedServices = requiredServices.Where(s => !serviceTypes.Any(t => t.Name == s)).ToArray();

                if (notSupportedServices.Length > 0)
                {
                    throw new RequiredServicesNotSupportedException(notSupportedServices);
                }
            }

            var plugin = new Plugin
            {
                Name = name,
                Type = type,
                Author = author,
                Version = version,
                RequiredServicesString = requiredServicesString
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
