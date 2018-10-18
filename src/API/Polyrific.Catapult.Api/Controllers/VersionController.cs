// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Polyrific.Catapult.Api.Controllers
{
    [Route("Version")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private readonly ILogger<VersionController> _logger;

        public VersionController(ILogger<VersionController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public IActionResult Get()
        {
            _logger.LogDebug("Checking versions");

            var assembly = Assembly.GetExecutingAssembly();
            var informationalVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            
            return Ok(new {Version = informationalVersion});
        }
    }
}
