// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Identity;
using Polyrific.Catapult.Shared.Dto.CatapultEngine;
using Polyrific.Catapult.Shared.Dto.User;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Controllers
{
    [Route("Token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IProjectService _projectService;
        private readonly ICatapultEngineService _catapultEngineService;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public TokenController(IUserService userService, IProjectService projectService, ICatapultEngineService catapultEngineService, 
            IConfiguration configuration, ILogger<TokenController> logger)
        {
            _userService = userService;
            _projectService = projectService;
            _catapultEngineService = catapultEngineService;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Request a user token
        /// </summary>
        /// <param name="dto">Request token body</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RequestToken(RequestTokenDto dto)
        {
            _logger.LogRequest("Requesting user token for user {UserName}", dto?.UserName);

            if (!await _userService.ValidateUserPassword(dto.UserName, dto.Password))
            {
                _logger.LogWarning("Username or password is invalid. Username: {UserName}", dto?.UserName);
                return BadRequest("Username or password is invalid");
            }                

            var user = await _userService.GetUser(dto.UserName);
            if (!user.IsActive)
            {
                _logger.LogWarning("User is suspended");
                return BadRequest("User is suspended");
            }                

            var userRole = await _userService.GetUserRole(dto.UserName);
            var userProjects = await GetUserProjects(dto.UserName);
            var tokenKey = _configuration["Security:Tokens:Key"];
            var tokenIssuer = _configuration["Security:Tokens:Issuer"];
            var tokenAudience = _configuration["Security:Tokens:Audience"];

            var token = AuthorizationToken.GenerateToken(user.Id, user.UserName, user.FirstName, user.LastName, 
                userRole, userProjects, tokenKey, tokenIssuer, tokenAudience);

            _logger.LogResponse("Token for user {UserName} retrieved", dto?.UserName);

            return Ok(token);
        }

        /// <summary>
        /// Refresh a user token
        /// </summary>
        /// <returns></returns>
        [HttpGet("refresh")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            var userId = User.GetUserId();
            _logger.LogRequest("Refreshing user token for user {userId}", userId);

            var user = await _userService.GetUserById(userId);
            if (!user.IsActive)
            {
                _logger.LogWarning("User is suspended");
                return BadRequest("User is suspended");
            }

            var userRole = await _userService.GetUserRole(user.UserName);
            var userProjects = await GetUserProjects(user.UserName);
            var tokenKey = _configuration["Security:Tokens:Key"];
            var tokenIssuer = _configuration["Security:Tokens:Issuer"];
            var tokenAudience = _configuration["Security:Tokens:Audience"];

            var token = AuthorizationToken.GenerateToken(user.Id, user.UserName, user.FirstName, user.LastName,
                userRole, userProjects, tokenKey, tokenIssuer, tokenAudience);

            _logger.LogRequest("Refreshed token for user {userId} retrieved", userId);

            return Ok(token);
        }

        /// <summary>
        /// Request an engine token
        /// </summary>
        /// <param name="engineId">Id of the engine</param>
        /// <param name="dto">Request engine token body</param>
        /// <returns></returns>
        [HttpPost("engine/{engineId}")]
        [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
        public async Task<IActionResult> RequestEngineToken(int engineId, RequestEngineTokenDto dto)
        {
            _logger.LogRequest("Requesting token for engine {engineId}. Request body: {@dto}", engineId, dto);

            var engine = await _catapultEngineService.GetCatapultEngine(engineId);
            if (!engine.IsActive)
            {
                _logger.LogWarning("Engine is suspended");
                return BadRequest("Engine is suspended");
            }                

            var engineRole = await _catapultEngineService.GetCatapultEngineRole(engineId);
            var tokenKey = _configuration["Security:Tokens:Key"];
            var tokenIssuer = _configuration["Security:Tokens:Issuer"];
            var tokenAudience = _configuration["Security:Tokens:Audience"];

            var token = AuthorizationToken.GenerateEngineToken(engine.Name, engineRole, tokenKey, tokenIssuer, tokenAudience, dto?.Expires);

            _logger.LogRequest("Token for engine {engineId} retrieved", engineId);

            return Ok(token);
        }

        private async Task<List<(int, string, string)>> GetUserProjects(string userName)
        {
            var user = await _userService.GetUser(userName);
            var result = await _projectService.GetProjectsByUser(user.Id);
            return result.Select(m => (m.Item1.Id, m.Item1.Name, m.Item2?.Name)).ToList();
        }
    }
}
