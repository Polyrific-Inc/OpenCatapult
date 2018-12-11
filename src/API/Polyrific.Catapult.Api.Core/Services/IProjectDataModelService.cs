// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Core.Services
{
    public interface IProjectDataModelService
    {
        /// <summary>
        /// Add new project data model
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="name">Name of the data model</param>
        /// <param name="description">Description of the data model</param>
        /// <param name="label">Label of the data model</param>
        /// <param name="isManaged">Is the model managed in the UI?</param>
        /// <param name="selectKey">The property name used as the key for select control</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Id of the new added data model</returns>
        Task<int> AddProjectDataModel(int projectId, string name, string description, string label, bool? isManaged, string selectKey, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update a data model
        /// </summary>
        /// <param name="updateDataModel">The updated data model</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task UpdateDataModel(ProjectDataModel updateDataModel, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete a data model
        /// </summary>
        /// <param name="id">Delete a data model</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task DeleteDataModel(int id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get list of project data models
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="includeProperties">Indicate whether the result should include model's properties</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>List of project data models</returns>
        Task<List<ProjectDataModel>> GetProjectDataModels(int projectId, bool includeProperties, CancellationToken cancellationToken = default(CancellationToken));
        
        /// <summary>
        /// Get a project data model by id
        /// </summary>
        /// <param name="modelId">Id of the project data model</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The project data model entity</returns>
        Task<ProjectDataModel> GetProjectDataModelById(int modelId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a project data model by name
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="modelName">Name of the project data model</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The project data model entity</returns>
        Task<ProjectDataModel> GetProjectDataModelByName(int projectId, string modelName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Add new data model property
        /// </summary>
        /// <param name="dataModelId">Id of the data model</param>
        /// <param name="name">Name of the property</param>
        /// <param name="label">Label of the property</param>
        /// <param name="dataType">Data type of the property</param>
        /// <param name="controlType">Control type of the property</param>
        /// <param name="isRequired">Is property required?</param>
        /// <param name="relatedDataModelId">Id of the related data model</param>
        /// <param name="relationalType">Type of the relation with the related data model</param>
        /// <param name="isManaged">Is the model managed in the UI?</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Id of the new added data model property</returns>
        Task<int> AddDataModelProperty(int dataModelId, string name, string label, string dataType, string controlType, bool isRequired, int? relatedDataModelId, string relationalType, bool? isManaged, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update a property
        /// </summary>
        /// <param name="editedProperty">Edited property</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task UpdateDataModelProperty(ProjectDataModelProperty editedProperty, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete a property
        /// </summary>
        /// <param name="id">Id of the property</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task DeleteDataModelProperty(int id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get list of data model properties
        /// </summary>
        /// <param name="dataModelId">Id of the data model</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>List of data model properties</returns>
        Task<List<ProjectDataModelProperty>> GetDataModelProperties(int dataModelId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a project data model property by id
        /// </summary>
        /// <param name="propertyId">Id of the project data model property</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The project data model property entity</returns>
        Task<ProjectDataModelProperty> GetProjectDataModelPropertyById(int propertyId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a project data model property by name
        /// </summary>
        /// <param name="dataModelId">Id of the data model</param>
        /// <param name="propertyName">Name of the project data model property</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The project data model property entity</returns>
        Task<ProjectDataModelProperty> GetProjectDataModelPropertyByName(int dataModelId, string propertyName, CancellationToken cancellationToken = default(CancellationToken));
    }
}
