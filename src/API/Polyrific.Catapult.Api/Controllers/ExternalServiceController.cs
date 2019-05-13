// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Identity;
using Polyrific.Catapult.Shared.Dto.ExternalService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Controllers
{
    [Route("Service")]
    [ApiController]
    public class ExternalServiceController : ControllerBase
    {
        private readonly IExternalServiceService _externalServiceService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ExternalServiceController(IExternalServiceService externalServiceService, IMapper mapper, ILogger<ExternalServiceController> logger)
        {
            _externalServiceService = externalServiceService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get list of external service that user have access to
        /// </summary>
        /// <returns>The list of external service</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetExternalServices()
        {
            _logger.LogRequest("Getting external services");

            var currentUserId = User.GetUserId();
            var externalServices = await _externalServiceService.GetExternalServices(currentUserId);
            var results = _mapper.Map<List<ExternalServiceDto>>(externalServices);

            _logger.LogResponse("External services retrieved. Response body: {@results}", results);

            return Ok(results);
        }

        /// <summary>
        /// Get an external service
        /// </summary>
        /// <param name="serviceId">Id of the external service</param>
        /// <returns>The external service object</returns>
        [HttpGet("{serviceId}", Name = "GetExternalServiceById")]
        [Authorize]
        public async Task<IActionResult> GetExternalService(int serviceId)
        {
            _logger.LogRequest("Getting external service {serviceId}", serviceId);

            var externalService = await _externalServiceService.GetExternalService(serviceId);
            var result = _mapper.Map<ExternalServiceDto>(externalService);

            _logger.LogResponse("External service {serviceId} retrieved. Response body: {@result}", serviceId, result);

            return Ok(result);
        }

        /// <summary>
        /// Get an external service by name
        /// </summary>
        /// <param name="serviceName">Name of the external service</param>
        /// <returns>The external service object</returns>
        [HttpGet("name/{serviceName}")]
        [Authorize]
        public async Task<IActionResult> GetExternalService(string serviceName)
        {
            _logger.LogRequest("Getting external service {serviceName}", serviceName);

            var externalService = await _externalServiceService.GetExternalServiceByName(serviceName);
            var result = _mapper.Map<ExternalServiceDto>(externalService);

            _logger.LogResponse("External service {serviceName} retrieved. Response body: {@result}", serviceName, result);

            return Ok(result);
        }

        /// <summary>
        /// Create a new external service
        /// </summary>
        /// <param name="dto">The request dto</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateExternalService(CreateExternalServiceDto dto)
        {
            // exclude configs since it may contain secret values
            var requestBodyToLog = new CreateExternalServiceDto
            {
                Name = dto.Name,
                Description = dto.Description,
                ExternalServiceTypeId = dto.ExternalServiceTypeId,
                IsGlobal = dto.IsGlobal
            };
            _logger.LogRequest("Creating external service. Request body: {@requestBodyToLog}", requestBodyToLog);

            try
            {
                var currentUserId = User.GetUserId();

                var serviceId = await _externalServiceService.AddExternalService(dto.Name,
                    dto.Description,
                    dto.ExternalServiceTypeId,
                    JsonConvert.SerializeObject(dto.Config),
                    currentUserId,
                    dto.IsGlobal);

                var externalService = await _externalServiceService.GetExternalService(serviceId);
                var result = _mapper.Map<ExternalServiceDto>(externalService);

                _logger.LogResponse("External service created. Response body: {@result}", result);

                return CreatedAtRoute("GetExternalServiceById", new
                {
                    serviceId
                }, result);
            }
            catch (DuplicateExternalServiceException ex)
            {
                _logger.LogWarning(ex, "Duplicate external service name");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update an external service
        /// </summary>
        /// <param name="serviceId">Id of the external service</param>
        /// <param name="dto">The request dto</param>
        /// <returns></returns>
        [HttpPut("{serviceId}")]
        [Authorize]
        public async Task<IActionResult> UpdateExternalService(int serviceId, UpdateExternalServiceDto dto)
        {
            // exclude configs since it may contain secret values
            var requestBodyToLog = new UpdateExternalServiceDto
            {
                Description = dto.Description
            };

            _logger.LogRequest("Updating external service {serviceId}. Request body: {@requestBodyToLog}", serviceId, requestBodyToLog);

            var updatedService = _mapper.Map<ExternalService>(dto);
            updatedService.Id = serviceId;
            await _externalServiceService.UpdateExternalService(updatedService);

            _logger.LogResponse("External service {serviceId} updated", serviceId);

            return Ok();
        }

        /// <summary>
        /// Delete a job definition
        /// </summary>
        /// <param name="serviceId">Id of the external service</param>
        /// <returns></returns>
        [HttpDelete("{serviceId}")]
        [Authorize]
        public async Task<IActionResult> DeleteExternalService(int serviceId)
        {
            _logger.LogRequest("Deleting external service {serviceId}", serviceId);

            await _externalServiceService.DeleteExternalService(serviceId);

            _logger.LogResponse("External service {serviceId} deleted", serviceId);

            return NoContent();
        }
    }
}
