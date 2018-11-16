// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.JobQueue;
using Polyrific.Catapult.Shared.Dto.Project;

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

        /// <summary>
        /// Execute a job task
        /// </summary>
        /// <returns></returns>
        Task ExecuteTask(string taskType, string providerName, Dictionary<string, string> configs, NewProjectDto project = null);
    }
}
