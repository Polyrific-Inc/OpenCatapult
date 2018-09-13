// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.Project;

namespace Polyrific.Catapult.Shared.Service
{
    public interface IProjectService
    {
        /// <summary>
        /// Get a project
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <returns>Project object</returns>
        Task<ProjectDto> GetProject(int projectId);

        /// <summary>
        /// Get a project by name
        /// </summary>
        /// <param name="projectName">Name of the project</param>
        /// <returns></returns>
        Task<ProjectDto> GetProjectByName(string projectName);

        /// <summary>
        /// Get list of projects that user has access to
        /// </summary>
        /// <param name="status">Status of the project (all | active | archived)</param>
        /// <returns>List of projects</returns>
        Task<List<ProjectDto>> GetProjects(string status = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newProject"></param>
        /// <returns>Project object</returns>
        Task<ProjectDto> CreateProject(NewProjectDto newProject);

        /// <summary>
        /// Clone a project
        /// </summary>
        /// <param name="projectId">Id of the source project</param>
        /// <param name="option">Clone option</param>
        /// <returns>New project object</returns>
        Task<ProjectDto> CloneProject(int projectId, CloneProjectOptionDto option);

        /// <summary>
        /// Update a project
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="dto">DTO containing project details</param>
        /// <returns></returns>
        Task UpdateProject(int projectId, UpdateProjectDto dto);

        /// <summary>
        /// Delete a project
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <returns></returns>
        Task DeleteProject(int projectId);

        /// <summary>
        /// Archive a project
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <returns></returns>
        Task ArchiveProject(int projectId);

        /// <summary>
        /// Restore an archived project
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <returns></returns>
        Task RestoreProject(int projectId);

        /// <summary>
        /// Export a project into a yaml format
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <returns>project yaml text</returns>
        Task<string> ExportProject(int projectId);
    }
}