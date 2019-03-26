// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.Project;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Shared.ApiClient
{
    public class ProjectService : BaseService, IProjectService
    {
        public ProjectService(IApiClient api) : base(api)
        {
        }

        public async Task ArchiveProject(int projectId)
        {
            var path = $"project/{projectId}/archive";

            await Api.Post<object>(path, null);
        }

        public async Task RestoreProject(int projectId)
        {
            var path = $"project/{projectId}/restore";

            await Api.Post<object>(path, null);
        }

        public async Task<ProjectDto> CloneProject(int projectId, CloneProjectOptionDto option)
        {
            var path = $"project/{projectId}/clone";

            return await Api.Post<CloneProjectOptionDto, ProjectDto>(path, option);
        }

        public async Task<ProjectDto> CreateProject(NewProjectDto newProject)
        {
            var path = $"project";

            return await Api.Post<NewProjectDto, ProjectDto>(path, newProject);
        }

        public async Task UpdateProject(int projectId, UpdateProjectDto dto)
        {
            var path = $"project/{projectId}";

            await Api.Put(path, dto);
        }

        public async Task DeleteProject(int projectId, bool sendNotification = false)
        {
            var path = $"project/{projectId}?sendNotification={sendNotification}";

            await Api.Delete(path);
        }

        public async Task DeleteProjectByEngine(int projectId)
        {
            var path = $"project/{projectId}/engine";

            await Api.Delete(path);
        }

        public async Task MarkProjectDeleting(int projectId)
        {
            var path = $"project/{projectId}";

            await Api.Put<object>(path, null);
        }

        public async Task<ProjectDto> GetProject(int projectId)
        {
            var path = $"project/{projectId}";

            return await Api.Get<ProjectDto>(path);
        }

        public async Task<List<ProjectDto>> GetProjects(string status = null, bool getAll = false)
        {
            var path = $"project?status={status}&getAll={getAll}";

            return await Api.Get<List<ProjectDto>>(path);
        }

        public async Task<ProjectDto> GetProjectByName(string projectName)
        {
            var path = $"project/name/{projectName}";

            return await Api.Get<ProjectDto>(path);
        }

        public async Task<string> ExportProject(int projectId)
        {
            var path = $"project/{projectId}/export";

            return await Api.Get<string>(path);
        }
    }
}
