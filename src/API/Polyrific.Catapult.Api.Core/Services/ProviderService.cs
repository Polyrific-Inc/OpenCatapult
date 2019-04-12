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
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IExternalServiceTypeRepository _externalServiceTypeRepository;
        private readonly ITagRepository _tagRepository;

        public ProviderService(IProviderRepository providerRepository, IExternalServiceTypeRepository externalServiceTypeRepository, ITagRepository tagRepository)
        {
            _providerRepository = providerRepository;
            _externalServiceTypeRepository = externalServiceTypeRepository;
            _tagRepository = tagRepository;
        }

        public async Task<Provider> AddProvider(string name, string type, string author, string version, string[] requiredServices, string displayName, string description, string thumbnailUrl,
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

                var newTags = tagArray.Where(tag => !tagList.Any(t => tag.ToLower() == t.Name.ToLower()));
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
            
            var provider = new Provider
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
                Tags = tagList?.Select(t => new ProviderTag
                {
                    TagId = t.Id
                }).ToList()
            };

            var id = await _providerRepository.Create(provider, cancellationToken);

            return await _providerRepository.GetById(id, cancellationToken);
        }

        public async Task DeleteProvider(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _providerRepository.Delete(id, cancellationToken);
        }

        public async Task<Provider> GetProviderById(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var spec = new ProviderFilterSpecification(id);
            spec.IncludeStrings.Add("Tags.Tag");
            return await _providerRepository.GetSingleBySpec(spec, cancellationToken);
        }

        public async Task<Provider> GetProviderByName(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var spec = new ProviderFilterSpecification(name, null);
            spec.IncludeStrings.Add("Tags.Tag");

            return await _providerRepository.GetSingleBySpec(spec, cancellationToken);
        }

        public async Task<List<Provider>> GetProviders(string type = "all", CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var spec = new ProviderFilterSpecification(null, type != "all" ? type : null);
            spec.IncludeStrings.Add("Tags.Tag");
            var providers = await _providerRepository.GetBySpec(spec, cancellationToken);

            return providers.ToList();
        }
    }
}
