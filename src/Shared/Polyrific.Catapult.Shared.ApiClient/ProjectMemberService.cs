// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.ProjectMember;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Shared.ApiClient
{
    public class ProjectMemberService : BaseService, IProjectMemberService
    {
        public ProjectMemberService(IApiClient api) : base(api)
        {
        }

        public async Task<ProjectMemberDto> CreateProjectMember(int projectId, NewProjectMemberDto dto)
        {
            var path = $"project/{projectId}/member";

            return await Api.Post<NewProjectMemberDto, ProjectMemberDto>(path, dto);
        }

        public async Task<ProjectMemberDto> GetProjectMember(int projectId, int memberId)
        {
            var path = $"project/{projectId}/member/{memberId}";

            return await Api.Get<ProjectMemberDto>(path);
        }

        public async Task<ProjectMemberDto> GetProjectMemberByUserId(int projectId, int userId)
        {
            var path = $"project/{projectId}/member/user/{userId}";

            return await Api.Get<ProjectMemberDto>(path);
        }

        public async Task<List<ProjectMemberDto>> GetProjectMembers(int projectId, int roleId = 0)
        {
            var path = $"project/{projectId}/member";

            if (roleId > 0)
            {
                path = $"{path}?roleId={roleId}";
            }

            return await Api.Get<List<ProjectMemberDto>>(path);
        }

        public async Task<List<ProjectMemberDto>> GetProjectMembersForEngine(int projectId)
        {
            var path = $"project/{projectId}/member/engine";

            return await Api.Get<List<ProjectMemberDto>>(path);
        }

        public async Task RemoveProjectMember(int projectId, int memberId)
        {
            var path = $"project/{projectId}/member/{memberId}";

            await Api.Delete(path);
        }

        public async Task UpdateProjectMember(int projectId, int memberId, UpdateProjectMemberDto dto)
        {
            var path = $"project/{projectId}/member/{memberId}";

            await Api.Put(path, dto);
        }
    }
}
