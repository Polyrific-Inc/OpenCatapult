// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper _mapper;

        public JobQueueController(IJobQueueService jobQueueService, IMapper mapper)
        {
            _jobQueueService = jobQueueService;
            _mapper = mapper;
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
            try
            {
                var jobs = await _jobQueueService.GetJobQueues(projectId, filter);
                var results = _mapper.Map<List<JobDto>>(jobs);

                return Ok(results);
            }
            catch (FilterTypeNotFoundException ex)
            {
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
            try
            {
                if (projectId != newJobQueue.ProjectId)
                    return BadRequest("project Id doesn't match.");

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
                return BadRequest(jobEx.Message);
            }
            catch (ProjectNotFoundException projEx)
            {
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
            try
            {
                await _jobQueueService.CancelJobQueue(queueId);

                return Ok();
            }
            catch (CancelCompletedJobException ex)
            {
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
            var isEngine = User.IsInRole(UserRole.Engine);

            var engineName = "";
            if (isEngine)
                engineName = User.Identity.Name;

            var job = await _jobQueueService.GetFirstUnassignedQueuedJob(engineName);
            var result = _mapper.Map<JobDto>(job);
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
            try
            {
                if (queueId != job.Id)
                    return BadRequest("Queue Id doesn't match.");

                var entity = _mapper.Map<JobQueue>(job);
                await _jobQueueService.UpdateJobQueue(entity);

                return Ok();
            }
            catch (JobProcessedByOtherEngineException ex)
            {
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
            try
            {
                var jobTaskStatus = await _jobQueueService.GetJobTaskStatus(queueId, filter);
                var result = _mapper.Map<List<JobTaskStatusDto>>(jobTaskStatus);

                return Ok(result);
            }
            catch (FilterTypeNotFoundException ex)
            {
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
            var logs = await _jobQueueService.GetJobLogs(queueId);

            return Ok(logs);
        }
    }
}