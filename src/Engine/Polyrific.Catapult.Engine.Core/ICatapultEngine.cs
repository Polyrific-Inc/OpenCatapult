// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.JobQueue;

namespace Polyrific.Catapult.Engine.Core
{
    public interface ICatapultEngine
    {
        /// <summary>
        /// Check connection to API
        /// </summary>
        /// <returns></returns>
        Task<bool> CheckApiConnection();

        /// <summary>
        /// Get a job in queue to execute
        /// </summary>
        /// <returns>Job in queue</returns>
        Task<JobDto> GetJobInQueue();

        /// <summary>
        /// Execute a job in queue
        /// </summary>
        /// <param name="jobQueue">Job in queue</param>
        /// <returns></returns>
        Task ExecuteJob(JobDto jobQueue);
    }
}