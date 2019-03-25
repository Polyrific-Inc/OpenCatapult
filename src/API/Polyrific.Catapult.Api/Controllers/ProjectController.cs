// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Identity;
using Polyrific.Catapult.Shared.Common.Notification;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.JobDefinition;
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
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ProjectController(IProjectService projectService, IMapper mapper, ILogger<ProjectController> logger)
        {
            _projectService = projectService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get project list that the user authorized to view
        /// </summary>
        /// <param name="status">Status of the project (all | active | archived)</param>
        /// <param name="getAll">Get all projects</param>
        /// <returns>List of the project</returns>
        [HttpGet]
        [Authorize(Policy = AuthorizePolicy.ProjectMemberAccess)]
        public async Task<IActionResult> GetProjects(string status = null, bool getAll = false)
        {
            _logger.LogInformation("Getting projects. Filtered by status = {status}", status);

            try
            {
                var currentUserId = User.GetUserId();
                var projects = await _projectService.GetProjectsByUser(currentUserId, status, getAll && User.IsInRole(UserRole.Administrator));
                var results = _mapper.Map<List<ProjectDto>>(projects.Select(p => p.Item1));

                return Ok(results);
            }
            catch (FilterTypeNotFoundException ex)
            {
                _logger.LogWarning(ex, "Filter type not found");
                return BadRequest(ex);
            }
        }

        [HttpGet("name/{projectName}")]
        [Authorize(Policy = AuthorizePolicy.ProjectAccess)]
        public async Task<IActionResult> GetProjectByName(string projectName)
        {
            _logger.LogInformation("Getting project {projectName}", projectName);

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
            _logger.LogInformation("Getting project {projectId}", projectId);

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
            // exclude task's additional configs since it may contain secret values
            var requestBodyToLog = new NewProjectDto
            {
                Name = newProject.Name,
                Members = newProject.Members,
                Client = newProject.Client,
                Models = newProject.Models,
                Jobs = newProject.Jobs?.Select(j => new CreateJobDefinitionWithTasksDto
                {
                    Name = j.Name,
                    Tasks = j.Tasks?.Select(t => new CreateJobTaskDefinitionDto
                    {
                        Name = t.Name,
                        Configs = t.Configs,
                        Provider = t.Provider,
                        Sequence = t.Sequence,
                        Type = t.Type
                    }).ToList()
                }).ToList()
            };
            _logger.LogInformation("Creating project. Request body: {@requestBodyToLog}", requestBodyToLog);

            try
            {
                var projectMembers = newProject.Members?.Select(m => (m.UserId, m.ProjectMemberRoleId)).ToList();
                var currentUserId = User.GetUserId();

                List<ProjectDataModel> models = null;
                List<JobDefinition> jobs = null;

                if (newProject.Models != null)
                    models = _mapper.Map<List<ProjectDataModel>>(newProject.Models);
                
                if (newProject.Jobs != null)
                    jobs = _mapper.Map<List<JobDefinition>>(newProject.Jobs);

                var createdProject = await _projectService.CreateProject(newProject.Name, newProject.DisplayName, newProject.Client, projectMembers, models, jobs, currentUserId);
                var project = _mapper.Map<ProjectDto>(createdProject);
                return CreatedAtRoute("GetProjectById", new { projectId = project.Id }, project);
                
            }
            catch (DuplicateProjectException dupEx)
            {
                _logger.LogWarning(dupEx, "Duplicate project name");
                return BadRequest(dupEx.Message);
            }
            catch (ProjectDataModelNotFoundException modelEx)
            {
                _logger.LogWarning(modelEx, "Project data model not found");
                return BadRequest(modelEx.Message);
            }
            catch (DuplicateJobTaskDefinitionException dupTaskEx)
            {
                _logger.LogWarning(dupTaskEx, "Duplicate task name");
                return BadRequest(dupTaskEx.Message);
            }
            catch (InvalidPluginTypeException pluginTypeEx)
            {
                _logger.LogWarning(pluginTypeEx, "Invalid provider's plugin type");
                return BadRequest(pluginTypeEx.Message);
            }
            catch (ProviderNotInstalledException provEx)
            {
                _logger.LogWarning(provEx, "Provider not installed");
                return BadRequest(provEx.Message);
            }
            catch (ExternalServiceRequiredException esrEx)
            {
                _logger.LogWarning(esrEx, "External service required");
                return BadRequest(esrEx.Message);
            }
            catch (ExternalServiceNotFoundException esnfEx)
            {
                _logger.LogWarning(esnfEx, "External service not found");
                return BadRequest(esnfEx.Message);
            }
            catch (IncorrectExternalServiceTypeException iestEx)
            {
                _logger.LogWarning(iestEx, "Incorrect external service type");
                return BadRequest(iestEx.Message);
            }
            catch (JobTaskDefinitionTypeException taskEx)
            {
                _logger.LogWarning(taskEx, "Incorrect task definition type");
                return BadRequest(taskEx.Message);
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
            _logger.LogInformation("Updating project {projectId}. Request body: {@updatedProject}", projectId, updatedProject);

            try
            {
                if (projectId != updatedProject.Id)
                {
                    _logger.LogWarning("Project Id doesn't match");
                    return BadRequest("Project Id doesn't match.");
                }                    

                var entity = _mapper.Map<Project>(updatedProject);
                await _projectService.UpdateProject(entity);

                return Ok();
            }
            catch (DuplicateProjectException dupEx)
            {
                _logger.LogWarning(dupEx, "Duplicate project name");
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
            _logger.LogInformation("Cloning project {projectId}. Request body: {@option}", projectId, option);

            try
            {
                var ownerUserId = User.GetUserId();
                var createdProject = await _projectService.CloneProject(projectId, option.NewProjectName, option.DisplayName, option.Client, ownerUserId,
                    option.IncludeMembers, option.IncludeJobDefinitions);
                var project = _mapper.Map<ProjectDto>(createdProject);
                return CreatedAtRoute("GetProjectById", new {projectId = project.Id}, project);
            }
            catch (DuplicateProjectException dupEx)
            {
                _logger.LogWarning(dupEx, "Duplicate project name");
                return BadRequest(dupEx.Message);
            }
            catch (ProjectNotFoundException nopEx)
            {
                _logger.LogWarning(nopEx, "Project not found");
                return BadRequest(nopEx.Message);
            }
        }

        /// <summary>
        /// Delete a project
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="sendNotification">Send notification after project deletion</param>
        /// <returns></returns>
        [HttpDelete("{projectId}")]
        [Authorize(Policy = AuthorizePolicy.ProjectOwnerAccess)]
        public async Task<IActionResult> DeleteProject(int projectId, bool sendNotification = false)
        {
            _logger.LogInformation("Deleting project {projectId}", projectId);

            await _projectService.DeleteProject(projectId, sendNotification);

            return NoContent();
        }

        /// <summary>
        /// Delete a project invoked by engine
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <returns></returns>
        [HttpDelete("{projectId}/engine")]
        [Authorize(Policy = AuthorizePolicy.UserRoleEngineAccess)]
        public async Task<IActionResult> DeleteProjectByEngine(int projectId)
        {
            _logger.LogInformation("Deleting project {projectId}", projectId);

            var project = await _projectService.GetProjectById(projectId);

            // only allow engine delete deleting project
            if (project.Status != ProjectStatusFilterType.Deleting)
                return Unauthorized();

            await _projectService.DeleteProject(projectId, true);

            return NoContent();
        }

        /// <summary>
        /// Mark a project as deleting, and queue the deletion job definition
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <returns></returns>
        [HttpPut("{projectId}/deleting")]
        [Authorize(Policy = AuthorizePolicy.ProjectOwnerAccess)]
        public async Task<IActionResult> MarkProjectDeleting(int projectId)
        {
            try
            {
                _logger.LogInformation("Mark project {projectId} as \"deleting\"", projectId);

                await _projectService.MarkProjectDeleting(projectId, Request.Host.ToUriComponent());

                return Ok();
            }
            catch (DeletionJobDefinitionNotFound ex)
            {
                _logger.LogWarning(ex, "Deletion job definition not found");
                return BadRequest(ex.Message);
            }
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
            _logger.LogInformation("Archiving project {projectId}", projectId);

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
            _logger.LogInformation("Restoring project {projectId}", projectId);

            await _projectService.RestoreProject(projectId);

            return Ok();
        }

        [HttpGet("{projectId}/export")]
        [Authorize(Policy = AuthorizePolicy.ProjectOwnerAccess)]
        public async Task<IActionResult> ExportProject(int projectId)
        {
            _logger.LogInformation("Exporting project {projectId}", projectId);

            try
            {
                var yamlText = await _projectService.ExportProject(projectId);
                return Ok(yamlText);
            }
            catch (ProjectNotFoundException ex)
            {
                _logger.LogWarning(ex, "Project not found");
                return BadRequest(ex.Message);
            }
        }
    }
}
