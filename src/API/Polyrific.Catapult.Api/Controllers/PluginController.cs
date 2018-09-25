// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Identity;
using Polyrific.Catapult.Shared.Dto.Plugin;

namespace Polyrific.Catapult.Api.Controllers
{
    [Route("plugin")]
    [ApiController]
    public class PluginController : ControllerBase
    {
        private readonly IPluginService _pluginService;
        private readonly IPluginAdditionalConfigService _pluginAdditionalConfigService;
        private readonly IMapper _mapper;

        public PluginController(IPluginService pluginService, IPluginAdditionalConfigService pluginAdditionalConfigService, IMapper mapper)
        {
            _pluginService = pluginService;
            _pluginAdditionalConfigService = pluginAdditionalConfigService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all plugins
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
        public async Task<IActionResult> GetPlugins()
        {
            var plugins = await _pluginService.GetPlugins();

            var result = _mapper.Map<List<PluginDto>>(plugins);

            return Ok(result);
        }

        /// <summary>
        /// Get plugins by type
        /// </summary>
        /// <param name="pluginType">Type of the plugin</param>
        /// <returns></returns>
        [HttpGet("type/{pluginType}")]
        [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
        public async Task<IActionResult> GetPluginsByType(string pluginType)
        {
            var plugins = await _pluginService.GetPlugins(pluginType);

            var result = _mapper.Map<List<PluginDto>>(plugins);

            return Ok(result);
        }

        /// <summary>
        /// Get plugin by id
        /// </summary>
        /// <param name="pluginId">Id of the plugin</param>
        /// <returns></returns>
        [HttpGet("{pluginId}", Name = "GetPluginById")]
        [Authorize(Policy = AuthorizePolicy.UserRoleBasicAccess)]
        public async Task<IActionResult> GetPluginById(int pluginId)
        {
            var plugin = await _pluginService.GetPluginById(pluginId);
            if (plugin == null)
                return NoContent();

            var additionalConfigs = await _pluginAdditionalConfigService.GetByPlugin(pluginId);

            var result = _mapper.Map<PluginDto>(plugin);
            result.AdditionalConfigs = _mapper.Map<PluginAdditionalConfigDto[]>(additionalConfigs);

            return Ok(result);

        }

        /// <summary>
        /// Get plugin by name
        /// </summary>
        /// <param name="pluginName">Name of the plugin</param>
        /// <returns></returns>
        [HttpGet("name/{pluginName}", Name = "GetPluginByName")]
        [Authorize(Policy = AuthorizePolicy.UserRoleBasicAccess)]
        public async Task<IActionResult> GetPluginByName(string pluginName)
        {
            var plugin = await _pluginService.GetPluginByName(pluginName);
            if (plugin == null)
                return NoContent();

            var additionalConfigs = await _pluginAdditionalConfigService.GetByPlugin(plugin.Id);

            var result = _mapper.Map<PluginDto>(plugin);
            result.AdditionalConfigs = _mapper.Map<PluginAdditionalConfigDto[]>(additionalConfigs);

            return Ok(result);

        }

        /// <summary>
        /// Register a plugin
        /// </summary>
        /// <param name="dto"><see cref="NewPluginDto"/> object</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
        public async Task<IActionResult> RegisterPlugin(NewPluginDto dto)
        {
            try
            {
                var plugin = await _pluginService.AddPlugin(dto.Name, dto.Type, dto.Author, dto.Version, dto.RequiredServices);
                var result = _mapper.Map<PluginDto>(plugin);

                if (dto.AdditionalConfigs != null && dto.AdditionalConfigs.Length > 0)
                {
                    var additionalConfigs = _mapper.Map<List<PluginAdditionalConfig>>(dto.AdditionalConfigs);
                    var _ = await _pluginAdditionalConfigService.AddAdditionalConfigs(plugin.Id, additionalConfigs);
                }

                return CreatedAtRoute("GetPluginById", new { pluginId = plugin.Id }, result);
            }
            catch (RequiredServicesNotSupportedException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete a plugin
        /// </summary>
        /// <param name="pluginId">Id of the plugin</param>
        /// <returns></returns>
        [HttpDelete("{pluginId}")]
        [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
        public async Task<IActionResult> DeletePluginById(int pluginId)
        {
            await _pluginService.DeletePlugin(pluginId);

            return NoContent();
        }

        /// <summary>
        /// Get list of additional configs of a plugin
        /// </summary>
        /// <param name="pluginName">Name of the plugin</param>
        /// <returns></returns>
        [HttpGet("name/{pluginName}/config")]
        [Authorize(Policy = AuthorizePolicy.UserRoleBasicAccess)]
        public async Task<IActionResult> GetPluginAdditionalConfigsByPluginName(string pluginName)
        {
            var additionalConfigs = await _pluginAdditionalConfigService.GetByPluginName(pluginName);

            var result = _mapper.Map<List<PluginAdditionalConfigDto>>(additionalConfigs);

            return Ok(result);
        }

    }
}
