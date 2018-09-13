// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Polyrific.Catapult.Api.Controllers
{
    [Route("Health")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Check whether the engine API is healthy
        /// </summary>
        /// <returns></returns>
        [HttpHead]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Healthy");
        }

        /// <summary>
        /// Check whether the engine API is healthy when user is authenticated
        /// </summary>
        /// <returns></returns>
        [HttpHead("Secure")]
        [HttpGet("Secure")]
        [Authorize]
        public IActionResult GetSecure()
        {
            return Ok("Healthy");
        }
    }
}