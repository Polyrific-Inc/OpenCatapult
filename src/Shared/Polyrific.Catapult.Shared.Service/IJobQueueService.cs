// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.JobQueue;

namespace Polyrific.Catapult.Shared.Service
{
    public interface IJobQueueService
    {
        /// <summary>
        /// Get list of jobs in queue
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="filter">Status filter</param>
        /// <returns></returns>
        Task<List<JobDto>> GetJobQueues(int projectId, string filter);

        /// <summary>
        /// Get a job in queue
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="queueId">Id of the job queue</param>
        /// <returns></returns>
        Task<JobDto> GetJobQueue(int projectId, int queueId);

        /// <summary>
        /// Get a job in queue
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="queueCode">Code of the job queue</param>
        /// <returns></returns>
        Task<JobDto> GetJobQueue(int projectId, string queueCode);

        /// <summary>
        /// Add a job to queue
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="newJobQueue">Job details</param>
        /// <returns></returns>
        Task<JobDto> CreateJobQueue(int projectId, NewJobDto newJobQueue);

        /// <summary>
        /// Cancel a job in queue
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="queueId">Id of the job queue</param>
        /// <returns></returns>
        Task CancelJobQueue(int projectId, int queueId);

        /// <summary>
        /// Get the top most job in queue
        /// </summary>
        /// <returns></returns>
        Task<JobDto> CheckJob();

        /// <summary>
        /// Update job queue details
        /// </summary>
        /// <param name="queueId">Id of the job queue</param>
        /// <param name="job">Job details</param>
        /// <returns></returns>
        Task UpdateJobQueue(int queueId, UpdateJobDto job);

        /// <summary>
        /// Update job queue details using project authorization
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="queueId">Id of the job queue</param>
        /// <param name="job">Job details</param>
        /// <returns></returns>
        Task UpdateJobQueue(int projectId, int queueId, UpdateJobDto job);

        /// <summary>
        /// Get execution status of a job in queue
        /// </summary>
        /// <param name="queueId">Id of the job queue</param>
        /// <param name="filter">Status filter</param>
        /// <returns></returns>
        Task<List<JobTaskStatusDto>> GetJobQueueStatus(int projectId, int queueId, string filter);

        /// <summary>
        /// Get execution logs of a job in queue
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="queueId">Id of the job queue</param>
        /// <returns></returns>
        Task<string> GetJobLogs(int projectId, int queueId);
    }
}
