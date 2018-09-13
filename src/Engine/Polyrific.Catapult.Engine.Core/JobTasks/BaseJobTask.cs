// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Engine.Core.JobTasks
{
    public abstract class BaseJobTask<TTaskConfig> where TTaskConfig : BaseJobTaskConfig, new()
    {
        protected BaseJobTask(ILogger logger)
        {
            
            Logger = logger;
        }

        /// <summary>
        /// Id of the project
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Id of the job task definition
        /// </summary>
        public int JobTaskId { get; set; }

        /// <summary>
        /// Code of the job queue
        /// </summary>
        public string JobQueueCode { get; set; }
        
        /// <summary>
        /// Logger
        /// </summary>
        protected ILogger Logger { get; set; }

        /// <summary>
        /// Job task configuration
        /// </summary>
        protected TTaskConfig TaskConfig { get; private set; }

        /// <summary>
        /// Set job task configuration
        /// </summary>
        /// <param name="configString">Serialized configuration</param>
        public void SetConfig(string configString)
        {
            TaskConfig = JsonConvert.DeserializeObject<TTaskConfig>(configString) ?? new TTaskConfig();
        }

        /// <summary>
        /// Type of the job task
        /// </summary>
        public abstract string Type { get; }

        /// <summary>
        /// Run the main task
        /// </summary>
        /// <returns></returns>
        public abstract Task<TaskRunnerResult> RunMainTask();

        /// <summary>
        /// Run the pre-processing task
        /// </summary>
        /// <returns></returns>
        public virtual Task<TaskRunnerResult> RunPreprocessingTask()
        {
            return Task.FromResult(new TaskRunnerResult());
        }

        /// <summary>
        /// Run the post-processing task
        /// </summary>
        /// <returns></returns>
        public virtual Task<TaskRunnerResult> RunPostprocessingTask()
        {
            return Task.FromResult(new TaskRunnerResult());
        }
    }
}