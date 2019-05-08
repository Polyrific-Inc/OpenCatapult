// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Shared.Dto.HelpContext;

namespace Polyrific.Catapult.Api.Controllers
{
    [Route("help-context")]
    [ApiController]
    public class HelpContextController : ControllerBase
    {
        private readonly IHelpContextService _helpContextService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public HelpContextController(IHelpContextService helpContextService, IMapper mapper, ILogger<ExternalServiceController> logger)
        {
            _helpContextService = helpContextService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get help contexts by section
        /// </summary>
        /// <param name="section">Section filter</param>
        /// <returns></returns>
        [HttpGet("section/{section}")]
        public async Task<IActionResult> GetHelpContextsBySection(string section)
        {
            _logger.LogRequest("Getting help contexts for section {section}", section);

            var helpContexts = await _helpContextService.GetHelpContextsBySection(section);
            var results = _mapper.Map<List<HelpContextDto>>(helpContexts);

            _logger.LogResponse("Help contexts for section {section} retrieved. Reponse body: {@results}", section, results);

            return Ok(results);
        }
    }
}
