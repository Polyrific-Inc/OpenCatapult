// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Shared.ApiClient
{
    public class ProjectDataModelService : BaseService, IProjectDataModelService
    {
        public ProjectDataModelService(IApiClient api) : base(api)
        {
        }

        public async Task<ProjectDataModelDto> CreateProjectDataModel(int projectId, CreateProjectDataModelDto dto)
        {
            var path = $"project/{projectId}/model";

            return await Api.Post<CreateProjectDataModelDto, ProjectDataModelDto>(path, dto);
        }

        public async Task<ProjectDataModelPropertyDto> CreateProjectDataModelProperty(int projectId, int modelId, CreateProjectDataModelPropertyDto dto)
        {
            var path = $"project/{projectId}/model/{modelId}/property";

            return await Api.Post<CreateProjectDataModelPropertyDto, ProjectDataModelPropertyDto>(path, dto);
        }

        public async Task DeleteProjectDataModel(int projectId, int modelId)
        {
            var path = $"project/{projectId}/model/{modelId}";

            await Api.Delete(path);
        }

        public async Task DeleteProjectDataModelProperty(int projectId, int modelId, int propertyId)
        {
            var path = $"project/{projectId}/model/{modelId}/property/{propertyId}";

            await Api.Delete(path);
        }

        public async Task<ProjectDataModelDto> GetProjectDataModel(int projectId, int modelId)
        {
            var path = $"project/{projectId}/model/{modelId}";

            return await Api.Get<ProjectDataModelDto>(path);
        }

        public async Task<ProjectDataModelDto> GetProjectDataModelByName(int projectId, string modelName)
        {
            var path = $"project/{projectId}/model/name/{modelName}";

            return await Api.Get<ProjectDataModelDto>(path);
        }

        public async Task<List<ProjectDataModelPropertyDto>> GetProjectDataModelProperties(int projectId, int modelId)
        {
            var path = $"project/{projectId}/model/{modelId}/property";

            return await Api.Get<List<ProjectDataModelPropertyDto>>(path);
        }

        public async Task<ProjectDataModelPropertyDto> GetProjectDataModelProperty(int projectId, int modelId, int propertyId)
        {
            var path = $"project/{projectId}/model/{modelId}/property/{propertyId}";

            return await Api.Get<ProjectDataModelPropertyDto>(path);
        }

        public async Task<ProjectDataModelPropertyDto> GetProjectDataModelPropertyByName(int projectId, int modelId, string propertyName)
        {
            var path = $"project/{projectId}/model/{modelId}/property/name/{propertyName}";

            return await Api.Get<ProjectDataModelPropertyDto>(path);
        }

        public async Task<List<ProjectDataModelDto>> GetProjectDataModels(int projectId)
        {
            var path = $"project/{projectId}/model/";

            return await Api.Get<List<ProjectDataModelDto>>(path);
        }

        public async Task UpdateProjectDataModel(int projectId, int modelId, UpdateProjectDataModelDto dto)
        {
            var path = $"project/{projectId}/model/{modelId}";

            await Api.Put(path, dto);
        }

        public async Task UpdateProjectDataModelProperty(int projectId, int modelId, int propertyId, UpdateProjectDataModelPropertyDto dto)
        {
            var path = $"project/{projectId}/model/{modelId}/property/{propertyId}";

            await Api.Put(path, dto);
        }
    }
}