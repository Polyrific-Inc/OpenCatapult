// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Engine.Core.JobLogger;
using Polyrific.Catapult.Engine.Core.Utils;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.JobDefinition;
using Polyrific.Catapult.Shared.Dto.JobQueue;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Engine.Core
{
    public class CatapultEngine : ICatapultEngine
    {
        private readonly ICatapultEngineConfig _engineConfig;
        private readonly ITaskRunner _taskRunner;
        private readonly IHealthService _healthService;
        private readonly IJobQueueService _jobQueueService;
        private readonly IJobDefinitionService _jobDefinitionService;
        private readonly IJobLogWriter _jobLogWriter;
        private readonly ILogger<CatapultEngine> _logger;

        public CatapultEngine(ICatapultEngineConfig engineConfig, 
            ITaskRunner taskRunner,
            IHealthService healthService,
            IJobQueueService jobQueueService, 
            IJobDefinitionService jobDefinitionService,
            IJobLogWriter jobLogWriter,
            ILogger<CatapultEngine> logger)
        {
            _engineConfig = engineConfig;
            _taskRunner = taskRunner;
            _healthService = healthService;
            _jobQueueService = jobQueueService;
            _jobDefinitionService = jobDefinitionService;
            _jobLogWriter = jobLogWriter;
            _logger = logger;
        }

        public async Task<bool> CheckApiConnection()
        {
            _logger.LogInformation($"Checking connection to API {_engineConfig.ApiUrl}.");
            return await _healthService.CheckHealthSecure();
        }

        public async Task ExecuteJob(JobDto jobQueue)
        {
            using (_logger.BeginScope(new JobScope(jobQueue.Id)))
            {
                try
                {
                    _logger.LogInformation($"Executing job queue {jobQueue.Code}.");

                    var jobTasks = await _jobDefinitionService.GetJobTaskDefinitions(jobQueue.ProjectId, jobQueue.JobDefinitionId ?? 0);

                    var workingLocation = Path.Combine(_engineConfig.WorkingLocation, jobQueue.Code);
                    var result = await _taskRunner.Run(jobQueue.ProjectId, jobQueue, jobTasks, _engineConfig.PluginsLocation, workingLocation);

                    if (result.Values.Any(t => t.IsSuccess && t.StopTheProcess))
                        jobQueue.Status = JobStatus.Pending;
                    else if (result.Values.Any(t => !t.IsSuccess))
                        jobQueue.Status = JobStatus.Error;
                    else
                        jobQueue.Status = JobStatus.Completed;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    jobQueue.Status = JobStatus.Error;
                }

                await _jobQueueService.UpdateJobQueue(jobQueue.Id, new UpdateJobDto
                {
                    Id = jobQueue.Id,
                    CatapultEngineId = jobQueue.CatapultEngineId,
                    CatapultEngineIPAddress = jobQueue.CatapultEngineIPAddress,
                    CatapultEngineMachineName = jobQueue.CatapultEngineMachineName,
                    CatapultEngineVersion = jobQueue.CatapultEngineVersion,
                    JobType = jobQueue.JobType,
                    Status = jobQueue.Status,
                    JobTasksStatus = jobQueue.JobTasksStatus,
                    OutputValues = jobQueue.OutputValues
                });
                
                await _jobLogWriter.EndJobLog(jobQueue.Id);
            }
        }

        public async Task ExecuteTask(string taskType, string providerName, Dictionary<string, string> configs)
        {
            const string jobQueueCode = "001";

            using (_logger.BeginScope(new JobScope(1)))
            {
                var builtInConfigs = new Dictionary<string, string>();
                var additionalConfigs = new Dictionary<string, string>();

                var taskConfigNames = JobTaskConfigUtil.GetTaskConfigNames(taskType);
                foreach (var config in configs)
                {
                    if (taskConfigNames.Contains(config.Key))
                        builtInConfigs.Add(config.Key, config.Value);
                    else
                        additionalConfigs.Add(config.Key, config.Value);
                }

                var jobTask = new JobTaskDefinitionDto
                {
                    Type = taskType,
                    Provider = providerName,
                    Configs = builtInConfigs,
                    AdditionalConfigs = additionalConfigs
                };
                var workingLocation = Path.Combine(_engineConfig.WorkingLocation, jobQueueCode);

                try
                {
                    await _taskRunner.Run(jobTask, _engineConfig.PluginsLocation, workingLocation);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
        }

        public async Task<JobDto> GetJobInQueue()
        {
            _logger.LogInformation("Trying to get a job in queue.");

            return await _jobQueueService.CheckJob();
        }
    }
}
