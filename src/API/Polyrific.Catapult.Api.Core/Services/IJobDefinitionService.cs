﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Core.Services
{
    public interface IJobDefinitionService
    {
        /// <summary>
        /// Add new job definition
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="name">Name of the job definition</param>
        /// <param name="isDefault">Is the job definition is a default job in the project?</param>
        /// <param name="isDeletion">Is the job definition for resource deletion?</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Id of the new added job definition</returns>
        Task<int> AddJobDefinition(int projectId, string name, bool isDefault, bool isDeletion, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Rename a job definition
        /// </summary>
        /// <param name="id">Id of the job definition</param>
        /// <param name="newName">New name of the job definition</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task RenameJobDefinition(int id, string newName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Set a job definition as the default in project
        /// </summary>
        /// <param name="id">Id of the job definition</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task SetJobDefinitionAsDefault(int id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a default job definition in project
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task<JobDefinition> GetDefaultJobDefinition(int projectId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete a job definition
        /// </summary>
        /// <param name="id">Id of the job definition</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task DeleteJobDefinition(int id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete job definitions in batch
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobIds">Ids of the job definitions to be deleted</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task DeleteJobDefinitions(int projectId, int[] jobIds, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get list of job definitions
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>List of job definitions</returns>
        Task<List<JobDefinition>> GetJobDefinitions(int projectId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a job definition by id
        /// </summary>
        /// <param name="jobDefinitionId">Id of the job definition</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>job definition entity</returns>
        Task<JobDefinition> GetJobDefinitionById(int jobDefinitionId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a job definition by name
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobDefinitionName">Name of the job definition</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task<JobDefinition> GetJobDefinitionByName(int projectId, string jobDefinitionName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a job definition of a project with IsDeletion = true
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>job definition entity</returns>
        Task<JobDefinition> GetDeletionJobDefinition(int projectId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Add a new job task definition
        /// </summary>
        /// <param name="jobTaskDefinition">The job task definition entity</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Id of the new added job task definition</returns>
        Task<int> AddJobTaskDefinition(JobTaskDefinition jobTaskDefinition, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Add new job task definitions for a job definition
        /// </summary>
        /// <param name="jobDefinitionId">The job definition id</param>
        /// <param name="jobTaskDefinitions">A range of job task definitions</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Ids of the new added job task definitions</returns>
        Task<List<int>> AddJobTaskDefinitions(int jobDefinitionId, List<JobTaskDefinition> jobTaskDefinitions, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update a job task definition
        /// </summary>
        /// <param name="editedJobTaskDefinition">Edited job task definition</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task UpdateJobTaskDefinition(JobTaskDefinition editedJobTaskDefinition, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update job task configuration
        /// </summary>
        /// <param name="taskDefinitionId">Id of the job task definition</param>
        /// <param name="jobTaskConfig">Job task configuration</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task UpdateJobTaskConfig(int taskDefinitionId, Dictionary<string, string> jobTaskConfig, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete a job task definition
        /// </summary>
        /// <param name="id">Id of the job task definition</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task DeleteJobTaskDefinition(int id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get list of job task definitions for a job definition
        /// </summary>
        /// <param name="jobDefinitionId">Id of the job definition</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>List of job task definitions</returns>
        Task<List<JobTaskDefinition>> GetJobTaskDefinitions(int jobDefinitionId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a job task definition by id
        /// </summary>
        /// <param name="jobTaskDefinitionId">Id of the job task definition</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The job task definition entity</returns>
        Task<JobTaskDefinition> GetJobTaskDefinitionById(int jobTaskDefinitionId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a job task definition by name
        /// </summary>
        /// <param name="jobDefinitionId">Id of the job definition</param>
        /// <param name="jobTaskDefinitionName">Name of the job task definition</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The job task definition entity</returns>
        Task<JobTaskDefinition> GetJobTaskDefinitionByName(int jobDefinitionId, string jobTaskDefinitionName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Validate the task property and config whether it has satisfy required values
        /// </summary>
        /// <param name="jobDefinition">The job definition object</param>
        /// <param name="jobTaskDefinition">The job task definition object</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task ValidateJobTaskDefinition(JobDefinition jobDefinition, JobTaskDefinition jobTaskDefinition, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Encrypt all secret additional config in a task
        /// </summary>
        /// <param name="jobTaskDefinition">The job task definition object</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task EncryptSecretAdditionalConfig(JobTaskDefinition jobTaskDefinition, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Decrypt the secret additional configs to plain text
        /// </summary>
        /// <param name="jobTaskDefinition">The job task definition object</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task DecryptSecretAdditionalConfigs(JobTaskDefinition jobTaskDefinition, CancellationToken cancellationToken = default(CancellationToken));
    }
}
