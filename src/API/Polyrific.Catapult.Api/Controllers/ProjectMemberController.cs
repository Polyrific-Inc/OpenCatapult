// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Identity;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.ProjectMember;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Controllers
{
    [ApiController]
    public class ProjectMemberController : ControllerBase
    {
        private readonly IProjectMemberService _projectMemberService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ProjectMemberController(IProjectMemberService projectMemberService, IUserService userService, IMapper mapper)
        {
            _projectMemberService = projectMemberService;
            _userService = userService;
            _mapper = mapper;
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
            var projects = await _projectMemberService.GetProjectMembers(projectId, roleId);
            var results = _mapper.Map<List<ProjectMemberDto>>(projects);

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
            try
            {
                if (projectId != newProjectMember.ProjectId)
                    return BadRequest("project Id doesn't match.");

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
                        newProjectMember.ProjectMemberRoleId);
                }

                var projectMember = _mapper.Map<ProjectMemberDto>(newProjectMember);
                projectMember.Id = newProjectMemberId;
                if (newUserId > 0)
                    projectMember.UserId = newUserId;

                return CreatedAtRoute("GetProjectMemberById", new { projectId = newProjectMember.ProjectId,
                    memberId = newProjectMemberId }, projectMember);
            }
            catch (UserNotFoundException userEx)
            {
                return BadRequest(userEx.Message);
            }
            catch (DuplicateUserEmailException dupUserEx)
            {
                return BadRequest(dupUserEx.Message);
            }
            catch (ProjectNotFoundException projEx)
            {
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
            var projectMember = await _projectMemberService.GetProjectMemberById(memberId);
            var result = _mapper.Map<ProjectMemberDto>(projectMember);
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
            var projectMember = await _projectMemberService.GetProjectMemberByUserId(projectId, userId);
            var result = _mapper.Map<ProjectMemberDto>(projectMember);
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
            if (memberId != projectMember.Id)
                return BadRequest("Member Id doesn't match.");

            await _projectMemberService.UpdateProjectMemberRole(projectId, projectMember.UserId, projectMember.ProjectMemberRoleId);

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
            try
            {
                var member = await _projectMemberService.GetProjectMemberById(memberId);

                if (member != null)
                {
                    int currentUserId = 0;

                    if (!User.IsInRole(UserRole.Administrator))
                        currentUserId = User.GetUserId();

                    await _projectMemberService.RemoveProjectMember(projectId, member.UserId, currentUserId);
                }

                return NoContent();
            }
            catch (RemoveProjectOwnerException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
