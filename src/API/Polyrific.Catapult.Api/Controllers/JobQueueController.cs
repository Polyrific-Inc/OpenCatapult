// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Identity;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.JobQueue;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Controllers
{
    [ApiController]
    public class JobQueueController : ControllerBase
    {
        private readonly IJobQueueService _jobQueueService;
        private readonly ICatapultEngineService _engineService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public JobQueueController(IJobQueueService jobQueueService, ICatapultEngineService engineService, 
            IMapper mapper, IConfiguration configuration, ILogger<JobQueueController> logger)
        {
            _jobQueueService = jobQueueService;
            _engineService = engineService;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Get list of job queues
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="filter">Filter the result [all | current | past | succeeded | failed]</param>
        /// <returns>List of job queues</returns>
        [HttpGet("Project/{projectId}/queue", Name = "GetJobQueues")]
        [Authorize(Policy = AuthorizePolicy.ProjectMaintainerAccess)]
        public async Task<IActionResult> GetJobQueues(int projectId, string filter = JobQueueFilterType.All)
        {
            _logger.LogRequest("Getting job queues in project {projectId}. Filter: {filter}", projectId, filter);

            try
            {
                var jobs = await _jobQueueService.GetJobQueues(projectId, filter);
                var results = _mapper.Map<List<JobDto>>(jobs);

                _logger.LogResponse("Job queues in project {projectId} retrieved. Response body: {@results}", projectId, results);

                return Ok(results);
            }
            catch (FilterTypeNotFoundException ex)
            {
                _logger.LogWarning(ex, "Filter type not found");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get a job queue
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="queueId">Id of the job queue</param>
        /// <returns>The job queue object</returns>
        [HttpGet("Project/{projectId}/queue/{queueId}", Name = "GetJobQueueById")]
        [Authorize(Policy = AuthorizePolicy.ProjectMaintainerAccess)]
        public async Task<IActionResult> GetJobQueue(int projectId, int queueId)
        {
            _logger.LogRequest("Getting job queue {queueId} in project {projectId}", queueId, projectId);

            var job = await _jobQueueService.GetJobQueueById(projectId, queueId);
            var result = _mapper.Map<JobDto>(job);

            _logger.LogResponse("Job queue {queueId} in project {projectId} retrieved. Response body: {@result}", queueId, projectId, result);

            return Ok(result);
        }

        /// <summary>
        /// Get a job queue by code
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="queueCode">Code of the job queue</param>
        /// <returns>The job queue object</returns>
        [HttpGet("Project/{projectId}/queue/code/{queueCode}", Name = "GetJobQueueByCode")]
        [Authorize(Policy = AuthorizePolicy.ProjectMaintainerAccess)]
        public async Task<IActionResult> GetJobQueueByCode(int projectId, string queueCode)
        {
            _logger.LogRequest("Getting job queue {queueCode} in project {projectId}", queueCode, projectId);

            var job = await _jobQueueService.GetJobQueueByCode(projectId, queueCode);
            var result = _mapper.Map<JobDto>(job);

            _logger.LogResponse("Job queue {queueCode} in project {projectId} retrieved. Response body: {@result}", queueCode, projectId, result);

            return Ok(result);
        }

        /// <summary>
        /// Create a job queue
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="newJobQueue">Create job queue request body</param>
        /// <returns></returns>
        [HttpPost("Project/{projectId}/queue", Name = "CreateJobQueue")]
        [ProducesResponseType(201)]
        [Authorize(Policy = AuthorizePolicy.ProjectMaintainerAccess)]
        public async Task<IActionResult> CreateJobQueue(int projectId, NewJobDto newJobQueue)
        {
            _logger.LogRequest("Creating job queue for project {projectId}. Request body: {@newJobQueue}", projectId, newJobQueue);

            try
            {
                if (projectId != newJobQueue.ProjectId)
                {
                    _logger.LogWarning("Project Id doesn't match");
                    return BadRequest("Project Id doesn't match.");
                }
                
                var queueId = await _jobQueueService.AddJobQueue(newJobQueue.ProjectId,
                    newJobQueue.OriginUrl,
                    newJobQueue.JobType,
                    newJobQueue.JobDefinitionId);

                var job = await _jobQueueService.GetJobQueueById(projectId, queueId);
                var result = _mapper.Map<JobDto>(job);

                _logger.LogResponse("Job queue in project {projectId} created. Response body: {@result}", projectId, result);

                return CreatedAtRoute("GetJobQueueById", new
                {
                    projectId = newJobQueue.ProjectId,
                    queueId
                }, result);
            }
            catch (JobQueueInProgressException jobEx)
            {
                _logger.LogWarning(jobEx, "Job queue in progress");
                return BadRequest(jobEx.Message);
            }
            catch (ProjectNotFoundException projEx)
            {
                _logger.LogWarning(projEx, "Project not found");
                return BadRequest(projEx.Message);
            }
        }

        /// <summary>
        /// Cancel an in progress job
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="queueId">Id of the job queue</param>
        /// <returns></returns>
        [HttpPost("Project/{projectId}/queue/{queueId}/cancel")]
        [Authorize(Policy = AuthorizePolicy.ProjectMaintainerAccess)]
        public async Task<IActionResult> CancelJobQueue(int projectId, int queueId)
        {
            _logger.LogRequest("Cancel job queue {queueId} in project {projectId}", queueId, projectId);

            try
            {
                await _jobQueueService.CancelJobQueue(queueId);

                _logger.LogResponse("Job queue {queueId} in project {projectId} cancelled", queueId, projectId);

                return Ok();
            }
            catch (CancelCompletedJobException ex)
            {
                _logger.LogWarning(ex, "Cannot cancel a completed job");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Check whether there's a queued job 
        /// </summary>
        /// <returns>The top job to be run</returns>
        [HttpGet("Queue")]
        [Authorize(Policy = AuthorizePolicy.UserRoleEngineAccess)]
        public async Task<IActionResult> CheckJob()
        {
            if (!await CheckEngineStatus())
                return Unauthorized();

            // log as debug so it won't be put to log when min level is Info
            _logger.LogDebug("Checking for job queue");

            var isEngine = User.IsInRole(UserRole.Engine);

            var engineName = "";
            if (isEngine)
                engineName = User.Identity.Name;

            var job = await _jobQueueService.GetFirstUnassignedQueuedJob(engineName);
            var result = _mapper.Map<JobDto>(job);

            // update engine's last seen
            await _engineService.UpdateLastSeen(engineName, GetEngineVersion(Request.Headers["User-Agent"].ToString()));
            
            _logger.LogResponse("Queued job queue checked. Response body: {@result}", result);

            return Ok(result);
        }

        /// <summary>
        /// Update a job queue
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="queueId">Id of the job queue</param>
        /// <param name="job">Update job queue request body</param>
        /// <returns></returns>
        [HttpPut("Project/{projectId}/queue/{queueId}")]
        [Authorize(Policy = AuthorizePolicy.ProjectMaintainerAccess)]
        public async Task<IActionResult> UpdateJobQueue(int projectId, int queueId, UpdateJobDto job)
        {
            _logger.LogRequest("Update job queue {queueId} in project {projectId}", queueId, projectId);

            if (queueId != job.Id)
            {
                return BadRequest("Queue Id doesn't match.");
            }

            var entity = _mapper.Map<JobQueue>(job);
            entity.ProjectId = projectId;
            await _jobQueueService.UpdateJobQueue(entity);

            _logger.LogResponse("Job queue {queueId} in project {projectId} updated", queueId, projectId);

            return Ok();
        }

        /// <summary>
        /// Update a job queue
        /// </summary>
        /// <param name="queueId">Id of the job queue</param>
        /// <param name="job">Update job queue request body</param>
        /// <returns></returns>
        [HttpPut("Queue/{queueId}")]
        [Authorize(Policy = AuthorizePolicy.UserRoleBasicEngineAccess)]
        public async Task<IActionResult> UpdateJobQueue(int queueId, UpdateJobDto job)
        {
            if (!await CheckEngineStatus())
                return Unauthorized();
                
            _logger.LogRequest("Updating job queue. Request body: {@job}", job);

            try
            {
                if (queueId != job.Id)
                {
                    return BadRequest("Queue Id doesn't match.");
                }                    

                var entity = _mapper.Map<JobQueue>(job);
                await _jobQueueService.UpdateJobQueue(entity);


                _logger.LogResponse("Job queue {queueId} in project {projectId} updated", queueId);

                return Ok();
            }
            catch (JobProcessedByOtherEngineException ex)
            {
                _logger.LogWarning(ex, "Job is processed by other engine");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get job queue status
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="queueId">Id of the job queue</param>
        /// <param name="filter">Filter the result [all | latest]</param>
        /// <returns>The list of job queue status</returns>
        [HttpGet("Project/{projectId}/queue/{queueId}/status")]
        [Authorize(Policy = AuthorizePolicy.ProjectMaintainerAccess)]
        public async Task<IActionResult> GetJobQueueStatus(int projectId, int queueId, string filter = JobTaskStatusFilterType.All)
        {
            _logger.LogRequest("Getting status for job queue {queueId} in project {projectId}. Filter: {filter}", queueId, projectId, filter);

            try
            {
                var jobTaskStatus = await _jobQueueService.GetJobTaskStatus(queueId, filter);
                var results = _mapper.Map<List<JobTaskStatusDto>>(jobTaskStatus);

                _logger.LogResponse("Job queue status list for job queue {queueId} in project {projectId} retrieved. Response body: {@results}", queueId, projectId, results);

                return Ok(results);
            }
            catch (FilterTypeNotFoundException ex)
            {
                _logger.LogWarning(ex, "Filter type not found");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get a job log
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="queueId">Id of the job queue</param>
        /// <param name="taskName">Name of the task</param>
        /// <returns>Text log of a job</returns>
        [HttpGet("Project/{projectId}/queue/{queueId}/task/name/{taskName}/logs")]
        [Authorize(Policy = AuthorizePolicy.ProjectMaintainerAccess)]
        public async Task<IActionResult> GetTaskLogs(int projectId, int queueId, string taskName)
        {
            _logger.LogRequest("Getting logs for task {taskName} in job queue {queueId}", taskName, queueId);

            var logs = await _jobQueueService.GetTaskLogs(projectId, queueId, taskName);

            _logger.LogResponse("Log for task {taskName} in job queue {queueId} in project {projectId} retrieved", taskName, queueId, projectId);

            return Ok(logs);
        }

        /// <summary>
        /// Get a job log
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="queueId">Id of the job queue</param>
        /// <returns>Text log of a job</returns>
        [HttpGet("Project/{projectId}/queue/{queueId}/logs")]
        [Authorize(Policy = AuthorizePolicy.ProjectMaintainerAccess)]
        public async Task<IActionResult> GetJobLogs(int projectId, int queueId)
        {
            _logger.LogRequest("Getting logs for job queue {queueId}", queueId);

            var logs = await _jobQueueService.GetJobLogs(projectId, queueId);

            _logger.LogResponse("Log for job queue {queueId} in project {projectId} retrieved", queueId, projectId);

            return Ok(logs);
        }

        /// <summary>
        /// Send notification to project member about the status of the job queue
        /// </summary>
        /// <param name="queueId">Id of the queue</param>
        /// <returns></returns>
        [HttpPost("Queue/{queueId}/send-notification")]
        [Authorize(Policy = AuthorizePolicy.UserRoleEngineAccess)]
        public async Task<IActionResult> SendNotification(int queueId)
        {
            _logger.LogRequest("Sending email notification for job queue {queueId}", queueId);

            await _jobQueueService.SendNotification(queueId, _configuration[ConfigurationKey.WebUrl]);

            _logger.LogResponse("Notification for job queue {queueId} sent", queueId);

            return Ok();
        }

        private string GetEngineVersion(string userAgent)
        {
            if (!string.IsNullOrEmpty(userAgent))
            {
                var appNameVersion = userAgent.Split(' ').FirstOrDefault();

                if (appNameVersion != null)
                {
                    return appNameVersion.Split('/').LastOrDefault();
                }
            }

            return null;
        }

        private async Task<bool> CheckEngineStatus()
        {
            var engine = await _engineService.GetCatapultEngine(User.Identity.Name);

            return engine != null ? engine.IsActive : false;
        }
    }
}
