// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Identity;
using Polyrific.Catapult.Shared.Dto.ApplicationSetting;

namespace Polyrific.Catapult.Api.Controllers
{
    [Route("applicationsetting")]
    [ApiController]
    public class ApplicationSettingController : ControllerBase
    {
        private readonly IApplicationSettingService _applicationSettingService;
        private readonly ApplicationSettingValue _applicationSetting;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ApplicationSettingController(
            IApplicationSettingService applicationSettingService,
            IMapper mapper,
            ApplicationSettingValue applicationSetting,
            IConfiguration configuration,
            ILogger<ApplicationSettingController> logger)
        {
            _applicationSettingService = applicationSettingService;
            _configuration = configuration;
            _applicationSetting = applicationSetting;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get the application setting list
        /// </summary>
        /// <returns>List of application settings</returns>
        [HttpGet]
        [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
        public async Task<IActionResult> GetApplicationSettings()
        {
            _logger.LogRequest("Getting all application settings");

            var applicationSettings = await _applicationSettingService.GetApplicationSettings();

            var results = _mapper.Map<List<ApplicationSettingDto>>(applicationSettings);

            _logger.LogResponse("Application settings retrieved. Response body: {@results}", results);

            return Ok(results);
        }

        /// <summary>
        /// Get the application setting values
        /// </summary>
        /// <returns></returns>
        [HttpGet("value")]
        public IActionResult GetApplicationSettingValue()
        {
            _logger.LogRequest("Getting all application setting valuess");

            var result = _applicationSetting;

            _logger.LogResponse("Application settings value retrieved. Response body: {@results}", result);

            return Ok(result);
        }

        /// <summary>
        /// Update the application setting values
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
        public async Task<IActionResult> UpdateApplicationSetting(UpdateApplicationSettingDto dto)
        {
            _logger.LogRequest("Updating application setting. Request body: {@dto}", dto);

            await _applicationSettingService.UpdateApplicationSettings(dto.UpdatedSettings);
            ((IConfigurationRoot)_configuration).Reload();
            _logger.LogResponse("Aplication settings updated");

            return Ok();
        }
    }
}
