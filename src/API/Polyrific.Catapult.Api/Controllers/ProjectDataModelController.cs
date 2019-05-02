// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Identity;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace Polyrific.Catapult.Api.Controllers
{
    [ApiController]
    public class ProjectDataModelController : ControllerBase
    {
        private readonly IProjectDataModelService _projectDataModelService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ProjectDataModelController(IProjectDataModelService projectDataModelService, IMapper mapper, ILogger<ProjectDataModelController> logger)
        {
            _projectDataModelService = projectDataModelService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get project data models for a project
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="includeProperties">Indicate whether the result should include model's properties. Default to false</param>
        /// <returns>List of project data models</returns>
        [HttpGet("Project/{projectId}/model", Name = "GetProjectDataModels")]
        [Authorize(Policy = AuthorizePolicy.ProjectAccess)]
        public async Task<IActionResult> GetProjectDataModels(int projectId, bool includeProperties = false)
        {
            _logger.LogRequest("Getting data models in project {projectId}. Include properties: {includeProperties}", projectId, includeProperties);

            var dataModels = await _projectDataModelService.GetProjectDataModels(projectId, includeProperties);
            var results = _mapper.Map<List<ProjectDataModelDto>>(dataModels);

            _logger.LogResponse("Data models in project {projectId} retrieved. Response body: {@results}", projectId, results);

            return Ok(results);
        }

        /// <summary>
        /// Create a project data model for a project
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="newProjectDataModel">Create project data model request body</param>
        /// <returns></returns>
        [HttpPost("Project/{projectId}/model", Name = "CreateProjectDataModel")]
        [ProducesResponseType(201)]
        [Authorize(Policy = AuthorizePolicy.ProjectContributorAccess)]
        public async Task<IActionResult> CreateProjectDataModel(int projectId, CreateProjectDataModelDto newProjectDataModel)
        {
            _logger.LogRequest("Getting data models for project {projectId}. Request body: {@newProjectDataModel}", projectId, newProjectDataModel);

            try
            {
                var modelId = await _projectDataModelService.AddProjectDataModel(projectId,
                    newProjectDataModel.Name, newProjectDataModel.Description, newProjectDataModel.Label, newProjectDataModel.IsManaged, newProjectDataModel.SelectKey);

                var projectDataModel = await _projectDataModelService.GetProjectDataModelById(modelId);
                var result = _mapper.Map<ProjectDataModelDto>(projectDataModel);

                _logger.LogResponse("Data model in project {projectId} created. Response body: {@result}", projectId, result);

                return CreatedAtRoute("GetProjectDataModelById", new
                {
                    projectId,
                    modelId
                }, result);
            }
            catch (DuplicateProjectDataModelException dupEx)
            {
                return BadRequest(dupEx.Message);
            }
            catch (ProjectNotFoundException projEx)
            {
                return BadRequest(projEx.Message);
            }
        }

        /// <summary>
        /// Get a project data model
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="modelId">Id of the project data model</param>
        /// <returns>Project data model object</returns>
        [HttpGet("Project/{projectId}/model/{modelId}", Name = "GetProjectDataModelById")]
        [Authorize(Policy = AuthorizePolicy.ProjectAccess)]
        public async Task<IActionResult> GetProjectDataModel(int projectId, int modelId)
        {
            _logger.LogRequest("Getting data model {modelId} in project {projectId}", modelId, projectId);

            var projectDataModel = await _projectDataModelService.GetProjectDataModelById(modelId);
            var result = _mapper.Map<ProjectDataModelDto>(projectDataModel);

            _logger.LogResponse("Data model {modelId} in project {projectId} created. Response body: {@result}", modelId, projectId, result);

            return Ok(result);
        }

        /// <summary>
        /// Get a project data model by name
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="modelName">Name of the project data model</param>
        /// <returns>Project data model object</returns>
        [HttpGet("Project/{projectId}/model/name/{modelName}")]
        [Authorize(Policy = AuthorizePolicy.ProjectAccess)]
        public async Task<IActionResult> GetProjectDataModel(int projectId, string modelName)
        {
            _logger.LogRequest("Getting data model {modelName} in project {projectId}", modelName, projectId);

            var projectDataModel = await _projectDataModelService.GetProjectDataModelByName(projectId, modelName);
            var result = _mapper.Map<ProjectDataModelDto>(projectDataModel);

            _logger.LogResponse("Data model {modelName} in project {projectId} retrieved. Response body: {@result}", modelName, projectId, result);

            return Ok(result);
        }

        /// <summary>
        /// Update a project data model
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="modelId">Id of the project data model</param>
        /// <param name="projectDataModel">Update project data model request body</param>
        /// <returns></returns>
        [HttpPut("Project/{projectId}/model/{modelId}")]
        [Authorize(Policy = AuthorizePolicy.ProjectContributorAccess)]
        public async Task<IActionResult> UpdateProjectDataModel(int projectId, int modelId, UpdateProjectDataModelDto projectDataModel)
        {
            _logger.LogRequest("Updating data model {modelId} in project {projectId}. Request body: {@projectDataModel}", modelId, projectId, projectDataModel);

            try
            {
                if (modelId != projectDataModel.Id)
                {
                    _logger.LogWarning("Model Id doesn't match");
                    return BadRequest("Model Id doesn't match.");
                }                    

                var updatedModel = _mapper.Map<ProjectDataModel>(projectDataModel);
                await _projectDataModelService.UpdateDataModel(updatedModel);
                
                _logger.LogResponse("Data model {modelId} in project {projectId} updated", modelId, projectId);

                return Ok();
            }
            catch (DuplicateProjectDataModelException ex)
            {
                _logger.LogWarning(ex, "Duplicate project data model name");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete a project data model
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="modelId">Id of the project data model</param>
        /// <returns></returns>
        [HttpDelete("Project/{projectId}/model/{modelId}")]
        [Authorize(Policy = AuthorizePolicy.ProjectContributorAccess)]
        public async Task<IActionResult> DeleteProjectDataModel(int projectId, int modelId)
        {
            _logger.LogRequest("Deleting data model {modelId} in project {projectId}", modelId, projectId);

            try
            {
                await _projectDataModelService.DeleteDataModel(modelId);
            }
            catch (RelatedProjectDataModelException ex)
            {
                _logger.LogWarning(ex, "The data model is being used by other model(s)");
                return BadRequest(ex.Message);
            }

            _logger.LogResponse("Data model {modelId} in project {projectId} deleted", modelId, projectId);

            return NoContent();
        }

        /// <summary>
        /// Delete project data models in batch
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="modelIds">Ids of the project data models</param>
        /// <returns></returns>
        [HttpDelete("Project/{projectId}/model/bulkdelete")]
        [Authorize(Policy = AuthorizePolicy.ProjectContributorAccess)]
        public async Task<IActionResult> DeleteProjectDataModels(int projectId, [FromQuery(Name = "modelIds")]int[] modelIds)
        {
            _logger.LogRequest("Deleting data models {modelIds} in project {projectId}", modelIds, projectId);

            try
            {
                await _projectDataModelService.DeleteDataModels(projectId, modelIds);
            }
            catch (RelatedProjectDataModelException ex)
            {
                _logger.LogWarning(ex, "The data models are being used by other model(s)");
                return BadRequest(ex.Message);
            }

            _logger.LogResponse("Data models {modelIds} in project {projectId} deleted", modelIds, projectId);

            return NoContent();
        }

        /// <summary>
        /// Get list of project data model properties of a project data model
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="modelId">Id of the project data model</param>
        /// <returns>List of project data model properties</returns>
        [HttpGet("Project/{projectId}/model/{modelId}/property", Name = "GetProjectDataModelProperties")]
        [Authorize(Policy = AuthorizePolicy.ProjectAccess)]
        public async Task<IActionResult> GetProjectDataModelProperties(int projectId, int modelId)
        {
            _logger.LogRequest("Getting properties in data model {modelId}, project {projectId}", modelId, projectId);

            var properties = await _projectDataModelService.GetDataModelProperties(modelId);
            var results = _mapper.Map<List<ProjectDataModelPropertyDto>>(properties);
            
            _logger.LogResponse("Properties in data model {modelId} in project {projectId} deleted", modelId, projectId);

            return Ok(results);
        }

        /// <summary>
        /// Create a project data model property
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="modelId">Id of the project data model</param>
        /// <param name="newProperty">Create project data model property request body</param>
        /// <returns></returns>
        [HttpPost("Project/{projectId}/model/{modelId}/property", Name = "CreateProjectDataModelProperty")]
        [ProducesResponseType(201)]
        [Authorize(Policy = AuthorizePolicy.ProjectContributorAccess)]
        public async Task<IActionResult> CreateProjectDataModelProperty(int projectId, int modelId, CreateProjectDataModelPropertyDto newProperty)
        {
            _logger.LogRequest("Creating property for data model {modelId} in project {projectId}. Request body: {@newProperty}", modelId, projectId, newProperty);

            try
            {
                var propertyId = await _projectDataModelService.AddDataModelProperty(modelId,
                    newProperty.Name,
                    newProperty.Label,
                    newProperty.DataType,
                    newProperty.ControlType,
                    newProperty.IsRequired,
                    newProperty.RelatedProjectDataModelId,
                    newProperty.RelationalType,
                    newProperty.IsManaged);

                var property = await _projectDataModelService.GetProjectDataModelPropertyByName(modelId, newProperty.Name);
                var result = _mapper.Map<ProjectDataModelPropertyDto>(property);

                _logger.LogResponse("Property in data model {modelId} in project {projectId} created. Response body: {@result}", modelId, projectId, result);

                return CreatedAtRoute("GetProjectDataModelPropertyById", new
                {
                    projectId,
                    modelId,
                    propertyId
                }, result);
            }
            catch (DuplicateProjectDataModelPropertyException dupEx)
            {
                _logger.LogWarning(dupEx, "Duplicate property name");
                return BadRequest(dupEx.Message);
            }
            catch (ProjectDataModelNotFoundException modEx)
            {
                _logger.LogWarning(modEx, "Project data model not found");
                return BadRequest(modEx.Message);
            }
        }

        /// <summary>
        /// Get a project data model property
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="modelId">Id of the project data model</param>
        /// <param name="propertyId">Id of the project data model property</param>
        /// <returns>Project data model property object</returns>
        [HttpGet("Project/{projectId}/model/{modelId}/property/{propertyId}", Name = "GetProjectDataModelPropertyById")]
        [Authorize(Policy = AuthorizePolicy.ProjectAccess)]
        public async Task<IActionResult> GetProjectDataModelProperty(int projectId, int modelId, int propertyId)
        {
            _logger.LogRequest("Getting property {propertyId} in data model {modelId}, project {projectId}", propertyId, modelId, projectId);

            var property = await _projectDataModelService.GetProjectDataModelPropertyById(propertyId);
            var result = _mapper.Map<ProjectDataModelPropertyDto>(property);

            _logger.LogResponse("Property {propertyId} in data model {modelId} in project {projectId} retrieved. Response body: {@result}", propertyId, modelId, projectId, result);

            return Ok(result);
        }

        /// <summary>
        /// Get a project data model property by name
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="modelId">Id of the project data model</param>
        /// <param name="propertyName">Name of the project data model property</param>
        /// <returns>Project data model property object</returns>
        [HttpGet("Project/{projectId}/model/{modelId}/property/name/{propertyName}")]
        [Authorize(Policy = AuthorizePolicy.ProjectAccess)]
        public async Task<IActionResult> GetProjectDataModelProperty(int projectId, int modelId, string propertyName)
        {
            _logger.LogRequest("Getting property {propertyName} in data model {modelId}, project {projectId}", propertyName, modelId, projectId);

            var property = await _projectDataModelService.GetProjectDataModelPropertyByName(modelId, propertyName);
            var result = _mapper.Map<ProjectDataModelPropertyDto>(property);

            _logger.LogResponse("Property {propertyName} in data model {modelId} in project {projectId} retrieved. Response body: {@result}", propertyName, modelId, projectId, result);

            return Ok(result);
        }

        /// <summary>
        /// Update a project data model property
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="modelId">Id of the project data model</param>
        /// <param name="propertyId">Id of the project data model property</param>
        /// <param name="projectDataModelProperty">Update project data model property request body</param>
        /// <returns></returns>
        [HttpPut("Project/{projectId}/model/{modelId}/property/{propertyId}")]
        [Authorize(Policy = AuthorizePolicy.ProjectContributorAccess)]
        public async Task<IActionResult> UpdateProjectDataModelProperty(int projectId, int modelId, int propertyId, UpdateProjectDataModelPropertyDto projectDataModelProperty)
        {
            _logger.LogRequest("Updating property {propertyId} in data model {modelId}, project {projectId}. Request body: {@projectDataModelProperty}", propertyId, modelId, projectId, projectDataModelProperty);

            try
            {
                if (propertyId != projectDataModelProperty.Id)
                {
                    _logger.LogWarning("Property Id doesn't match");
                    return BadRequest("Property Id doesn't match.");
                }                    

                var entity = _mapper.Map<ProjectDataModelProperty>(projectDataModelProperty);
                await _projectDataModelService.UpdateDataModelProperty(entity);

                _logger.LogResponse("Property {propertyId} in data model {modelId} in project {projectId} updated", propertyId, modelId, projectId);

                return Ok();
            }
            catch (DuplicateProjectDataModelPropertyException ex)
            {
                _logger.LogWarning(ex, "Duplicate property name");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete a project data model property
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="modelId">Id of the project data model</param>
        /// <param name="propertyId">Id of the project data model property</param>
        /// <returns></returns>
        [HttpDelete("Project/{projectId}/model/{modelId}/property/{propertyId}")]
        [Authorize(Policy = AuthorizePolicy.ProjectContributorAccess)]
        public async Task<IActionResult> DeleteProjectDataModelProperty(int projectId, int modelId, int propertyId)
        {
            _logger.LogRequest("Updating property {propertyId} in data model {modelId}, project {projectId}", propertyId, modelId, projectId);

            await _projectDataModelService.DeleteDataModelProperty(propertyId);

            _logger.LogResponse("Property {propertyId} in data model {modelId} in project {projectId} deleted", propertyId, modelId, projectId);

            return NoContent();
        }
    }
}
