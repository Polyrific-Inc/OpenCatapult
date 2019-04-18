// Copyright (c) Polyrific, Inc 2018. All rights reserved.

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
        [HttpGet("help-context/{section}")]
        [Authorize]
        public async Task<IActionResult> GetHelpContextsBySection(int section)
        {
            _logger.LogInformation("Getting help contexts for section {section}", section);

            var externalService = await _helpContextService.GetHelpContextsBySection(section);
            var result = _mapper.Map<HelpContextDto>(externalService);
            return Ok(result);
        }
    }
}
