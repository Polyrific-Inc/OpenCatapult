// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Core.Services
{
    public interface IProjectService
    {
        /// <summary>
        /// Archive a project
        /// </summary>
        /// <param name="id">Id of the project</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task ArchiveProject(int id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Clone a project
        /// </summary>
        /// <param name="id">Id of the project</param>
        /// <param name="newProjectName">Name of the new project</param>
        /// <param name="ownerUserId">User id of the project owner</param>
        /// <param name="includeMembers">Copy project members into the new project?</param>
        /// <param name="includeJobDefinitions">Copy job definitions into the new project?</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The new project entity</returns>
        Task<Project> CloneProject(int id, string newProjectName, int ownerUserId, bool includeMembers = false, bool includeJobDefinitions = false, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Export a project into a yaml text
        /// </summary>
        /// <param name="id">Id of the project</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task<string> ExportProject(int id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Create a new project
        /// </summary>
        /// <param name="projectName">Name of the project</param>
        /// <param name="client">Client of the project</param>
        /// <param name="projectMembers">Members of the project</param>
        /// <param name="configs">Configs of the project</param>
        /// <param name="models">Models of the project</param>
        /// <param name="jobs">Job definitions of the project</param>
        /// <param name="currentUserId">Id of the current user</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The new created project</returns>
        Task<Project> CreateProject(string projectName, string client, List<(int userId, int projectMemberRoleId)> projectMembers, Dictionary<string, string> configs, List<ProjectDataModel> models, List<JobDefinition> jobs, int currentUserId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete a project
        /// </summary>
        /// <param name="id">Id of the project</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task DeleteProject(int id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get project by id
        /// </summary>
        /// <param name="id">Id of the project</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The project entity</returns>
        Task<Project> GetProjectById(int id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get project by name
        /// </summary>
        /// <param name="name">Name of the project</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The project entity</returns>
        Task<Project> GetProjectByName(string name, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update the project
        /// </summary>
        /// <param name="entity">The project entity</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task UpdateProject(Project entity, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get all accessible projects for a user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="status">Status of the project</param>
        /// <param name="getAll">Indicates whether to get all projects for other members</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>List of accessible projects</returns>
        Task<List<(Project, ProjectMemberRole)>> GetProjectsByUser(int userId, string status = null, bool getAll = false, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Restore an archived project
        /// </summary>
        /// <param name="id">Id of the project</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task RestoreProject(int projectId, CancellationToken cancellationToken = default(CancellationToken));
    }
}
