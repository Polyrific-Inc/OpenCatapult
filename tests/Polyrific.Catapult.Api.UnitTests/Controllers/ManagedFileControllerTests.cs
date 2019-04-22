// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Api.Controllers;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.UnitTests.Utilities;
using Polyrific.Catapult.Shared.Dto.ManagedFile;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Controllers
{
    public class ManagedFileControllerTests
    {
        private readonly Mock<IManagedFileService> _managedFileService;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ManagedFileController>> _logger;
        private readonly Mock<IFormFile> _file;

        public ManagedFileControllerTests()
        {
            _managedFileService = new Mock<IManagedFileService>();

            _mapper = AutoMapperUtils.GetMapper();

            _logger = LoggerMock.GetLogger<ManagedFileController>();

            _file = new Mock<IFormFile>();
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            _file.Setup(_ => _.OpenReadStream()).Returns(ms);
            _file.Setup(_ => _.FileName).Returns(fileName);
            _file.Setup(_ => _.Length).Returns(ms.Length);
        }

        [Fact]
        public async void GetManagedFile_ReturnsFile()
        {
            _managedFileService.Setup(s => s.GetManagedFileById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken token) => new ManagedFile
                {
                    Id = id,
                    FileName = "test.png",
                    File = new byte[1]
                });

            var controller = new ManagedFileController(_managedFileService.Object, _logger.Object);

            var result = await controller.GetManagedFile(1);

            var fileActionResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("image/png", fileActionResult.ContentType);
            Assert.Single(fileActionResult.FileContents);
        }

        [Fact]
        public async void CreateManagedFile_ReturnsCreatedManagedFile()
        {
            _managedFileService
                .Setup(s => s.CreateManagedFile(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var controller = new ManagedFileController(_managedFileService.Object, _logger.Object);

            var result = await controller.CreateManagedFile(_file.Object);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ManagedFileDto>(okActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void UpdateManagedFile_ReturnsSuccess()
        {
            _managedFileService.Setup(s => s.UpdateManagedFile(It.IsAny<ManagedFile>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var controller = new ManagedFileController(_managedFileService.Object, _logger.Object);

            var result = await controller.UpdateManagedFile(1, _file.Object);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void DeleteManagedFile_ReturnsNoContent()
        {
            _managedFileService.Setup(s => s.DeleteManagedFile(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var controller = new ManagedFileController(_managedFileService.Object, _logger.Object);

            var result = await controller.DeleteManagedFile(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
