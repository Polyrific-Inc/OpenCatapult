// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using Polyrific.Catapult.Shared.Dto.Project;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Engine.Core.JobTasks
{
    public abstract class BaseJobTask<TTaskConfig> where TTaskConfig : BaseJobTaskConfig, new()
    {
        private readonly IProjectService _projectService;

        /// <summary>
        /// Instantiate job task
        /// </summary>
        /// <param name="projectService">Instance of <see cref="IProjectService"/></param>
        /// <param name="logger"></param>
        protected BaseJobTask(IProjectService projectService, ILogger logger)
        {
            _projectService = projectService;

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
        /// Provider of the job task definition
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// Logger
        /// </summary>
        protected ILogger Logger { get; set; }

        /// <summary>
        /// Job task configuration
        /// </summary>
        protected TTaskConfig TaskConfig { get; private set; }

        /// <summary>
        /// Additional configurations which are required by specific providers
        /// </summary>
        public Dictionary<string, string> AdditionalConfigs { get; set; }

        private ProjectDto _project;
        /// <summary>
        /// Project object of the task
        /// </summary>
        protected ProjectDto Project
        {
            get => _project ?? (_project = _projectService.GetProject(ProjectId).Result);
            set => _project = value;
        }

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
        /// <param name="previousTasksOutputValues">Output values from the previous tasks</param>
        /// <returns></returns>
        public abstract Task<TaskRunnerResult> RunMainTask(Dictionary<string, string> previousTasksOutputValues);

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

        protected Task LoadRequiredServicesToAdditionalConfigs(string[] serviceNames)
        {
            if (AdditionalConfigs == null)
                AdditionalConfigs = new Dictionary<string, string>();

            // TODO: load service properties to the configs

            return Task.CompletedTask;
        }
    }
}
