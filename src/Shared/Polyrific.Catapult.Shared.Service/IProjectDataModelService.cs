// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace Polyrific.Catapult.Shared.Service
{
    public interface IProjectDataModelService
    {
        /// <summary>
        /// Get list of project data models
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="includeProperties">Indicate whether the result should include model's properties. Default to false</param>
        /// <returns>List of Project data models</returns>
        Task<List<ProjectDataModelDto>> GetProjectDataModels(int projectId, bool includeProperties = false);

        /// <summary>
        /// Create a project data model
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="dto">DTO containing project data model details</param>
        /// <returns>Project data model object</returns>
        Task<ProjectDataModelDto> CreateProjectDataModel(int projectId, CreateProjectDataModelDto dto);

        /// <summary>
        /// Get project data model
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="modelId">Id of the project data model</param>
        /// <returns>Project data model object</returns>
        Task<ProjectDataModelDto> GetProjectDataModel(int projectId, int modelId);

        /// <summary>
        /// Get project data model by name
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="modelName">Name of the project data model</param>
        /// <returns>Project data model object</returns>
        Task<ProjectDataModelDto> GetProjectDataModelByName(int projectId, string modelName);

        /// <summary>
        /// Update project data model
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="modelId">Id of the project data model</param>
        /// <param name="dto">DTO containing project data model details</param>
        /// <returns></returns>
        Task UpdateProjectDataModel(int projectId, int modelId, UpdateProjectDataModelDto dto);

        /// <summary>
        /// Delete a project data model
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="modelId">Id of the project data model</param>
        /// <returns></returns>
        Task DeleteProjectDataModel(int projectId, int modelId);

        /// <summary>
        /// Get list of project data model properties
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="modelId">Id of the project data model</param>
        /// <returns>List of project data model properties</returns>
        Task<List<ProjectDataModelPropertyDto>> GetProjectDataModelProperties(int projectId, int modelId);

        /// <summary>
        /// Create new project data model property
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="modelId">Id of the project data model</param>
        /// <param name="dto">DTO containing project data model property details</param>
        /// <returns>Project data model property object</returns>
        Task<ProjectDataModelPropertyDto> CreateProjectDataModelProperty(int projectId, int modelId,
            CreateProjectDataModelPropertyDto dto);

        /// <summary>
        /// Get project data model property
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="modelId">Id of the project data model</param>
        /// <param name="propertyId">Id of the project data model property</param>
        /// <returns>Project data model property object</returns>
        Task<ProjectDataModelPropertyDto> GetProjectDataModelProperty(int projectId, int modelId, int propertyId);

        /// <summary>
        /// Get project data model property
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="modelId">Id of the project data model</param>
        /// <param name="propertyName">Name of the project data model property</param>
        /// <returns>Project data model property object</returns>
        Task<ProjectDataModelPropertyDto> GetProjectDataModelPropertyByName(int projectId, int modelId, string propertyName);

        /// <summary>
        /// Update project data model property
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="modelId">Id of the project data model</param>
        /// <param name="propertyId">Id of the project data model property</param>
        /// <param name="dto">DTO containing project data model property details</param>
        /// <returns></returns>
        Task UpdateProjectDataModelProperty(int projectId, int modelId, int propertyId, UpdateProjectDataModelPropertyDto dto);

        /// <summary>
        /// Delete a project data model property
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="modelId">Id of project data model</param>
        /// <param name="propertyId">Id of project data model property</param>
        /// <returns></returns>
        Task DeleteProjectDataModelProperty(int projectId, int modelId, int propertyId);
    }
}
