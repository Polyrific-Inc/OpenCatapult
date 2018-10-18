// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
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
        /// Additional configurations which are required by specific providers
        /// </summary>
        Dictionary<string, string> AdditionalConfigs { get; set; }

        /// <summary>
        /// Set job task configuration
        /// </summary>
        /// <param name="configs">Configurations</param>
        /// <param name="workingLocation">Location of the working directory</param>
        void SetConfig(Dictionary<string, string> configs, string workingLocation);

        /// <summary>
        /// Reload the project of task instance
        /// </summary>
        void ReloadProject();

        /// <summary>
        /// Run the main task
        /// </summary>
        /// <param name="previousTasksOutputValues">Output values from the previous tasks</param>
        /// <returns></returns>
        Task<TaskRunnerResult> RunMainTask(Dictionary<string, string> previousTasksOutputValues);

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
