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
        private readonly ITaskProviderService _providerService;
        private readonly IMapper _mapper;
        private readonly ILogger<VersionController> _logger;

        public VersionController(ICatapultEngineService engineService, ITaskProviderService providerService,
            IMapper mapper, ILogger<VersionController> logger)
        {
            _engineService = engineService;
            _providerService = providerService;
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
            var providers = await _providerService.GetTaskProviders();
                        
            return Ok(new VersionDto
            {
                ApiVersion = apiVersion,
                Engines = _mapper.Map<List<CatapultEngineDto>>(engines),
                TaskProviders = _mapper.Map<List<TaskProviderDto>>(providers)
            });
        }
    }
}
