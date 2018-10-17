// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Polyrific.Catapult.Api.Controllers
{
    [Route("Version")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        [Authorize]
        public IActionResult Get()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var informationalVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            
            return Ok(new {Version = informationalVersion});
        }
    }
}
