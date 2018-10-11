// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Identity;
using Polyrific.Catapult.Shared.Dto.Project;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Controllers
{
    [Route("Project")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ProjectController(IProjectService projectService, IUserService userService, IMapper mapper)
        {
            _projectService = projectService;
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get project list that the user authorized to view
        /// </summary>
        /// <param name="status">Status of the project (all | active | archived)</param>
        /// <returns>List of the project</returns>
        [HttpGet]
        [Authorize(Policy = AuthorizePolicy.ProjectMemberAccess)]
        public async Task<IActionResult> GetProjects(string status = null)
        {
            try
            {
                var currentUserId = User.GetUserId();
                var projects = await _projectService.GetProjectsByUser(currentUserId, status);
                var results = _mapper.Map<List<ProjectDto>>(projects.Select(p => p.Item1));

                return Ok(results);
            }
            catch (FilterTypeNotFoundException ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("name/{projectName}")]
        [Authorize(Policy = AuthorizePolicy.ProjectAccess)]
        public async Task<IActionResult> GetProjectByName(string projectName)
        {
            var project = await _projectService.GetProjectByName(projectName);
            var result = _mapper.Map<ProjectDto>(project);
            return Ok(result);
        }

        /// <summary>
        /// Get a project by id
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <returns>The project object</returns>
        [HttpGet("{projectId}", Name = "GetProjectById")]
        [Authorize(Policy = AuthorizePolicy.ProjectAccess)]
        public async Task<IActionResult> GetProject(int projectId)
        {
            var project = await _projectService.GetProjectById(projectId);
            var result = _mapper.Map<ProjectDto>(project);
            return Ok(result);
        }

        /// <summary>
        /// Create a project
        /// </summary>
        /// <param name="newProject">Create project request body</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = AuthorizePolicy.UserRoleBasicAccess)]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateProject(NewProjectDto newProject)
        {
            try
            {
                var projectMembers = newProject.Members.Select(m => (m.UserId, m.ProjectMemberRoleId)).ToList();
                var currentUserId = User.GetUserId();

                List<ProjectDataModel> models = null;
                List<JobDefinition> jobs = null;

                if (newProject.Models != null)
                    models = _mapper.Map<List<ProjectDataModel>>(newProject.Models);
                
                if (newProject.Jobs != null)
                    jobs = _mapper.Map<List<JobDefinition>>(newProject.Jobs);

                var createdProject = await _projectService.CreateProject(newProject.Name, newProject.Client, projectMembers, newProject.Config, models, jobs, currentUserId);
                var project = _mapper.Map<ProjectDto>(createdProject);
                return CreatedAtRoute("GetProjectById", new { projectId = project.Id }, project);
                
            }
            catch (DuplicateProjectException dupEx)
            {
                return BadRequest(dupEx.Message);
            }
            catch (ProjectDataModelNotFoundException modelEx)
            {
                return BadRequest(modelEx.Message);
            }
            catch (ProviderNotInstalledException provEx)
            {
                return BadRequest(provEx.Message);
            }
            catch (ExternalServiceRequiredException esrEx)
            {
                return BadRequest(esrEx.Message);
            }
            catch (ExternalServiceNotFoundException esnfEx)
            {
                return BadRequest(esnfEx.Message);
            }
            catch (IncorrectExternalServiceTypeException iestEx)
            {
                return BadRequest(iestEx.Message);
            }
        }

        /// <summary>
        /// Update a project
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="updatedProject">Update project request body</param>
        /// <returns></returns>
        [HttpPut("{projectId}")]
        [Authorize(Policy = AuthorizePolicy.ProjectOwnerAccess)]
        public async Task<IActionResult> UpdateProject(int projectId, UpdateProjectDto updatedProject)
        {
            try
            {
                if (projectId != updatedProject.Id)
                    return BadRequest("project Id doesn't match.");

                var entity = _mapper.Map<Project>(updatedProject);
                await _projectService.UpdateProject(entity);

                return Ok();
            }
            catch (DuplicateProjectException dupEx)
            {
                return BadRequest(dupEx.Message);
            }
        }

        /// <summary>
        /// Clone a project to a new project
        /// </summary>
        /// <param name="projectId">Id of the source project</param>
        /// <param name="option">Clone project request body</param>
        /// <returns></returns>
        [HttpPost("{projectId}/clone")]
        [ProducesResponseType(201)]
        [Authorize(Policy = AuthorizePolicy.ProjectOwnerAccess)]
        public async Task<IActionResult> CloneProject(int projectId, CloneProjectOptionDto option)
        {
            try
            {
                var createdProject = await _projectService.CloneProject(projectId, option.NewProjectName,
                    option.IncludeMembers, option.IncludeJobDefinitions);
                var project = _mapper.Map<ProjectDto>(createdProject);
                return CreatedAtRoute("GetProjectById", new {projectId = project.Id}, project);

            }
            catch (DuplicateProjectException dupEx)
            {
                return BadRequest(dupEx.Message);
            }
            catch (ProjectNotFoundException nopEx)
            {
                return BadRequest(nopEx.Message);
            }
        }

        /// <summary>
        /// Delete a project
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <returns></returns>
        [HttpDelete("{projectId}")]
        [Authorize(Policy = AuthorizePolicy.ProjectOwnerAccess)]
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            await _projectService.DeleteProject(projectId);

            return NoContent();
        }

        /// <summary>
        /// Archive a project
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <returns></returns>
        [HttpPost("{projectId}/archive")]
        [Authorize(Policy = AuthorizePolicy.ProjectOwnerAccess)]
        public async Task<IActionResult> ArchiveProject(int projectId)
        {
            await _projectService.ArchiveProject(projectId);

            return NoContent();
        }

        /// <summary>
        /// Restore an archived project
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <returns></returns>
        [HttpPost("{projectId}/restore")]
        [Authorize(Policy = AuthorizePolicy.ProjectOwnerAccess)]
        public async Task<IActionResult> RestoreProject(int projectId)
        {
            await _projectService.RestoreProject(projectId);

            return Ok();
        }

        [HttpGet("{projectId}/export")]
        [Authorize(Policy = AuthorizePolicy.ProjectOwnerAccess)]
        public async Task<IActionResult> ExportProject(int projectId)
        {
            try
            {
                var yamlText = await _projectService.ExportProject(projectId);
                return Ok(yamlText);
            }
            catch (ProjectNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
