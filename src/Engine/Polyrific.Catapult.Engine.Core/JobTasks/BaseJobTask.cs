// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polyrific.Catapult.Engine.Core.Exceptions;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using Polyrific.Catapult.Shared.Dto.Project;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Engine.Core.JobTasks
{
    public abstract class BaseJobTask<TTaskConfig> where TTaskConfig : BaseJobTaskConfig, new()
    {
        private readonly IProjectService _projectService;
        private readonly IExternalServiceService _externalServiceService;

        /// <summary>
        /// Instantiate job task
        /// </summary>
        /// <param name="projectService">Instance of <see cref="IProjectService"/></param>
        /// <param name="logger"></param>
        protected BaseJobTask(IProjectService projectService, IExternalServiceService externalServiceService, ILogger logger)
        {
            _projectService = projectService;

            _externalServiceService = externalServiceService;

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
            get => _project == null || _project.Id != ProjectId ? (_project = _projectService.GetProject(ProjectId).Result) : _project;
            set => _project = value;
        }

        private Dictionary<string, string> _configs;

        /// <summary>
        /// Set job task configuration
        /// </summary>
        /// <param name="configs">Configurations</param>
        /// <param name="workingLocation">Location of the working directory</param>
        public void SetConfig(Dictionary<string, string> configs, string workingLocation)
        {
            _configs = configs;
            var configString = JsonConvert.SerializeObject(configs);
            TaskConfig = JsonConvert.DeserializeObject<TTaskConfig>(configString) ?? new TTaskConfig();
            TaskConfig.WorkingLocation = workingLocation;
        }

        /// <summary>
        /// Reload the project of task instance
        /// </summary>
        public virtual void ReloadProject()
        {
            _project = null;
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

        protected async Task LoadRequiredServicesToAdditionalConfigs(string[] serviceNames)
        {
            if (AdditionalConfigs == null)
                AdditionalConfigs = new Dictionary<string, string>();
            
            foreach (var serviceType in serviceNames)
            {
                if (_configs.TryGetValue($"{serviceType}ExternalService", out var externalServiceName))
                {
                    var externalService = await _externalServiceService.GetExternalServiceByName(externalServiceName);

                    if (externalService != null)
                    {
                        foreach (var serviceProp in externalService.Config)
                        {
                            if (!AdditionalConfigs.ContainsKey(serviceProp.Key))
                                AdditionalConfigs.Add(serviceProp.Key, serviceProp.Value);
                        }                            
                    }
                    else
                        throw new ExternalServiceNotFoundException(externalServiceName);
                }
                else
                {
                    throw new InvalidExternalServiceTypeException(serviceType, JobTaskId);
                }
            }
        }
    }
}
