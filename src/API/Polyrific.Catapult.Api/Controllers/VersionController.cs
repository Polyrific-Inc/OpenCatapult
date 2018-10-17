// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.PlatformAbstractions;

namespace Polyrific.Catapult.Api.Controllers
{
    [Route("Version")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        [Authorize]
        public IActionResult Get()
        {
            return Ok(new {Version = PlatformServices.Default.Application.ApplicationVersion});
        }
    }
}
