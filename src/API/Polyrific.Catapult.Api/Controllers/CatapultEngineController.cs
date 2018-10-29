// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Identity;
using Polyrific.Catapult.Shared.Common;
using Polyrific.Catapult.Shared.Dto.CatapultEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace Polyrific.Catapult.Api.Controllers
{
    [Route("Engine")]
    [ApiController]
    [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
    public class CatapultEngineController : ControllerBase
    {
        private readonly ICatapultEngineService _catapultEngineService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CatapultEngineController(ICatapultEngineService service, IMapper mapper, ILogger<CatapultEngineController> logger)
        {
            _catapultEngineService = service;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Register an engine
        /// </summary>
        /// <param name="dto">Register engine request body</param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterEngine(RegisterCatapultEngineDto dto)
        {
            _logger.LogInformation("Registering engine. Request body: {@dto}", dto);

            int catapultEngineId = 0;

            try
            {
                var createdCatapultEngine = await _catapultEngineService.CreateCatapultEngine(dto.Name);
                if (createdCatapultEngine != null)
                {
                    catapultEngineId = createdCatapultEngine.Id;

                    var token = await _catapultEngineService.GenerateConfirmationToken(createdCatapultEngine.Id);
                    await _catapultEngineService.ConfirmRegistration(catapultEngineId, token);
                }

                return Ok(new CatapultEngineDto
                {
                    Id = catapultEngineId,
                    Name = dto.Name
                });
            }
            catch (DuplicateCatapultEngineException nex)
            {
                _logger.LogWarning(nex, "Duplicate catapult engine name");
                return BadRequest(nex.Message);
            }
            catch (CatapultEngineCreationFailedException uex)
            {
                _logger.LogWarning(uex, "Catapult engine creation failed");
                return BadRequest(uex.GetExceptionMessageList());
            }
        }

        /// <summary>
        /// Confirm engine registration
        /// </summary>
        /// <param name="engineId">Id of the engine</param>
        /// <param name="token">Confirm registration token</param>
        /// <returns></returns>
        [HttpGet("{engineId}/Confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmRegistration(int engineId, string token)
        {
            _logger.LogInformation("Confirming engine {engineId} registration", engineId);

            await _catapultEngineService.ConfirmRegistration(engineId, token);

            return Ok("Engine confirmed.");
        }

        /// <summary>
        /// Suspend an engine
        /// </summary>
        /// <param name="engineId">Id of the engine</param>
        /// <returns></returns>
        [HttpPost("{engineId}/Suspend")]
        public async Task<IActionResult> Suspend(int engineId)
        {
            _logger.LogInformation("Suspending engine {engineId}", engineId);

            await _catapultEngineService.Suspend(engineId);

            return Ok("Engine suspended.");
        }

        /// <summary>
        /// Reactivate a suspended engine
        /// </summary>
        /// <param name="engineId">Id of the engine</param>
        /// <returns></returns>
        [HttpPost("{engineId}/Activate")]
        public async Task<IActionResult> Reactivate(int engineId)
        {
            _logger.LogInformation("Reactivating engine {engineId}", engineId);

            await _catapultEngineService.Reactivate(engineId);

            return Ok("Engine reactivated.");
        }

        /// <summary>
        /// Get the list of catapult engines
        /// </summary>
        /// <returns>List of catapult engines</returns>
        [HttpGet]
        public async Task<IActionResult> GetCatapultEngines(string status)
        {
            _logger.LogInformation("Getting engines with status = {status}", status);

            var catapultEngines = await _catapultEngineService.GetCatapultEngines(status);

            var results = _mapper.Map<List<CatapultEngineDto>>(catapultEngines);

            return Ok(results);
        }

        /// <summary>
        /// Get a catapult engines by name
        /// </summary>
        /// <param name="engineName">Name of the engine</param>
        /// <returns>Catapult engine object</returns>
        [HttpGet("name/{engineName}")]
        public async Task<IActionResult> GetCatapultEngineByName(string engineName)
        {
            _logger.LogInformation("Getting engine {engineName}", engineName);

            var catapultEngine = await _catapultEngineService.GetCatapultEngine(engineName);

            var results = _mapper.Map<CatapultEngineDto>(catapultEngine);

            return Ok(results);
        }

        /// <summary>
        /// Delete a catapult engine
        /// </summary>
        /// <param name="engineId">Id of the engine</param>
        /// <returns></returns>
        [HttpDelete("{engineId}")]
        public async Task<IActionResult> RemoveCatapultEngine(int engineId)
        {
            _logger.LogInformation("Removing engine {engineId}", engineId);

            try
            {
                await _catapultEngineService.DeleteCatapultEngine(engineId);
            }
            catch (CatapultEngineDeletionFailedException uex)
            {
                _logger.LogWarning(uex, "Catapult engine deletion failed");
                return BadRequest(uex.GetExceptionMessageList());
            }

            return NoContent();
        }
    }
}
