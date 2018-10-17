// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Shared.Dto.ExternalServiceType;

namespace Polyrific.Catapult.Api.Controllers
{
    [Route("ServiceType")]
    [ApiController]
    public class ExternalServiceTypeController : ControllerBase
    {
        private readonly IExternalServiceTypeService _externalServiceTypeService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ExternalServiceTypeController(IExternalServiceTypeService externalServiceTypeService, IMapper mapper, ILogger<ExternalServiceTypeController> logger)
        {
            _externalServiceTypeService = externalServiceTypeService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get list of external service type that user have access to
        /// </summary>
        /// <returns>The list of external service type</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetExternalServiceTypes(bool includeProperties = false)
        {
            _logger.LogInformation("Getting external service types. Includes properties: {includeProperties}", includeProperties);

            var externalServiceTypes = await _externalServiceTypeService.GetExternalServiceTypes(includeProperties);
            var results = _mapper.Map<List<ExternalServiceTypeDto>>(externalServiceTypes);

            return Ok(results);
        }

        /// <summary>
        /// Get an external service type
        /// </summary>
        /// <param name="serviceTypeId">Id of the external service type</param>
        /// <returns>The external service type object</returns>
        [HttpGet("{serviceTypeId}", Name = "GetExternalServiceTypeById")]
        [Authorize]
        public async Task<IActionResult> GetExternalServiceType(int serviceTypeId)
        {
            _logger.LogInformation("Getting external service type {serviceTypeId}", serviceTypeId);

            var externalServiceType = await _externalServiceTypeService.GetExternalServiceType(serviceTypeId);
            var result = _mapper.Map<ExternalServiceTypeDto>(externalServiceType);
            return Ok(result);
        }

        /// <summary>
        /// Get an external service type by name
        /// </summary>
        /// <param name="serviceTypeName">Name of the external service type</param>
        /// <returns>The external service type object</returns>
        [HttpGet("name/{serviceTypeName}")]
        [Authorize]
        public async Task<IActionResult> GetExternalServiceType(string serviceTypeName)
        {
            _logger.LogInformation("Getting external service type {serviceTypeName}", serviceTypeName);

            var externalServiceType = await _externalServiceTypeService.GetExternalServiceTypeByName(serviceTypeName);
            var result = _mapper.Map<ExternalServiceTypeDto>(externalServiceType);
            return Ok(result);
        }
    }
}
