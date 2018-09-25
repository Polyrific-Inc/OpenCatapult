// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CatapultEngine> _logger;

        public CatapultEngine(ICatapultEngineConfig engineConfig, 
            ITaskRunner taskRunner,
            IHealthService healthService,
            IJobQueueService jobQueueService, 
            IJobDefinitionService jobDefinitionService,
            ILogger<CatapultEngine> logger)
        {
            _engineConfig = engineConfig;
            _taskRunner = taskRunner;
            _healthService = healthService;
            _jobQueueService = jobQueueService;
            _jobDefinitionService = jobDefinitionService;
            _logger = logger;
        }

        public async Task<bool> CheckApiConnection()
        {
            _logger.LogInformation($"Checking connection to API {_engineConfig.ApiUrl}.");
            return await _healthService.CheckHealthSecure();
        }

        public async Task ExecuteJob(JobDto jobQueue)
        {
            _logger.LogInformation($"Executing job queue {jobQueue.Code}.");
            
            var jobTasks = await _jobDefinitionService.GetJobTaskDefinitions(jobQueue.ProjectId, jobQueue.JobDefinitionId ?? 0);
            
            var workingLocation = Path.Combine(_engineConfig.WorkingLocation, jobQueue.Code);
            await _taskRunner.Run(jobQueue.ProjectId, jobQueue.Code, jobTasks, _engineConfig.PluginsLocation, workingLocation);
        }

        public async Task<JobDto> GetJobInQueue()
        {
            _logger.LogInformation("Trying to get a job in queue.");

            return await _jobQueueService.CheckJob();
        }
        
    }
}
