// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.ProjectMember;

namespace Polyrific.Catapult.Shared.Service
{
    public interface IProjectMemberService
    {
        /// <summary>
        /// Get list of project members
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <returns></returns>
        Task<List<ProjectMemberDto>> GetProjectMembers(int projectId, int roleId = 0);

        /// <summary>
        /// Create new project member
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="dto">DTO containing project member details</param>
        /// <returns>Project member object</returns>
        Task<ProjectMemberDto> CreateProjectMember(int projectId, NewProjectMemberDto dto);

        /// <summary>
        /// Get project member
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="memberId">Id of the project member</param>
        /// <returns>Project member object</returns>
        Task<ProjectMemberDto> GetProjectMember(int projectId, int memberId);

        /// <summary>
        /// Get project member by user id
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="userId">Id of the user</param>
        /// <returns>Project member object</returns>
        Task<ProjectMemberDto> GetProjectMemberByUserId(int projectId, int userId);

        /// <summary>
        /// Update project member
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="memberId">Id of the project member</param>
        /// <param name="dto">DTO containing project member details</param>
        /// <returns></returns>
        Task UpdateProjectMember(int projectId, int memberId, UpdateProjectMemberDto dto);

        /// <summary>
        /// Remove project member
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="memberId">Id of the project member</param>
        /// <returns></returns>
        Task RemoveProjectMember(int projectId, int memberId);
    }
}