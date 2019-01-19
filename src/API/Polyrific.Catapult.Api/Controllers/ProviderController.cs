// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Identity;
using Polyrific.Catapult.Shared.Dto.Provider;

namespace Polyrific.Catapult.Api.Controllers
{
    [Route("provider")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        private readonly IPluginService _pluginService;
        private readonly IPluginAdditionalConfigService _pluginAdditionalConfigService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ProviderController(IPluginService pluginService, IPluginAdditionalConfigService pluginAdditionalConfigService, 
            IMapper mapper, ILogger<ProviderController> logger)
        {
            _pluginService = pluginService;
            _pluginAdditionalConfigService = pluginAdditionalConfigService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all providers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
        public async Task<IActionResult> GetProviders()
        {
            _logger.LogInformation("Getting providers");

            var plugins = await _pluginService.GetPlugins();

            var result = _mapper.Map<List<ProviderDto>>(plugins);

            return Ok(result);
        }

        /// <summary>
        /// Get providers by type
        /// </summary>
        /// <param name="providerType">Type of the provider</param>
        /// <returns></returns>
        [HttpGet("type/{providerType}")]
        [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
        public async Task<IActionResult> GetProvidersByType(string providerType)
        {
            _logger.LogInformation("Getting providers for type {providerType}", providerType);

            var plugins = await _pluginService.GetPlugins(providerType);

            var result = _mapper.Map<List<ProviderDto>>(plugins);

            return Ok(result);
        }

        /// <summary>
        /// Get provider by id
        /// </summary>
        /// <param name="providerId">Id of the provider</param>
        /// <returns></returns>
        [HttpGet("{providerId}", Name = "GetProviderById")]
        [Authorize(Policy = AuthorizePolicy.UserRoleBasicAccess)]
        public async Task<IActionResult> GetProviderById(int providerId)
        {
            _logger.LogInformation("Getting provider {providerId}", providerId);

            var plugin = await _pluginService.GetPluginById(providerId);
            if (plugin == null)
                return NoContent();

            var additionalConfigs = await _pluginAdditionalConfigService.GetByPlugin(providerId);

            var result = _mapper.Map<ProviderDto>(plugin);
            result.AdditionalConfigs = _mapper.Map<ProviderAdditionalConfigDto[]>(additionalConfigs);

            return Ok(result);

        }

        /// <summary>
        /// Get provider by name
        /// </summary>
        /// <param name="providerName">Name of the provider</param>
        /// <returns></returns>
        [HttpGet("name/{providerName}", Name = "GetProviderByName")]
        [Authorize(Policy = AuthorizePolicy.UserRoleBasicAccess)]
        public async Task<IActionResult> GetProviderByName(string providerName)
        {
            _logger.LogInformation("Getting provider {providerName}", providerName);

            var plugin = await _pluginService.GetPluginByName(providerName);
            if (plugin == null)
                return NoContent();

            var additionalConfigs = await _pluginAdditionalConfigService.GetByPlugin(plugin.Id);

            var result = _mapper.Map<ProviderDto>(plugin);
            result.AdditionalConfigs = _mapper.Map<ProviderAdditionalConfigDto[]>(additionalConfigs);

            return Ok(result);

        }

        /// <summary>
        /// Register a provider
        /// </summary>
        /// <param name="dto"><see cref="NewProviderDto"/> object</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
        public async Task<IActionResult> RegisterProvider(NewProviderDto dto)
        {
            _logger.LogInformation("Registering provider. Request body: {@dto}", dto);

            try
            {
                var plugin = await _pluginService.AddPlugin(dto.Name, dto.Type, dto.Author, dto.Version, dto.RequiredServices);
                var result = _mapper.Map<ProviderDto>(plugin);

                if (dto.AdditionalConfigs != null && dto.AdditionalConfigs.Length > 0)
                {
                    var additionalConfigs = _mapper.Map<List<PluginAdditionalConfig>>(dto.AdditionalConfigs);
                    var _ = await _pluginAdditionalConfigService.AddAdditionalConfigs(plugin.Id, additionalConfigs);
                }

                return CreatedAtRoute("GetProviderById", new { providerId = plugin.Id }, result);
            }
            catch (RequiredServicesNotSupportedException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete a provider
        /// </summary>
        /// <param name="providerId">Id of the provider</param>
        /// <returns></returns>
        [HttpDelete("{providerId}")]
        [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
        public async Task<IActionResult> DeleteProviderById(int providerId)
        {
            _logger.LogInformation("Deleting provider {providerId}", providerId);

            await _pluginService.DeletePlugin(providerId);

            return NoContent();
        }

        /// <summary>
        /// Get list of additional configs of a provider
        /// </summary>
        /// <param name="providerName">Name of the provider</param>
        /// <returns></returns>
        [HttpGet("name/{providerName}/config")]
        [Authorize(Policy = AuthorizePolicy.UserRoleBasicEngineAccess)]
        public async Task<IActionResult> GetProviderAdditionalConfigsByProviderName(string providerName)
        {
            _logger.LogInformation("Getting additional configs for provider {providerName}", providerName);

            var additionalConfigs = await _pluginAdditionalConfigService.GetByPluginName(providerName);

            var result = _mapper.Map<List<ProviderAdditionalConfigDto>>(additionalConfigs);

            return Ok(result);
        }

    }
}
