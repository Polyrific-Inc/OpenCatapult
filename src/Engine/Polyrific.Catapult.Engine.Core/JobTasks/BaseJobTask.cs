// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polyrific.Catapult.Engine.Core.Exceptions;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using Polyrific.Catapult.Shared.Common;
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
        /// <param name="pluginManager"></param>
        /// <param name="logger"></param>
        /// <param name="externalServiceService"></param>
        protected BaseJobTask(IProjectService projectService, IExternalServiceService externalServiceService, IPluginManager pluginManager, ILogger logger)
        {
            _projectService = projectService;

            _externalServiceService = externalServiceService;

            PluginManager = pluginManager;
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
        /// Plugin Manager
        /// </summary>
        protected IPluginManager PluginManager { get; set; }

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

        protected async Task<Dictionary<string, object>> InvokeTaskProvider(string pluginDll, string pluginArgs)
        {
            Dictionary<string, object> result = null;

            var startInfo = new ProcessStartInfo()
            {
                FileName = "dotnet",
                Arguments = $"\"{pluginDll}\" {pluginArgs}",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (var process = Process.Start(startInfo))
            {
                if (process != null)
                {
                    Console.WriteLine($"[Master] Command: {process.StartInfo.FileName} {process.StartInfo.Arguments}");

                    var reader = process.StandardOutput;
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();

                        var tags = line.GetPrefixTags();
                        if (tags.Length > 0 && tags[0] == "OUTPUT")
                        {
                            var output = line.Replace("[OUTPUT] ", "");
                            result = JsonConvert.DeserializeObject<Dictionary<string, object>>(output);
                        } else if (tags.Length > 0 && tags[0] == "LOG")
                        {
                            SubmitLog(line.Replace("[LOG]", ""));
                        }
                        else
                            Console.WriteLine($"[Plugin] {line}");
                    }
                }
            }

            return result ?? new Dictionary<string, object>();
        }

        private void SubmitLog(string logMessage)
        {
            var tags = logMessage.GetPrefixTags();
            if (tags.Length > 0)
            {
                switch (tags[0])
                {
                    case "Critical":
                        Logger.LogCritical(logMessage.Replace("[Critical]", ""));
                        break;
                    case "Error":
                        Logger.LogError(logMessage.Replace("[Error]", ""));
                        break;
                    case "Warning":
                        Logger.LogWarning(logMessage.Replace("[Warning]", ""));
                        break;
                    case "Information":
                        Logger.LogInformation(logMessage.Replace("[Information]", ""));
                        break;
                    case "Debug":
                        Logger.LogDebug(logMessage.Replace("[Debug]", ""));
                        break;
                    case "Trace":
                        Logger.LogTrace(logMessage.Replace("[Trace]", ""));
                        break;
                }
            }
        }
    }
}
