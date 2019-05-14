// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Core.Services
{
    public interface IJobQueueService
    {
        /// <summary>
        /// Queue a new job
        /// </summary>
        /// <param name="projectId">The project which we queue</param>
        /// <param name="originUrl">The origin url where the job is queue</param>
        /// <param name="jobType">The job type</param>
        /// <param name="jobDefinitionId">The job definition id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The new job queue id</returns>
        Task<int> AddJobQueue(int projectId, string originUrl, string jobType, int? jobDefinitionId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Queue a new job of default job definition
        /// </summary>
        /// <param name="projectId">The project which we queue</param>
        /// <param name="originUrl">The origin url where the job is queue</param>
        /// <param name="jobType">The job type</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The new job queue id</returns>
        Task<int> AddDefaultJobQueue(int projectId, string originUrl, string jobType, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// update a job queue
        /// </summary>
        /// <param name="updatedJob">The updated job queue</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task UpdateJobQueue(JobQueue updatedJob, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Cancel a job queue
        /// </summary>
        /// <param name="id">Cancel a job queue</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task CancelJobQueue(int id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get list of job queues
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="filter">the filter type: all | current | past | succeeded | failed</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>List of job queues</returns>
        Task<List<JobQueue>> GetJobQueues(int projectId, string filter, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get list of job queues by status
        /// </summary>
        /// <param name="status">Status of the job queue</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>List of job queues</returns>
        Task<List<JobQueue>> GetJobQueuesByStatus(string status, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a job queue by id
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobQueueId">Id of the job queue</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>job queue entity</returns>
        Task<JobQueue> GetJobQueueById(int projectId, int jobQueueId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a job queue by code
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobQueueCode">Code of the job queue</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>job queue entity</returns>
        Task<JobQueue> GetJobQueueByCode(int projectId, string jobQueueCode, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get the execution status of a job
        /// </summary>
        /// <param name="id">The id of the job queue</param>
        /// <param name="filter">The filter type: all | latest</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The list of task status</returns>
        Task<List<JobTaskStatus>> GetJobTaskStatus(int id, string filter, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get the execution logs of a job
        /// </summary>
        /// <param name="projectId">The id of the project</param>
        /// <param name="jobQueueId">The job queue id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The log text</returns>
        Task<string> GetJobLogs(int projectId, int jobQueueId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get the execution logs of a task
        /// </summary>
        /// <param name="projectId">The id of the project</param>
        /// <param name="jobQueueId">The job queue id</param>
        /// <param name="taskName">The task name</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task<string> GetTaskLogs(int projectId, int jobQueueId, string taskName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get the first unassigned queued job to be run
        /// </summary>
        /// <param name="engine">Requesting engine name</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The job queue entity</returns>
        Task<JobQueue> GetFirstUnassignedQueuedJob(string engine, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Send the notification to project member related to the job queue
        /// </summary>
        /// <param name="jobQueueId">Id of the job queue</param>
        /// <param name="webUrl">The base web url</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task SendNotification(int jobQueueId, string webUrl, CancellationToken cancellationToken = default(CancellationToken));
    }
}
