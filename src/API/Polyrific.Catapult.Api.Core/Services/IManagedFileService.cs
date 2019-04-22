// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Services
{
    public interface IManagedFileService
    {
        /// <summary>
        /// Get the managed file entity by its id
        /// </summary>
        /// <param name="id">Id of the managed file</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task<ManagedFile> GetManagedFileById(int id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Create a new managed file
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <param name="file">The file to be uploaded</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task<int> CreateManagedFile(string fileName, byte[] file, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update a managed file
        /// </summary>
        /// <param name="managedFile">The managed file entity</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        Task UpdateManagedFile(ManagedFile managedFile, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete a managed file
        /// </summary>
        /// <param name="id">Id of the managed file</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        Task DeleteManagedFile(int id, CancellationToken cancellationToken = default(CancellationToken));
    }
}
