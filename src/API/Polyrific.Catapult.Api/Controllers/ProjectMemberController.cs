// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Identity;
using Polyrific.Catapult.Shared.Common.Notification;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.ProjectMember;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace Polyrific.Catapult.Api.Controllers
{
    [ApiController]
    public class ProjectMemberController : ControllerBase
    {
        private readonly IProjectMemberService _projectMemberService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public ProjectMemberController(
            IProjectMemberService projectMemberService,
            IMapper mapper, 
            IConfiguration configuration,
            ILogger<ProjectMemberController> logger)
        {
            _projectMemberService = projectMemberService;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Get list of project members of a project
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="roleId">Id of the project role</param>
        /// <returns>List of the project members</returns>
        [HttpGet("Project/{projectId}/member", Name = "GetProjectMembers")]
        [Authorize(Policy = AuthorizePolicy.ProjectAccess)]
        public async Task<IActionResult> GetProjectMembers(int projectId, int roleId)
        {
            _logger.LogRequest("Getting members in project {projectId} with role {roleId}", projectId, roleId);

            var projects = await _projectMemberService.GetProjectMembers(projectId, roleId);
            var results = _mapper.Map<List<ProjectMemberDto>>(projects);

            _logger.LogResponse("Members in project {projectId} retrieved. Response body: {@results}", projectId, results);

            return Ok(results);
        }

        /// <summary>
        /// Create a project member for a project
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="newProjectMember">Create project member request body</param>
        /// <returns></returns>
        [HttpPost("Project/{projectId}/member", Name = "CreateProjectMember")]
        [ProducesResponseType(201)]
        [Authorize(Policy = AuthorizePolicy.ProjectMaintainerAccess)]
        public async Task<IActionResult> CreateProjectMember(int projectId, NewProjectMemberDto newProjectMember)
        {
            _logger.LogRequest("Creating member in project {projectId}. Request body: {@newProjectMember}", projectId, newProjectMember);

            try
            {
                if (projectId != newProjectMember.ProjectId)
                {
                    _logger.LogWarning("Project Id doesn't match");
                    return BadRequest("Project Id doesn't match.");
                }

                var (newProjectMemberId, newUserId) = (0, 0);
                if (newProjectMember.UserId > 0)
                {
                    newProjectMemberId = await _projectMemberService.AddProjectMember(newProjectMember.ProjectId, 
                        newProjectMember.UserId, 
                        newProjectMember.ProjectMemberRoleId);
                }
                else
                {
                    (newProjectMemberId, newUserId) = await _projectMemberService.AddProjectMember(newProjectMember.ProjectId, 
                        newProjectMember.Email, 
                        newProjectMember.FirstName, 
                        newProjectMember.LastName, 
                        newProjectMember.ExternalAccountIds,
                        newProjectMember.ProjectMemberRoleId,
                        _configuration[ConfigurationKey.WebUrl]);
                }

                var projectMember = await _projectMemberService.GetProjectMemberById(newProjectMemberId);
                var result = _mapper.Map<ProjectMemberDto>(projectMember);
                
                _logger.LogResponse("Members in project {projectId} created. Response body: {@results}", projectId, result);

                return CreatedAtRoute("GetProjectMemberById", new { projectId = newProjectMember.ProjectId,
                    memberId = newProjectMemberId }, result);
            }
            catch (UserNotFoundException userEx)
            {
                _logger.LogWarning(userEx, "User not found");
                return BadRequest(userEx.Message);
            }
            catch (UserCreationFailedException userCreateEx)
            {
                _logger.LogWarning(userCreateEx, "User creation failed");
                return BadRequest(userCreateEx.Message);
            }
            catch (DuplicateUserEmailException dupUserEx)
            {
                _logger.LogWarning(dupUserEx, "Duplicate user email");
                return BadRequest(dupUserEx.Message);
            }
            catch (ProjectNotFoundException projEx)
            {
                _logger.LogWarning(projEx, "Project not found");
                return BadRequest(projEx.Message);
            }
        }
        
        /// <summary>
        /// Get a project member
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="memberId">Id of the project member</param>
        /// <returns></returns>
        [HttpGet("Project/{projectId}/member/{memberId}", Name = "GetProjectMemberById")]
        [Authorize(Policy = AuthorizePolicy.ProjectAccess)]
        public async Task<IActionResult> GetProjectMember(int projectId, int memberId)
        {
            _logger.LogRequest("Getting member {memberId} in project {projectId}", memberId, projectId);

            var projectMember = await _projectMemberService.GetProjectMemberById(memberId);
            var result = _mapper.Map<ProjectMemberDto>(projectMember);

            _logger.LogResponse("Member {memberId} in project {projectId} retrieved. Response body: {@result}", memberId, projectId, result);

            return Ok(result);
        }

        /// <summary>
        /// Get a project member
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="userId">Id of the user</param>
        /// <returns>Project Member entity</returns>
        [HttpGet("Project/{projectId}/member/user/{userId}", Name = "GetProjectMemberByUserId")]
        [Authorize(Policy = AuthorizePolicy.ProjectAccess)]
        public async Task<IActionResult> GetProjectMemberByUserId(int projectId, int userId)
        {
            _logger.LogRequest("Getting member for user {userId} in project {projectId}", userId, projectId);

            var projectMember = await _projectMemberService.GetProjectMemberByUserId(projectId, userId);
            var result = _mapper.Map<ProjectMemberDto>(projectMember);

            _logger.LogResponse("Member with user id {userId} in project {projectId} retrieved. Response body: {@result}", userId, projectId, result);

            return Ok(result);
        }

        /// <summary>
        /// Update a project member
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="memberId">Id of the project member</param>
        /// <param name="projectMember">Update project member request body</param>
        /// <returns></returns>
        [HttpPut("Project/{projectId}/member/{memberId}")]
        [Authorize(Policy = AuthorizePolicy.ProjectMaintainerAccess)]
        public async Task<IActionResult> UpdateProjectMember(int projectId, int memberId, UpdateProjectMemberDto projectMember)
        {
            _logger.LogRequest("Updating member {memberId} in project {projectId}. Request body: {@projectMember}", memberId, projectId, projectMember);

            if (memberId != projectMember.Id)
            {
                _logger.LogWarning("Member Id doesn't match");
                return BadRequest("Member Id doesn't match.");
            }                

            await _projectMemberService.UpdateProjectMemberRole(projectId, projectMember.UserId, projectMember.ProjectMemberRoleId);

            _logger.LogResponse("Member {memberId} in project {projectId} updated", memberId, projectId);

            return Ok();
        }

        /// <summary>
        /// Delete a project member
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="memberId">Id of the project member</param>
        /// <returns></returns>
        [HttpDelete("Project/{projectId}/member/{memberId}")]
        [Authorize(Policy = AuthorizePolicy.ProjectMaintainerAccess)]
        public async Task<IActionResult> RemoveProjectMember(int projectId, int memberId)
        {
            _logger.LogRequest("Removing member {memberId} in project {projectId}", memberId, projectId);

            try
            {
                var member = await _projectMemberService.GetProjectMemberById(memberId);

                if (member != null)
                {
                    int currentUserId = 0;

                    if (!User.IsInRole(UserRole.Administrator))
                        currentUserId = User.GetUserId();

                    _logger.LogResponse("Member {memberId} in project {projectId} removed", memberId, projectId);

                    await _projectMemberService.RemoveProjectMember(projectId, member.UserId, currentUserId);
                }

                _logger.LogResponse("Member {memberId} in project {projectId} is not found", memberId, projectId);

                return NoContent();
            }
            catch (RemoveProjectOwnerException ex)
            {
                _logger.LogWarning(ex, "Cannot remove project owner");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Project/{projectId}/member/engine")]
        [Authorize(Policy = AuthorizePolicy.UserRoleEngineAccess)]
        public async Task<IActionResult> GetProjectMembersForEngine(int projectId)
        {
            _logger.LogRequest("Getting members in project {projectId} for engine", projectId);

            var projects = await _projectMemberService.GetProjectMembers(projectId, includeUser: true);
            var results = _mapper.Map<List<ProjectMemberDto>>(projects);

            _logger.LogResponse("Members in project {projectId} retrieved. Response body: {@results}", projectId, results);

            return Ok(results);
        }
    }
}
