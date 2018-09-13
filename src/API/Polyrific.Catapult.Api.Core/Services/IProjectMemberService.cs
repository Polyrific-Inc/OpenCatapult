// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Core.Services
{
    public interface IProjectMemberService
    {
        /// <summary>
        /// Add existing user as a project member
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="userId">Id of the user</param>
        /// <param name="roleId">Id of the role</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>New id of the project member</returns>
        Task<int> AddProjectMember(int projectId, int userId, int roleId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Add new user as a project member
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="email">Email of the new user</param>
        /// <param name="firstName">First Name of the new user</param>
        /// <param name="lastName">Last Name of the new user</param>
        /// <param name="roleId">Id of the role</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>New id of the project member and id of the new user</returns>
        Task<(int newProjectMemberId, int newUserId)> AddProjectMember(int projectId, string email, string firstName, string lastName, int roleId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get members of a project
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// /// <param name="roleId">role id of the project</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>List of project members</returns>
        Task<List<ProjectMember>> GetProjectMembers(int projectId, int roleId = 0, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update role of a project member
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="userId">Id of the user</param>
        /// <param name="roleId">Id of the role</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task UpdateProjectMemberRole(int projectId, int userId, int roleId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Remove a project member
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="userId">Id of the user</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task RemoveProjectMember(int projectId, int userId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get project member by id
        /// </summary>
        /// <param name="id">Id of the project member</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The project member entity</returns>
        Task<ProjectMember> GetProjectMemberById(int id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get project member by user id
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="userId">Id of the user</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The project member entity</returns>
        Task<ProjectMember> GetProjectMemberByUserId(int projectId, int userId, CancellationToken cancellationToken = default(CancellationToken));
    }
}