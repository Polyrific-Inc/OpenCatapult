// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Specifications;
using Polyrific.Catapult.Shared.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Core.Services
{
    public class ProjectDataModelService : IProjectDataModelService
    {
        private readonly IProjectDataModelRepository _dataModelRepository;
        private readonly IProjectDataModelPropertyRepository _dataModelPropertyRepository;
        private readonly IProjectRepository _projectRepository;

        public ProjectDataModelService(IProjectDataModelRepository dataModelRepository, 
            IProjectDataModelPropertyRepository dataModelPropertyRepository,
            IProjectRepository projectRepository)
        {
            _dataModelRepository = dataModelRepository;
            _dataModelPropertyRepository = dataModelPropertyRepository;
            _projectRepository = projectRepository;
        }

        public async Task<int> AddDataModelProperty(int dataModelId, string name, string label, string dataType, string controlType, bool isRequired, int? relatedDataModelId, string relationalType, bool? isManaged, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var dataModel = await _dataModelRepository.GetById(dataModelId, cancellationToken);
            if (dataModel == null)
            {
                throw new ProjectDataModelNotFoundException(dataModelId);
            }

            var projectDataModelPropertyByProjectSpec = new ProjectDataModelPropertyFilterSpecification(name, dataModelId);
            if (await _dataModelPropertyRepository.CountBySpec(projectDataModelPropertyByProjectSpec, cancellationToken) > 0)
            {
                throw new DuplicateProjectDataModelPropertyException(name);
            }

            var newDataModelProperty = new ProjectDataModelProperty
            {
                ProjectDataModelId = dataModelId,
                Name = name,
                Label = string.IsNullOrEmpty(label) ? TextHelper.SplitTextOnCapitalLetters(name) : label,
                DataType = dataType,
                ControlType = controlType,
                RelatedProjectDataModelId = relatedDataModelId,
                RelationalType = relationalType,
                IsRequired = isRequired,
                IsManaged = isManaged
            };

            return await _dataModelPropertyRepository.Create(newDataModelProperty, cancellationToken);
        }

        public async Task<int> AddProjectDataModel(int projectId, string name, string description, string label, bool? isManaged, string selectKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var project = await _projectRepository.GetById(projectId, cancellationToken);
            if (project == null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            var projectDataModelPropertyByProjectSpec = new ProjectDataModelFilterSpecification(name, projectId);
            if (await _dataModelRepository.CountBySpec(projectDataModelPropertyByProjectSpec, cancellationToken) > 0)
            {
                throw new DuplicateProjectDataModelException(name);
            }
            
            var newDataModel = new ProjectDataModel
            {
                ProjectId = projectId,
                Name = name,
                Description = description,
                Label = label,
                IsManaged = isManaged,
                SelectKey = selectKey
            };

            if (string.IsNullOrEmpty(newDataModel.Label))
            {
                newDataModel.Label = TextHelper.SplitTextOnCapitalLetters(name);
            }

            return await _dataModelRepository.Create(newDataModel, cancellationToken);
        }

        public async Task DeleteDataModel(int id, bool validateRelatedModel = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (validateRelatedModel)
            {
                var model = await _dataModelRepository.GetById(id, cancellationToken);

                var relatedDataModelsSpec = new ProjectDataModelFilterSpecification(model.ProjectId, id);
                var relatedDataModels = await _dataModelRepository.GetBySpec(relatedDataModelsSpec, cancellationToken);
                if (relatedDataModels.Any())
                {
                    throw new RelatedProjectDataModelException(model.Name, relatedDataModels.Select(m => m.Name).ToArray());
                }
            }

            var propertyByDataModelSpec = new ProjectDataModelPropertyFilterSpecification(id);
            var properties = await _dataModelPropertyRepository.GetBySpec(propertyByDataModelSpec, cancellationToken);
            foreach (var property in properties.ToList())
            {
                await DeleteDataModelProperty(property.Id, cancellationToken);
            }

            await _dataModelRepository.Delete(id, cancellationToken);
        }

        public async Task DeleteDataModels(int projectId, int[] ids, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var modelsSpec = new ProjectDataModelFilterSpecification(ids);
            modelsSpec.IncludeStrings.Add("Properties.RelatedProjectDataModel");
            var models = (await _dataModelRepository.GetBySpec(modelsSpec, cancellationToken)).ToList();

            // search for models to be deleted that dependency is not included in the current deletion
            var relatedDataModelsSpec = new ProjectDataModelFilterSpecification(projectId, ids.Select(id => (int?)id).ToArray());
            var relatedDataModels = (await _dataModelRepository.GetBySpec(relatedDataModelsSpec, cancellationToken)).ToList();

            if (relatedDataModels.Count > 0)
            {
                throw new RelatedProjectDataModelException(relatedDataModels.Select(m => m.Name).Distinct().ToArray());
            }

            // sort the deletion order so the conflict foreign key error is not thrown
            models.Sort(CompareDataModelRelation);

            foreach (var model in models)
            {
                await this.DeleteDataModel(model.Id, false, cancellationToken);
            }
        }

        public async Task DeleteDataModelProperty(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _dataModelPropertyRepository.Delete(id, cancellationToken);
        }

        public async Task<List<ProjectDataModelProperty>> GetDataModelProperties(int dataModelId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var propertyByDataModelSpec = new ProjectDataModelPropertyFilterSpecification(dataModelId);
            propertyByDataModelSpec.Includes.Add(p => p.RelatedProjectDataModel);
            var properties = await _dataModelPropertyRepository.GetBySpec(propertyByDataModelSpec, cancellationToken);

            return properties.ToList();
        }

        public async Task<ProjectDataModel> GetProjectDataModelById(int modelId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _dataModelRepository.GetById(modelId, cancellationToken);
        }

        public async Task<ProjectDataModel> GetProjectDataModelByName(int projectId, string modelName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var dataModelByNameSpec = new ProjectDataModelFilterSpecification(modelName, projectId);
            dataModelByNameSpec.IncludeStrings.Add("Properties.RelatedProjectDataModel");

            return await _dataModelRepository.GetSingleBySpec(dataModelByNameSpec, cancellationToken);
        }

        public async Task<ProjectDataModelProperty> GetProjectDataModelPropertyById(int dataModelPropertyId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _dataModelPropertyRepository.GetById(dataModelPropertyId, cancellationToken);
        }

        public async Task<ProjectDataModelProperty> GetProjectDataModelPropertyByName(int dataModelId, string propertyName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var propertySpec = new ProjectDataModelPropertyFilterSpecification(propertyName, dataModelId);
            propertySpec.Includes.Add(p => p.RelatedProjectDataModel);

            return await _dataModelPropertyRepository.GetSingleBySpec(propertySpec, cancellationToken);
        }

        public async Task<List<ProjectDataModel>> GetProjectDataModels(int projectId, bool includeProperties, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var dataModelByProjectSpec = new ProjectDataModelFilterSpecification(projectId);

            if (includeProperties)
                dataModelByProjectSpec.IncludeStrings.Add("Properties.RelatedProjectDataModel");

            var dataModels = (await _dataModelRepository.GetBySpec(dataModelByProjectSpec, cancellationToken)).ToList();

            foreach (var dataModel in dataModels)
            {
                dataModel.Properties = dataModel.Properties.OrderBy(p => p.Sequence).ToList();
            }

            return dataModels;
        }

        public async Task UpdateDataModel(ProjectDataModel updatedDataModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var dataModel = await _dataModelRepository.GetById(updatedDataModel.Id, cancellationToken);

            if (dataModel != null)
            {
                var dataModelByNameSpec = new ProjectDataModelFilterSpecification(updatedDataModel.Name, dataModel.ProjectId, dataModel.Id);
                if (await _dataModelRepository.CountBySpec(dataModelByNameSpec, cancellationToken) > 0)
                {
                    throw new DuplicateProjectDataModelException(updatedDataModel.Name);
                }

                dataModel.Name = updatedDataModel.Name;
                dataModel.Description = updatedDataModel.Description;
                dataModel.Label = updatedDataModel.Label;
                dataModel.IsManaged = updatedDataModel.IsManaged;
                dataModel.SelectKey = updatedDataModel.SelectKey;

                await _dataModelRepository.Update(dataModel, cancellationToken);
            }
        }

        public async Task UpdateDataModelProperty(ProjectDataModelProperty editedProperty, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var propertyByNameSpec = new ProjectDataModelPropertyFilterSpecification(editedProperty.Name, editedProperty.ProjectDataModelId, editedProperty.Id);
            if (await _dataModelPropertyRepository.CountBySpec(propertyByNameSpec, cancellationToken) > 0)
            {
                throw new DuplicateProjectDataModelPropertyException(editedProperty.Name);
            }

            var property = await _dataModelPropertyRepository.GetById(editedProperty.Id, cancellationToken);

            if (property != null)
            {
                property.Name = editedProperty.Name;
                property.DataType = editedProperty.DataType;
                property.Label = editedProperty.Label;
                property.IsRequired = editedProperty.IsRequired;
                property.ControlType = editedProperty.ControlType;
                property.RelatedProjectDataModelId = editedProperty.RelatedProjectDataModelId;
                property.RelationalType = editedProperty.RelationalType;
                property.IsManaged = editedProperty.IsManaged;
                await _dataModelPropertyRepository.Update(property, cancellationToken);
            }
        }

        private int CompareDataModelRelation(ProjectDataModel m1, ProjectDataModel m2)
        {
            if (m1.Properties?.Any(p => p.RelatedProjectDataModelId == m2.Id) ?? false)
            {
                return -1;
            }
            else if (m2.Properties?.Any(p => p.RelatedProjectDataModelId == m1.Id) ?? false)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }                  
}
