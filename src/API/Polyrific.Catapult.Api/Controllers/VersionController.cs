// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Shared.Dto.CatapultEngine;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.Provider;
using Polyrific.Catapult.Shared.Dto.Version;

namespace Polyrific.Catapult.Api.Controllers
{
    [Route("Version")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private readonly ICatapultEngineService _engineService;
        private readonly IPluginService _pluginService;
        private readonly IMapper _mapper;
        private readonly ILogger<VersionController> _logger;

        public VersionController(ICatapultEngineService engineService, IPluginService pluginService,
            IMapper mapper, ILogger<VersionController> logger)
        {
            _engineService = engineService;
            _pluginService = pluginService;
            _mapper = mapper;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogDebug("Checking versions");

            var assembly = Assembly.GetExecutingAssembly();
            var apiVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

            var engines = await _engineService.GetCatapultEngines(EngineStatus.All);
            var providers = await _pluginService.GetPlugins();
                        
            return Ok(new VersionDto
            {
                ApiVersion = apiVersion,
                Engines = _mapper.Map<List<CatapultEngineDto>>(engines),
                Providers = _mapper.Map<List<ProviderDto>>(providers)
            });
        }
    }
}
