// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Shared.Dto.ManagedFile;

namespace Polyrific.Catapult.Api.Controllers
{
    [Route("file")]
    [ApiController]
    [Authorize]
    public class ManagedFileController : ControllerBase
    {
        private readonly IManagedFileService _managedFileService;
        private readonly ILogger _logger;

        public ManagedFileController(IManagedFileService managedFileService, ILogger<ManagedFileController> logger)
        {
            _managedFileService = managedFileService;
            _logger = logger;
        }

        /// <summary>
        /// Create a new managed file
        /// </summary>
        /// <param name="file">The form file</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateManagedFile(IFormFile file)
        {
            var fileName = file.FileName;

            _logger.LogInformation("Creating managed file {fileName}...", fileName);

            using (var stream = file.OpenReadStream())
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);

                    var managedFileId = await _managedFileService.CreateManagedFile(fileName, ms.ToArray());

                    return Ok(new ManagedFileDto
                    {
                        Id = managedFileId,
                        FileName = fileName
                    });
                }
            }
        }

        /// <summary>
        /// Get an image file
        /// </summary>
        /// <param name="managedFileId">Id of the managed file</param>
        /// <returns></returns>
        [HttpGet("{managedFileId}/content")]
        [AllowAnonymous]
        public async Task<IActionResult> GetManagedFile(int managedFileId)
        {
            _logger.LogInformation("Getting managed file {managedFileId}...", managedFileId);

            var managedFile = await _managedFileService.GetManagedFileById(managedFileId);
            
            if (managedFile != null)
            {
                new FileExtensionContentTypeProvider().TryGetContentType(managedFile.FileName, out var contentType);
                return File(managedFile.File, contentType ?? "application/octet-stream");
            }
            else
            {
                return NoContent();
            }
        }

        /// <summary>
        /// Update a managed file
        /// </summary>
        /// <param name="managedFileId">Id of the managed file</param>
        /// <param name="file">The form file</param>
        /// <returns></returns>
        [HttpPut("{managedFileId}")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> UpdateManagedFile(int managedFileId, IFormFile file)
        {
            _logger.LogInformation("Updating managed file {managedFileId}...", managedFileId);

            using (var stream = file.OpenReadStream())
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);

                    var fileName = file.FileName;

                    await _managedFileService.UpdateManagedFile(new ManagedFile
                    {
                        Id = managedFileId,
                        FileName = fileName,
                        File = ms.ToArray()
                    });

                    return Ok();
                }
            }
        }

        /// <summary>
        /// Delete a managed file
        /// </summary>
        /// <param name="managedFileId">Id of the managed file</param>
        /// <returns></returns>
        [HttpDelete("{managedFileId}")]
        public async Task<IActionResult> DeleteManagedFile(int managedFileId)
        {
            _logger.LogInformation("Deleting managed file {managedFileId}...", managedFileId);

            await _managedFileService.DeleteManagedFile(managedFileId);

            return NoContent();
        }
    }
}
