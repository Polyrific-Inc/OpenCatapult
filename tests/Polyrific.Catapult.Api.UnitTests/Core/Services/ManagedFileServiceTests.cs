// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Services;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Core.Services
{
    public class ManagedFileServiceTests
    {
        private readonly List<ManagedFile> _data;
        private readonly Mock<IManagedFileRepository> _ManagedFileRepository;

        public ManagedFileServiceTests()
        {
            _data = new List<ManagedFile>
            {
                new ManagedFile
                {
                    Id = 1,
                    FileName = "test.png",
                    File = new byte[1]
                }
            };

            _ManagedFileRepository = new Mock<IManagedFileRepository>();
            _ManagedFileRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                (int id, CancellationToken cancellationToken) => { return _data.FirstOrDefault(d => d.Id == id); });            
            _ManagedFileRepository.Setup(r => r.Create(It.IsAny<ManagedFile>(), It.IsAny<CancellationToken>())).ReturnsAsync(2)
                .Callback((ManagedFile entity, CancellationToken cancellationToken) =>
                {
                    entity.Id = 2;
                    _data.Add(entity);
                });
            _ManagedFileRepository.Setup(r => r.Update(It.IsAny<ManagedFile>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Callback((ManagedFile entity, CancellationToken cancellationToken) =>
                {
                    var oldEntity = _data.FirstOrDefault(d => d.Id == entity.Id);
                    if (oldEntity != null)
                    {
                        _data.Remove(oldEntity);
                        _data.Add(entity);
                    }
                });
            _ManagedFileRepository.Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Callback((int id, CancellationToken cancellationToken) =>
                {
                    var entity = _data.FirstOrDefault(d => d.Id == id);
                    if (entity != null)
                        _data.Remove(entity);
                });
        }

        [Fact]
        public async void CreateManagedFile_ValidItem()
        {
            var ManagedFileService = new ManagedFileService(_ManagedFileRepository.Object);
            var newManagedFileId = await ManagedFileService.CreateManagedFile("test2.png", new byte[1]);

            Assert.True(_data.Count > 1);
            Assert.True(newManagedFileId > 1);
        }

        [Fact]
        public async void GetManagedFileById_ReturnItem()
        {
            var ManagedFileService = new ManagedFileService(_ManagedFileRepository.Object);
            var ManagedFile = await ManagedFileService.GetManagedFileById(1);

            Assert.NotNull(ManagedFile);
            Assert.Equal(1, ManagedFile.Id);
        }

        [Fact]
        public async void UpdateManagedFile_ValidItem()
        {
            var ManagedFileService = new ManagedFileService(_ManagedFileRepository.Object);
            await ManagedFileService.UpdateManagedFile(new ManagedFile
            {
                Id = 1,
                FileName = "renamed.png"
            });

            Assert.Equal("renamed.png", _data.First(p => p.Id == 1).FileName);
        }

        [Fact]
        public async void DeleteManagedFile_ValidItem()
        {
            var ManagedFileService = new ManagedFileService(_ManagedFileRepository.Object);
            await ManagedFileService.DeleteManagedFile(1);

            Assert.Empty(_data);
        }
    }
}
