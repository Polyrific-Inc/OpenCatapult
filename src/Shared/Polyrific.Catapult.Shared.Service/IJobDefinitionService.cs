// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Shared.Dto.JobDefinition;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Shared.Service
{
    public interface IJobDefinitionService
    {
        /// <summary>
        /// Get list of job definitions
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <returns></returns>
        Task<List<JobDefinitionDto>> GetJobDefinitions(int projectId);

        /// <summary>
        /// Create a new job definition
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="dto">DTO containing job definition details</param>
        /// <returns>Job definition object</returns>
        Task<JobDefinitionDto> CreateJobDefinition(int projectId, CreateJobDefinitionDto dto);

        /// <summary>
        /// Get a job definition
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobId">Id of the job definition</param>
        /// <returns>Job definition object</returns>
        Task<JobDefinitionDto> GetJobDefinition(int projectId, int jobId);

        /// <summary>
        /// Get a job definition
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="name">Name of the job definition</param>
        /// <returns>Job definition object</returns>
        Task<JobDefinitionDto> GetJobDefinitionByName(int projectId, string jobName);

        /// <summary>
        /// Update job definition
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobId">Id of the job definition</param>
        /// <param name="dto">DTO containing job definition details</param>
        /// <returns></returns>
        Task UpdateJobDefinition(int projectId, int jobId, UpdateJobDefinitionDto dto);

        /// <summary>
        /// Delete job definition
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobId">Id of the job definition</param>
        /// <returns></returns>
        Task DeleteJobDefinition(int projectId, int jobId);

        /// <summary>
        /// Create new job task definition
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobId">Id of the job definition</param>
        /// <param name="dto">DTO containing job task definition details</param>
        /// <returns>Job task definition object</returns>
        Task<JobTaskDefinitionDto> CreateJobTaskDefinition(int projectId, int jobId, CreateJobTaskDefinitionDto dto);

        /// <summary>
        /// Create a range of job task definitions
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobId">Id of the job definition</param>
        /// <param name="dto">DTO containing list of job task definition details</param>
        /// <returns>List of job task definitions</returns>
        Task<List<JobTaskDefinitionDto>> CreateJobTaskDefinitions(int projectId, int jobId, List<CreateJobTaskDefinitionDto> dto);

        /// <summary>
        /// Get list of job task definitions
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobId">Id of job definition</param>
        /// <returns>List of job task definitions</returns>
        Task<List<JobTaskDefinitionDto>> GetJobTaskDefinitions(int projectId, int jobId);

        /// <summary>
        /// Get a job task definition
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobId">Id of the job definition</param>
        /// <param name="taskId">Id of the job task definition</param>
        /// <returns>Job task definition object</returns>
        Task<JobTaskDefinitionDto> GetJobTaskDefinition(int projectId, int jobId, int taskId);

        /// <summary>
        /// Get a job task definition by name
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobId">Id of the job definition</param>
        /// <param name="taskName">Name of the job task definition</param>
        /// <returns>Job task definition object</returns>
        Task<JobTaskDefinitionDto> GetJobTaskDefinitionByName(int projectId, int jobId, string taskName);

        /// <summary>
        /// Update job task definition
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobId">Id of the job definition</param>
        /// <param name="taskId">Id of the job task definition</param>
        /// <param name="dto">DTO containing job task definition details</param>
        /// <returns></returns>
        Task UpdateJobTaskDefinition(int projectId, int jobId, int taskId, UpdateJobTaskDefinitionDto dto);

        /// <summary>
        /// Delete a job task definition
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobId">Id of the job definition</param>
        /// <param name="taskId">Id of the job task definition</param>
        /// <returns></returns>
        Task DeleteJobTaskDefinition(int projectId, int jobId, int taskId);
    }
}