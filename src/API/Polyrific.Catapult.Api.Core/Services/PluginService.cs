// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
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
        private readonly ITagRepository _tagRepository;

        public PluginService(IPluginRepository pluginRepository, IExternalServiceTypeRepository externalServiceTypeRepository, ITagRepository tagRepository)
        {
            _pluginRepository = pluginRepository;
            _externalServiceTypeRepository = externalServiceTypeRepository;
            _tagRepository = tagRepository;
        }

        public async Task<Plugin> AddPlugin(string name, string type, string author, string version, string[] requiredServices, string displayName, string description, string thumbnailUrl,
            string tags, DateTime created, DateTime? updated, CancellationToken cancellationToken = default(CancellationToken))
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

            List<Tag> tagList = null;

            if (!string.IsNullOrEmpty(tags))
            {
                var tagArray = tags.Split(DataDelimiter.Comma);

                var tagSpec = new TagFilterSpecification(tagArray);
                tagList = (await _tagRepository.GetBySpec(tagSpec, cancellationToken)).ToList();

                var newTags = tagArray.Where(tag => tagList.Any(t => tag.ToLower() == t.Name.ToLower()));
                foreach (var newTagName in newTags)
                {
                    var newTagId = await _tagRepository.Create(new Tag
                    {
                        Name = newTagName
                    }, cancellationToken);

                    tagList.Add(new Tag
                    {
                        Id = newTagId,
                        Name = newTagName
                    });
                }
            }
            
            var plugin = new Plugin
            {
                Name = name,
                DisplayName = displayName,
                Description = description,
                ThumbnailUrl = thumbnailUrl,
                Type = type,
                Author = author,
                Version = version,
                RequiredServicesString = requiredServicesString,
                Created = created > DateTime.MinValue ? created : DateTime.UtcNow,
                Updated = updated,
                Tags = tagList?.Select(t => new PluginTag
                {
                    TagId = t.Id
                }).ToList()
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
