// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Services
{
    public interface ITaskProviderService
    {
        /// <summary>
        /// Register a task provider
        /// </summary>
        /// <param name="name">Name of the task provider</param>
        /// <param name="type">Type of the task provider</param>
        /// <param name="author">Author of the task provider</param>
        /// <param name="version">Version of the task provider</param>
        /// <param name="requiredServices">Required services of the task provider</param>
        /// <param name="displayName">Display name of the task provider</param>
        /// <param name="description">Description of the task provider</param>
        /// <param name="thumbnailUrl">Url of the task provider thumbnail</param>
        /// <param name="tags">Tags of the task provider</param>
        /// <param name="created">Created date of the task provider</param>
        /// <param name="updated">updated date of the task provider</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Provider object</returns>
        Task<TaskProvider> AddTaskProvider(string name, string type, string author, string version, string[] requiredServices, string displayName, string description, string thumbnailUrl,
            string tags, DateTime created, DateTime? updated, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete a registered task provider
        /// </summary>
        /// <param name="id">Id of the task provider</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task DeleteTaskProvider(int id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get all registered task provider
        /// </summary>
        /// <param name="type">Type of the task provider</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task<List<TaskProvider>> GetTaskProviders(string type = "all", CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a task provider by id
        /// </summary>
        /// <param name="id">Id of the task provider</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task<TaskProvider> GetTaskProviderById(int id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a task provider by name
        /// </summary>
        /// <param name="name">Name of the task provider</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task<TaskProvider> GetTaskProviderByName(string name, CancellationToken cancellationToken = default(CancellationToken));
    }
}
