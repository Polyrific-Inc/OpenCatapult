// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Services
{
    public interface IProviderService
    {
        /// <summary>
        /// Register a provider
        /// </summary>
        /// <param name="name">Name of the provider</param>
        /// <param name="type">Type of the provider</param>
        /// <param name="author">Author of the provider</param>
        /// <param name="version">Version of the provider</param>
        /// <param name="requiredServices">Required services of the provider</param>
        /// <param name="displayName">Display name of the provider</param>
        /// <param name="description">Description of the provider</param>
        /// <param name="thumbnailUrl">Url of the provider thumbnail</param>
        /// <param name="tags">Tags of the provider</param>
        /// <param name="created">Created date of the provider</param>
        /// <param name="updated">updated date of the provider</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Provider object</returns>
        Task<Provider> AddProvider(string name, string type, string author, string version, string[] requiredServices, string displayName, string description, string thumbnailUrl,
            string tags, DateTime created, DateTime? updated, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete a registered provider
        /// </summary>
        /// <param name="id">Id of the provider</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task DeleteProvider(int id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get all registered provider
        /// </summary>
        /// <param name="type">Type of the provider</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task<List<Provider>> GetProviders(string type = "all", CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a provider by id
        /// </summary>
        /// <param name="id">Id of the provider</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task<Provider> GetProviderById(int id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a provider by name
        /// </summary>
        /// <param name="name">Name of the provider</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task<Provider> GetProviderByName(string name, CancellationToken cancellationToken = default(CancellationToken));
    }
}
