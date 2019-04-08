// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;

namespace Polyrific.Catapult.Api.Core.Services
{
    public class ManagedFileService : IManagedFileService
    {
        private readonly IManagedFileRepository _managedFileRepository;

        public ManagedFileService(IManagedFileRepository managedFileRepository)
        {
            _managedFileRepository = managedFileRepository;
        }

        public async Task<int> CreateManagedFile(string fileName, byte[] file, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _managedFileRepository.Create(new ManagedFile
            {
                FileName = fileName,
                File = file
            }, cancellationToken);
        }

        public async Task DeleteManagedFile(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _managedFileRepository.Delete(id, cancellationToken);
        }

        public async Task<ManagedFile> GetManagedFileById(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _managedFileRepository.GetById(id, cancellationToken);
        }

        public async Task UpdateManagedFile(ManagedFile managedFile, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var entity = await _managedFileRepository.GetById(managedFile.Id, cancellationToken);

            if (entity != null)
            {
                entity.FileName = managedFile.FileName;
                entity.File = managedFile.File;
                await _managedFileRepository.Update(entity, cancellationToken);
            }
            
        }
    }
}
