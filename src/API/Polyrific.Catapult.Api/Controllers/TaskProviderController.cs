// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Identity;
using Polyrific.Catapult.Shared.Dto.Provider;

namespace Polyrific.Catapult.Api.Controllers
{
    [Route("task-provider")]
    [ApiController]
    public class TaskProviderController : ControllerBase
    {
        private readonly ITaskProviderService _taskProviderService;
        private readonly ITaskProviderAdditionalConfigService _taskProviderAdditionalConfigService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public TaskProviderController(ITaskProviderService taskProviderService, ITaskProviderAdditionalConfigService taskProviderAdditionalConfigService, 
            IMapper mapper, ILogger<TaskProviderController> logger)
        {
            _taskProviderService = taskProviderService;
            _taskProviderAdditionalConfigService = taskProviderAdditionalConfigService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all task providers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = AuthorizePolicy.UserRoleBasicAccess)]
        public async Task<IActionResult> GetTaskProviders()
        {
            _logger.LogRequest("Getting task providers");

            var taskProviders = await _taskProviderService.GetTaskProviders();

            var results = _mapper.Map<List<TaskProviderDto>>(taskProviders);

            _logger.LogResponse("Task providers retrieved. Response body: {@results}", results);

            return Ok(results);
        }

        /// <summary>
        /// Get task providers by type
        /// </summary>
        /// <param name="taskProviderType">Type of the taskProvider</param>
        /// <returns></returns>
        [HttpGet("type/{taskProviderType}")]
        [Authorize(Policy = AuthorizePolicy.UserRoleBasicAccess)]
        public async Task<IActionResult> GetTaskProvidersByType(string taskProviderType)
        {
            _logger.LogRequest("Getting task providers for type {taskProviderType}", taskProviderType);

            var taskProviders = await _taskProviderService.GetTaskProviders(taskProviderType);

            var results = _mapper.Map<List<TaskProviderDto>>(taskProviders);

            _logger.LogResponse("Task providers for type {taskProviderType} retrieved. Response body: {@results}", taskProviderType, results);

            return Ok(results);
        }

        /// <summary>
        /// Get taskProvider by id
        /// </summary>
        /// <param name="taskProviderId">Id of the taskProvider</param>
        /// <returns></returns>
        [HttpGet("{taskProviderId}", Name = "GetProviderById")]
        [Authorize(Policy = AuthorizePolicy.UserRoleBasicAccess)]
        public async Task<IActionResult> GetTaskProviderById(int taskProviderId)
        {
            _logger.LogRequest("Getting task provider {taskProviderId}", taskProviderId);

            var taskProvider = await _taskProviderService.GetTaskProviderById(taskProviderId);
            if (taskProvider == null)
                return NoContent();

            var additionalConfigs = await _taskProviderAdditionalConfigService.GetByTaskProvider(taskProviderId);

            var result = _mapper.Map<TaskProviderDto>(taskProvider);
            result.AdditionalConfigs = _mapper.Map<TaskProviderAdditionalConfigDto[]>(additionalConfigs);


            _logger.LogResponse("Task provider {taskProviderId} retrieved. Response body: {@result}", taskProviderId, result);

            return Ok(result);
        }

        /// <summary>
        /// Get taskProvider by name
        /// </summary>
        /// <param name="taskProviderName">Name of the taskProvider</param>
        /// <returns></returns>
        [HttpGet("name/{taskProviderName}", Name = "GetProviderByName")]
        [Authorize(Policy = AuthorizePolicy.UserRoleBasicAccess)]
        public async Task<IActionResult> GetTaskProviderByName(string taskProviderName)
        {
            _logger.LogRequest("Getting task provider {taskProviderName}", taskProviderName);

            var taskProvider = await _taskProviderService.GetTaskProviderByName(taskProviderName);
            if (taskProvider == null)
                return NoContent();

            var additionalConfigs = await _taskProviderAdditionalConfigService.GetByTaskProvider(taskProvider.Id);

            var result = _mapper.Map<TaskProviderDto>(taskProvider);
            result.AdditionalConfigs = _mapper.Map<TaskProviderAdditionalConfigDto[]>(additionalConfigs);

            _logger.LogResponse("Task provider {taskProviderName} retrieved. Response body: {@result}", taskProviderName, result);

            return Ok(result);

        }

        /// <summary>
        /// Register a taskProvider
        /// </summary>
        /// <param name="dto"><see cref="NewTaskProviderDto"/> object</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
        public async Task<IActionResult> RegisterTaskProvider(NewTaskProviderDto dto)
        {
            _logger.LogRequest("Registering task provider. Request body: {@dto}", dto);

            try
            {
                var taskProvider = await _taskProviderService.AddTaskProvider(dto.Name, dto.Type, dto.Author, dto.Version, dto.RequiredServices, dto.DisplayName, dto.Description,
                    dto.ThumbnailUrl, dto.Tags, dto.Created, dto.Updated);
                var result = _mapper.Map<TaskProviderDto>(taskProvider);

                if (dto.AdditionalConfigs != null && dto.AdditionalConfigs.Length > 0)
                {
                    var additionalConfigs = _mapper.Map<List<TaskProviderAdditionalConfig>>(dto.AdditionalConfigs);
                    var _ = await _taskProviderAdditionalConfigService.AddAdditionalConfigs(taskProvider.Id, additionalConfigs);
                }

                _logger.LogResponse("Task provider created. Response body: {@result}", result);

                return CreatedAtRoute("GetProviderById", new { taskProviderId = taskProvider.Id }, result);
            }
            catch (RequiredServicesNotSupportedException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete a taskProvider
        /// </summary>
        /// <param name="taskProviderId">Id of the taskProvider</param>
        /// <returns></returns>
        [HttpDelete("{taskProviderId}")]
        [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
        public async Task<IActionResult> DeleteTaskProviderById(int taskProviderId)
        {
            _logger.LogRequest("Deleting task provider {taskProviderId}", taskProviderId);

            await _taskProviderService.DeleteTaskProvider(taskProviderId);

            _logger.LogResponse("Task provider {taskProviderId} deleted", taskProviderId);

            return NoContent();
        }

        /// <summary>
        /// Get list of additional configs of a taskProvider
        /// </summary>
        /// <param name="taskProviderName">Name of the taskProvider</param>
        /// <returns></returns>
        [HttpGet("name/{taskProviderName}/config")]
        [Authorize(Policy = AuthorizePolicy.UserRoleBasicEngineAccess)]
        public async Task<IActionResult> GetTaskProviderAdditionalConfigsByTaskProviderName(string taskProviderName)
        {
            _logger.LogRequest("Getting additional configs for task provider {taskProviderName}", taskProviderName);

            var additionalConfigs = await _taskProviderAdditionalConfigService.GetByTaskProviderName(taskProviderName);

            var results = _mapper.Map<List<TaskProviderAdditionalConfigDto>>(additionalConfigs);

            _logger.LogResponse("Additional configs for task provider {taskProviderName} retrieved. Response body: {@result}", taskProviderName, results);

            return Ok(results);
        }

    }
}
