// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Polyrific.Catapult.Api.Controllers
{
    [Route("Health")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly ILogger _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Check whether the engine API is healthy
        /// </summary>
        /// <returns></returns>
        [HttpHead]
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Checking API health");

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
            _logger.LogInformation("Checking secured API health");

            return Ok("Healthy");
        }
    }
}
