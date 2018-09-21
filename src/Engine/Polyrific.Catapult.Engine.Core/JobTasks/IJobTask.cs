// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;

namespace Polyrific.Catapult.Engine.Core.JobTasks
{
    public interface IJobTask
    {
        /// <summary>
        /// Type of the task
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Id of the project
        /// </summary>
        int ProjectId { get; set; }
        
        /// <summary>
        /// Id of the job task definition
        /// </summary>
        int JobTaskId { get; set; }

        /// <summary>
        /// Provider of the job task definition
        /// </summary>
        string Provider { get; set; }

        /// <summary>
        /// Code of the job queue
        /// </summary>
        string JobQueueCode { get; set; }

        /// <summary>
        /// Set job task configuration
        /// </summary>
        /// <param name="configString">Serialized configuration</param>
        void SetConfig(string configString);

        /// <summary>
        /// Run the main task
        /// </summary>
        /// <returns></returns>
        Task<TaskRunnerResult> RunMainTask();

        /// <summary>
        /// Run the pre-processing task
        /// </summary>
        /// <returns></returns>
        Task<TaskRunnerResult> RunPreprocessingTask();

        /// <summary>
        /// Run the post-processing task
        /// </summary>
        /// <returns></returns>
        Task<TaskRunnerResult> RunPostprocessingTask();
    }
}
