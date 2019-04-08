// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.ManagedFile;

namespace Polyrific.Catapult.Shared.Service
{
    public interface IManagedFileService
    {
        /// <summary>
        /// Create a new managed file
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <param name="file">The file content</param>
        /// <returns></returns>
        Task<ManagedFileDto> CreateManagedFile(string fileName, byte[] file);

        /// <summary>
        /// Update a managed file
        /// </summary>
        /// <param name="managedFileId">Id of the managed file</param>
        /// <param name="fileName">Name of the file</param>
        /// <param name="file">The file content</param>
        /// <returns></returns>
        Task UpdateManagedFile(int managedFileId, string fileName, byte[] file);
    }
}
