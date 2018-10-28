// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Identity;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.JobQueue;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Controllers
{
    [ApiController]
    public class JobQueueController : ControllerBase
    {
        private readonly IJobQueueService _jobQueueService;
        private readonly ICatapultEngineService _engineService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public JobQueueController(IJobQueueService jobQueueService, ICatapultEngineService engineService, IMapper mapper, ILogger<JobQueueController> logger)
        {
            _jobQueueService = jobQueueService;
            _engineService = engineService;
            _mapper = mapper;
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
            _logger.LogInformation("Getting job queues in project {projectId}. Filter: {filter}", projectId, filter);

            try
            {
                var jobs = await _jobQueueService.GetJobQueues(projectId, filter);
                var results = _mapper.Map<List<JobDto>>(jobs);

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
            _logger.LogInformation("Getting job queue {queueId} in project {projectId}", queueId, projectId);

            var job = await _jobQueueService.GetJobQueueById(queueId);
            var result = _mapper.Map<JobDto>(job);
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
            _logger.LogInformation("Creating job queue for project {projectId}. Request body: {@newJobQueue}", projectId, newJobQueue);

            try
            {
                if (projectId != newJobQueue.ProjectId)
                {
                    _logger.LogWarning("Project Id doesn't match");
                    return BadRequest("Project Id doesn't match.");
                }

                var newQueue = _mapper.Map<JobDto>(newJobQueue);

                newQueue.Id = await _jobQueueService.AddJobQueue(newJobQueue.ProjectId,
                    newJobQueue.OriginUrl,
                    newJobQueue.JobType,
                    newJobQueue.JobDefinitionId);

                return CreatedAtRoute("GetJobQueueById", new
                {
                    projectId = newJobQueue.ProjectId,
                    queueId = newQueue.Id
                }, newQueue);
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
            _logger.LogInformation("Cancel job queue {queueId} in project {projectId}", queueId, projectId);

            try
            {
                await _jobQueueService.CancelJobQueue(queueId);

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
            // log as debug so it won't be put to log when min level is Info
            _logger.LogDebug("Checking for job queue");

            var isEngine = User.IsInRole(UserRole.Engine);

            var engineName = "";
            if (isEngine)
                engineName = User.Identity.Name;

            var job = await _jobQueueService.GetFirstUnassignedQueuedJob(engineName);
            var result = _mapper.Map<JobDto>(job);

            // update engine's last seen
            await _engineService.UpdateLastSeen(engineName);

            return Ok(result);
        }

        /// <summary>
        /// Update a job queue
        /// </summary>
        /// <param name="queueId">Id of the job queue</param>
        /// <param name="job">Update job queue request body</param>
        /// <returns></returns>
        [HttpPut("Queue/{queueId}")]
        [Authorize(Policy = AuthorizePolicy.UserRoleEngineAccess)]
        public async Task<IActionResult> UpdateJobQueue(int queueId, UpdateJobDto job)
        {
            _logger.LogInformation("Updating job queue. Request body: {@job}", job);

            try
            {
                if (queueId != job.Id)
                {
                    return BadRequest("Queue Id doesn't match.");
                }                    

                var entity = _mapper.Map<JobQueue>(job);
                await _jobQueueService.UpdateJobQueue(entity);

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
            _logger.LogInformation("Getting status for job queue {queueId} in project {projectId}. Filter: {filter}", queueId, projectId, filter);

            try
            {
                var jobTaskStatus = await _jobQueueService.GetJobTaskStatus(queueId, filter);
                var result = _mapper.Map<List<JobTaskStatusDto>>(jobTaskStatus);

                return Ok(result);
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
        /// <returns>Text log of a job</returns>
        [HttpGet("Project/{projectId}/queue/{queueId}/logs")]
        [Authorize(Policy = AuthorizePolicy.ProjectMaintainerAccess)]
        public async Task<IActionResult> GetJobLogs(int projectId, int queueId)
        {
            _logger.LogInformation("Getting logs for job queue {queueId}", queueId);

            var logs = await _jobQueueService.GetJobLogs(queueId);

            return Ok(logs);
        }
    }
}
