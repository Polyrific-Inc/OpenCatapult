// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Identity;
using Polyrific.Catapult.Shared.Dto.JobDefinition;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthorizePolicy.ProjectContributorAccess)]
    public class JobDefinitionController : ControllerBase
    {
        private readonly IJobDefinitionService _jobDefinitionService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public JobDefinitionController(IJobDefinitionService jobDefinitionService, IMapper mapper, ILogger<JobDefinitionController> logger)
        {
            _jobDefinitionService = jobDefinitionService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get job definitions of a project
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <returns>List of job definitions</returns>
        [HttpGet("Project/{projectId}/job", Name = "GetJobDefinitions")]
        public async Task<IActionResult> GetJobDefinitions(int projectId)
        {
            _logger.LogInformation("Getting job definitions in project {projectId}", projectId);

            var jobDefinitions = await _jobDefinitionService.GetJobDefinitions(projectId);
            var results = _mapper.Map<List<JobDefinitionDto>>(jobDefinitions);

            return Ok(results);
        }

        /// <summary>
        /// Create a new job definition for a project
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="newJobDefinition">Create job definition request body</param>
        /// <returns></returns>
        [HttpPost("Project/{projectId}/job", Name = "CreateJobDefinition")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateJobDefinition(int projectId, CreateJobDefinitionDto newJobDefinition)
        {
            _logger.LogInformation("Creating job definition in project {projectId}. Request body: {@newJobDefinition}", projectId, newJobDefinition);

            try
            {
                var newJobDefinitionResponse = _mapper.Map<JobDefinitionDto>(newJobDefinition);
                newJobDefinitionResponse.ProjectId = projectId;
                newJobDefinitionResponse.Id = await _jobDefinitionService.AddJobDefinition(projectId,
                    newJobDefinition.Name,
                    newJobDefinition.IsDeletion);

                return CreatedAtRoute("GetJobDefinitionById", new
                {
                    projectId,
                    jobId = newJobDefinitionResponse.Id
                }, newJobDefinitionResponse);
            }
            catch (DuplicateJobDefinitionException dupEx)
            {
                _logger.LogWarning(dupEx, "Duplicate job definition name");
                return BadRequest(dupEx.Message);
            }
            catch (ProjectNotFoundException projEx)
            {
                _logger.LogWarning(projEx, "Project not found");
                return BadRequest(projEx.Message);
            }
            catch (MultipleDeletionJobException jobEx)
            {
                _logger.LogWarning(jobEx, "A deletion job definition is already exist. A project should only contain one deletion job definition.");
                return BadRequest(jobEx.Message);
            }
        }

        /// <summary>
        /// Get a job definition by id
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobId">Id of the job definition</param>
        /// <returns>Job definition object</returns>
        [HttpGet("Project/{projectId}/job/{jobId}", Name = "GetJobDefinitionById")]
        public async Task<IActionResult> GetJobDefinition(int projectId, int jobId)
        {
            _logger.LogInformation("Getting job definition {jobId} in project {projectId}", jobId, projectId);

            var jobDefinition = await _jobDefinitionService.GetJobDefinitionById(jobId);
            var result = _mapper.Map<JobDefinitionDto>(jobDefinition);
            return Ok(result);
        }

        /// <summary>
        /// Get a job definition by name
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobName">Name of the job definition</param>
        /// <returns>Job definition object</returns>
        [HttpGet("Project/{projectId}/job/name/{jobName}")]
        public async Task<IActionResult> GetJobDefinition(int projectId, string jobName)
        {
            _logger.LogInformation("Getting job definition {jobName} in project {projectId}", jobName, projectId);

            var jobDefinition = await _jobDefinitionService.GetJobDefinitionByName(projectId, jobName);
            var result = _mapper.Map<JobDefinitionDto>(jobDefinition);
            return Ok(result);
        }

        /// <summary>
        /// Update a job definition
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobId">Id of the job definition</param>
        /// <param name="jobDefinition">Update job definition request body</param>
        /// <returns></returns>
        [HttpPut("Project/{projectId}/job/{jobId}")]
        public async Task<IActionResult> UpdateJobDefinition(int projectId, int jobId, UpdateJobDefinitionDto jobDefinition)
        {
            _logger.LogInformation("Updating job definition {jobId} in project {projectId}. Request body: {@jobDefinition}", jobId, projectId, jobDefinition);

            try
            {
                if (jobId != jobDefinition.Id)
                {
                    _logger.LogWarning("Job Id doesn't match.");
                    return BadRequest("Job Id doesn't match.");
                }                    

                await _jobDefinitionService.RenameJobDefinition(jobId, jobDefinition.Name);

                return Ok();
            }
            catch (DuplicateJobDefinitionException ex)
            {
                _logger.LogWarning(ex, "Duplicate job definition name");
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("Project/{projectId}/job/{jobId}/task/order")]
        public async Task<IActionResult> UpdateJobTaskOrder(int projectId, int jobId, UpdateTaskOrderDto dto)
        {
            _logger.LogInformation("Updating tasks order for job {jobId} in project {projectId}. Request body: {@dto}", jobId, projectId, dto);

            var jobs = await _jobDefinitionService.GetJobTaskDefinitions(jobId);

            foreach (var jobOrder in dto.TaskOrders)
            {
                var job = jobs.FirstOrDefault(j => j.Id == jobOrder.Key);
                job.Sequence = jobOrder.Value;
                await _jobDefinitionService.UpdateJobTaskDefinition(job);
            }

            return Ok();
        }

        /// <summary>
        /// Delete a job definition
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobId">Id of the job definition</param>
        /// <returns></returns>
        [HttpDelete("Project/{projectId}/job/{jobId}")]
        public async Task<IActionResult> DeleteJobDefinition(int projectId, int jobId)
        {
            _logger.LogInformation("Deleting job definition {jobId} in project {projectId}", jobId, projectId);

            await _jobDefinitionService.DeleteJobDefinition(jobId);

            return NoContent();
        }

        /// <summary>
        /// Delete job definition in batch
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobIds">Ids of the job definitions to be deleted</param>
        /// <returns></returns>
        [HttpDelete("Project/{projectId}/job/bulkdelete")]
        [Authorize(Policy = AuthorizePolicy.ProjectContributorAccess)]
        public async Task<IActionResult> DeleteJobDefinitions(int projectId, [FromQuery(Name = "jobIds")]int[] jobIds)
        {
            _logger.LogInformation("Deleting job definitions {jobIds} in project {projectId}", jobIds, projectId);

            await _jobDefinitionService.DeleteJobDefinitions(projectId, jobIds);

            return NoContent();
        }

        /// <summary>
        /// Create a new job task definition for a job definition
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobId">Id of the job definition</param>
        /// <param name="newTask">The create job task definition request body</param>
        /// <returns></returns>
        [HttpPost("Project/{projectId}/job/{jobId}/task", Name = "CreateJobTaskDefinition")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateJobTaskDefinition(int projectId, int jobId, CreateJobTaskDefinitionDto newTask)
        {
            // exclude additional configs since it may contain secret values
            var requestBodyToLog = new CreateJobTaskDefinitionDto
            {
                Name = newTask.Name,
                Configs = newTask.Configs,
                Provider = newTask.Provider,
                Sequence = newTask.Sequence,
                Type = newTask.Type
            };

            _logger.LogInformation("Creating job task definition in job {jobId} in project {projectId}. Request body: {@requestBodyToLog}", 
                jobId, projectId, requestBodyToLog);

            try
            {
                var entity = _mapper.Map<JobTaskDefinition>(newTask);
                entity.JobDefinitionId = jobId;
                entity.Id = await _jobDefinitionService.AddJobTaskDefinition(entity);
                
                var newJobTaskDefinitionResponse = _mapper.Map<JobTaskDefinitionDto>(entity);

                return CreatedAtRoute("GetJobTaskDefinitionById", new
                {
                    projectId,
                    jobId,
                    taskId = newJobTaskDefinitionResponse.Id
                }, newJobTaskDefinitionResponse);
            }
            catch (DuplicateJobTaskDefinitionException dupTaskEx)
            {
                _logger.LogWarning(dupTaskEx, "Duplicate task name");
                return BadRequest(dupTaskEx.Message);
            }
            catch (InvalidPluginTypeException pluginTypeEx)
            {
                _logger.LogWarning(pluginTypeEx, "Invalid provider's plugin type");
                return BadRequest(pluginTypeEx.Message);
            }
            catch (JobDefinitionNotFoundException modEx)
            {
                _logger.LogWarning(modEx, "Job definition not found");
                return BadRequest(modEx.Message);
            }
            catch (ProviderNotInstalledException provEx)
            {
                _logger.LogWarning(provEx, "Provider not installed");
                return BadRequest(provEx.Message);
            }
            catch (ExternalServiceRequiredException esrEx)
            {
                _logger.LogWarning(esrEx, "External service required");
                return BadRequest(esrEx.Message);
            }
            catch (ExternalServiceNotFoundException esnfEx)
            {
                _logger.LogWarning(esnfEx, "External service not found");
                return BadRequest(esnfEx.Message);
            }
            catch (IncorrectExternalServiceTypeException iestEx)
            {
                _logger.LogWarning(iestEx, "Incorrect external service type");
                return BadRequest(iestEx.Message);
            }
            catch (JobTaskDefinitionTypeException taskEx)
            {
                _logger.LogWarning(taskEx, "Incorrect task definition type");
                return BadRequest(taskEx.Message);
            }
        }

        /// <summary>
        /// Create job task definitions in bulk for a job definition
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobId">Id of the job definition</param>
        /// <param name="newTasks">Create job task definitions request body</param>
        /// <returns></returns>
        [HttpPost("Project/{projectId}/job/{jobId}/tasks", Name = "CreateJobTaskDefinitions")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateJobTaskDefinitions(int projectId, int jobId, List<CreateJobTaskDefinitionDto> newTasks)
        {
            // exclude additional configs since it may contain secret values
            var requestBodyToLog = newTasks.Select(t => new CreateJobTaskDefinitionDto
            {
                Name = t.Name,
                Configs = t.Configs,
                Provider = t.Provider,
                Sequence = t.Sequence,
                Type = t.Type
            }).ToList();

            _logger.LogInformation("Creating job task definitions in job {jobId} in project {projectId}. Request body: {@requestBodyToLog}", 
                jobId, projectId, requestBodyToLog);

            try
            {
                var entities = _mapper.Map<List<JobTaskDefinition>>(newTasks);
                await _jobDefinitionService.AddJobTaskDefinitions(jobId, entities);

                var newTasksResponse = _mapper.Map<List<JobTaskDefinitionDto>>(entities);

                return CreatedAtRoute("GetJobTaskDefinitions", new
                {
                    projectId,
                    jobId,
                }, newTasksResponse);
            }
            catch (DuplicateJobTaskDefinitionException dupTaskEx)
            {
                _logger.LogWarning(dupTaskEx, "Duplicate task name");
                return BadRequest(dupTaskEx.Message);
            }
            catch (InvalidPluginTypeException pluginTypeEx)
            {
                _logger.LogWarning(pluginTypeEx, "Invalid provider's plugin type");
                return BadRequest(pluginTypeEx.Message);
            }
            catch (JobDefinitionNotFoundException modEx)
            {
                _logger.LogWarning(modEx, "Job definition not found");
                return BadRequest(modEx.Message);
            }
            catch (ProviderNotInstalledException provEx)
            {
                _logger.LogWarning(provEx, "Provider not installed");
                return BadRequest(provEx.Message);
            }
            catch (ExternalServiceRequiredException esrEx)
            {
                _logger.LogWarning(esrEx, "External service required");
                return BadRequest(esrEx.Message);
            }
            catch (ExternalServiceNotFoundException esnfEx)
            {
                _logger.LogWarning(esnfEx, "External service not found");
                return BadRequest(esnfEx.Message);
            }
            catch (IncorrectExternalServiceTypeException iestEx)
            {
                _logger.LogWarning(iestEx, "Incorrect external service type");
                return BadRequest(iestEx.Message);
            }
        }

        /// <summary>
        /// Get job task definitions for a job definition
        /// </summary>
        /// <param name="jobId">Id of the job definition</param>
        /// <returns>List of job task definitions</returns>
        [HttpGet("Project/{projectId}/job/{jobId}/task", Name = "GetJobTaskDefinitions")]
        public async Task<IActionResult> GetJobTaskDefinitions(int jobId)
        {
            _logger.LogInformation("Getting job task definitions in job {jobId}", jobId);

            var jobTaskDefinitions = await _jobDefinitionService.GetJobTaskDefinitions(jobId);
            var results = _mapper.Map<List<JobTaskDefinitionDto>>(jobTaskDefinitions);

            return Ok(results);
        }

        /// <summary>
        /// Get a job task definition
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobId">Id of the job definition</param>
        /// <param name="taskId">Id of the job task definition</param>
        /// <returns>Job task definition object</returns>
        [HttpGet("Project/{projectId}/job/{jobId}/task/{taskId}", Name = "GetJobTaskDefinitionById")]
        public async Task<IActionResult> GetJobTaskDefinition(int projectId, int jobId, int taskId)
        {
            _logger.LogInformation("Getting job task definition {taskId} in job definition {jobId}, project {projectId}", taskId, jobId, projectId);

            var jobTaskDefinition = await _jobDefinitionService.GetJobTaskDefinitionById(taskId);
            var result = _mapper.Map<JobTaskDefinitionDto>(jobTaskDefinition);
            return Ok(result);
        }

        /// <summary>
        /// Get a job task definition by name
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobId">Id of the job definition</param>
        /// <param name="taskName">Name of the job task definition</param>
        /// <returns>Job task definition object</returns>
        [HttpGet("Project/{projectId}/job/{jobId}/task/name/{taskName}")]
        public async Task<IActionResult> GetJobTaskDefinition(int projectId, int jobId, string taskName)
        {
            _logger.LogInformation("Getting job task definition {taskName} in job definition {jobId}, project {projectId}", taskName, jobId, projectId);

            var jobTaskDefinition = await _jobDefinitionService.GetJobTaskDefinitionByName(jobId, taskName);
            var result = _mapper.Map<JobTaskDefinitionDto>(jobTaskDefinition);
            return Ok(result);
        }

        /// <summary>
        /// Update a job task definition
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobId">Id of the job definition</param>
        /// <param name="taskId">Id of the job task definition</param>
        /// <param name="jobTaskDefinition">Update job task definition request body</param>
        /// <returns></returns>
        [HttpPut("Project/{projectId}/job/{jobId}/task/{taskId}")]
        public async Task<IActionResult> UpdateJobTaskDefinition(int projectId, int jobId, int taskId, UpdateJobTaskDefinitionDto jobTaskDefinition)
        {
            // exclude additional configs since it may contain secret values
            var requestBodyToLog = new UpdateJobTaskDefinitionDto
            {
                Id = jobTaskDefinition.Id,
                Name = jobTaskDefinition.Name,
                Configs = jobTaskDefinition.Configs,
                Provider = jobTaskDefinition.Provider,
                Sequence = jobTaskDefinition.Sequence,
                Type = jobTaskDefinition.Type
            };

            _logger.LogInformation("Updating job task definition {taskId} in job definition {jobId}, project {projectId}. Request body: {@requestBodyToLog}", 
                taskId, jobId, projectId, requestBodyToLog);

            try
            {
                if (taskId != jobTaskDefinition.Id)
                {
                    _logger.LogWarning("Task Id doesn't match.");
                    return BadRequest("Task Id doesn't match.");
                }

                var entity = _mapper.Map<JobTaskDefinition>(jobTaskDefinition);
                entity.JobDefinitionId = jobId;
                await _jobDefinitionService.UpdateJobTaskDefinition(entity);

                return Ok();
            }
            catch (DuplicateJobTaskDefinitionException dupTaskEx)
            {
                _logger.LogWarning(dupTaskEx, "Duplicate task name");
                return BadRequest(dupTaskEx.Message);
            }
            catch (InvalidPluginTypeException pluginTypeEx)
            {
                _logger.LogWarning(pluginTypeEx, "Invalid provider's plugin type");
                return BadRequest(pluginTypeEx.Message);
            }
            catch (ProviderNotInstalledException provEx)
            {
                _logger.LogWarning(provEx, "Provider not installed");
                return BadRequest(provEx.Message);
            }
            catch (ExternalServiceRequiredException esrEx)
            {
                _logger.LogWarning(esrEx, "External service required");
                return BadRequest(esrEx.Message);
            }
            catch (ExternalServiceNotFoundException esnfEx)
            {
                _logger.LogWarning(esnfEx, "External service not found");
                return BadRequest(esnfEx.Message);
            }
            catch (IncorrectExternalServiceTypeException iestEx)
            {
                _logger.LogWarning(iestEx, "Incorrect external service type");
                return BadRequest(iestEx.Message);
            }
        }

        /// <summary>
        /// Update job task configuration
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobId">Id of the job definition</param>
        /// <param name="taskId">Id of the job task definition</param>
        /// <param name="jobTaskConfig">Updated job task configuration</param>
        /// <returns></returns>
        [HttpPut("Project/{projectId}/job/{jobId}/task/{taskId}/config")]
        public async Task<IActionResult> UpdateJobTaskConfig(int projectId, int jobId, int taskId, UpdateJobTaskConfigDto jobTaskConfig)
        {
            _logger.LogInformation("Updating job task config for task {taskId} in job definition {jobId}, project {projectId}. Request body: {@jobTaskConfig}", 
                taskId, jobId, projectId, jobTaskConfig);

            if (taskId != jobTaskConfig.Id)
            {
                _logger.LogWarning("Task Id doesn't match.");
                return BadRequest("Task Id doesn't match.");
            }                
            
            await _jobDefinitionService.UpdateJobTaskConfig(jobTaskConfig.Id, jobTaskConfig.Config);

            return Ok();
        }

        /// <summary>
        /// Delete a job task definition
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="jobId">Id of the job definition</param>
        /// <param name="taskId">Id of the job task definition</param>
        /// <returns></returns>
        [HttpDelete("Project/{projectId}/job/{jobId}/task/{taskId}")]
        public async Task<IActionResult> DeleteJobTaskDefinition(int projectId, int jobId, int taskId)
        {
            _logger.LogInformation("Deleting job task definition {taskId} in job definition {jobId}, project {projectId}", taskId, jobId, projectId);

            await _jobDefinitionService.DeleteJobTaskDefinition(taskId);

            return NoContent();
        }
    }
}
