﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Specifications;
using Polyrific.Catapult.Shared.Common.Interface;
using Polyrific.Catapult.Shared.Common.Notification;
using Polyrific.Catapult.Shared.Dto.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Core.Services
{
    public class JobQueueService : IJobQueueService
    {
        private readonly IJobQueueRepository _jobQueueRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJobCounterService _jobCounterService;
        private readonly ITextWriter _textWriter;
        private readonly INotificationProvider _notificationProvider;
        private readonly IConfiguration _configuration;

        private readonly string[] _inProgressJobStatus = { JobStatus.Queued, JobStatus.Processing, JobStatus.Pending };
        private readonly string[] _pastJobStatus = { JobStatus.Completed, JobStatus.Error, JobStatus.Cancelled };

        public JobQueueService(IJobQueueRepository jobQueueRepository, IProjectRepository projectRepository, IUserRepository userRepository,
            IJobCounterService jobCounterService, ITextWriter textWriter, INotificationProvider notificationProvider, IConfiguration configuration)
        {
            _jobQueueRepository = jobQueueRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _jobCounterService = jobCounterService;
            _textWriter = textWriter;
            _notificationProvider = notificationProvider;
            _configuration = configuration;
        }

        public async Task<int> AddJobQueue(int projectId, string originUrl, string jobType, int? jobDefinitionId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var project = await _projectRepository.GetById(projectId, cancellationToken);
            bool projectAllowAddingQueue = project?.Status == ProjectStatusFilterType.Active || 
                (project?.Status == ProjectStatusFilterType.Deleting && jobType == JobType.Delete);
            if (project == null || !projectAllowAddingQueue)
            {
                throw new ProjectNotFoundException(projectId);
            }

            var inProgressJobSpec = new JobQueueFilterSpecification(projectId, _inProgressJobStatus);
            var inProgressJob = await _jobQueueRepository.GetSingleBySpec(inProgressJobSpec, cancellationToken);
            if (inProgressJob != null)
            {
                if (project.Status == ProjectStatusFilterType.Deleting && jobType != JobType.Delete)
                {
                    return inProgressJob.Id;
                }

                throw new JobQueueInProgressException(projectId);
            }

            // detemine the jobQueue type if it is not provided by the caller
            if (string.IsNullOrWhiteSpace(jobType))
            {
                jobType = JobType.Create;

                var hasBeenCreatedBefore = (await _jobQueueRepository.CountBySpec(new JobQueueFilterSpecification(projectId, new string[] { JobStatus.Completed }))) > 0;

                if (hasBeenCreatedBefore)
                {
                    jobType = JobType.Update;
                }
            }

            var newJobQueue = new JobQueue
            {
                ProjectId = projectId,
                Code = await GetNextJobCode(),
                OriginUrl = originUrl,
                JobType = jobType,
                JobDefinitionId = jobDefinitionId,
                Status = JobStatus.Queued
            };
            return await _jobQueueRepository.Create(newJobQueue, cancellationToken);
        }

        public async Task UpdateJobQueue(JobQueue updatedJobQueue, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var jobQueueSpec = new JobQueueFilterSpecification(updatedJobQueue.ProjectId, updatedJobQueue.Id);
            jobQueueSpec.Includes.Add(j => j.Project);
            var jobQueue = await _jobQueueRepository.GetSingleBySpec(jobQueueSpec, cancellationToken);

            if (jobQueue != null && jobQueue.Project.Status != ProjectStatusFilterType.Archived)
            {
                if (updatedJobQueue.ProjectId == 0 && !string.IsNullOrEmpty(jobQueue.CatapultEngineId) && jobQueue.CatapultEngineId != updatedJobQueue.CatapultEngineId)
                {
                    throw new JobProcessedByOtherEngineException(jobQueue.Id);
                }

                jobQueue.CatapultEngineId = updatedJobQueue.CatapultEngineId;
                jobQueue.CatapultEngineIPAddress = updatedJobQueue.CatapultEngineIPAddress;
                jobQueue.CatapultEngineMachineName = updatedJobQueue.CatapultEngineMachineName;
                jobQueue.Status = updatedJobQueue.Status;
                jobQueue.JobTasksStatus = updatedJobQueue.JobTasksStatus;
                jobQueue.JobType = updatedJobQueue.JobType;
                jobQueue.OutputValues = updatedJobQueue.OutputValues;
                jobQueue.Remarks = updatedJobQueue.Remarks;
                await _jobQueueRepository.Update(jobQueue, cancellationToken);
            }
        }

        public async Task CancelJobQueue(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var jobQueueSpec = new JobQueueFilterSpecification(0, id);
            jobQueueSpec.Includes.Add(q => q.Project);
            var jobQueue = await _jobQueueRepository.GetSingleBySpec(jobQueueSpec, cancellationToken);

            if (jobQueue != null)
            {
                if (!_inProgressJobStatus.Contains(jobQueue.Status))
                {
                    throw new CancelCompletedJobException(id);
                }

                jobQueue.Status = JobStatus.Cancelled;
                await _jobQueueRepository.Update(jobQueue, cancellationToken);
            }
        }

        public async Task<List<JobQueue>> GetJobQueues(int projectId, string filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            JobQueueFilterSpecification jobQueueSpec;
            switch (filter?.ToLower())
            {
                case JobQueueFilterType.All:
                    jobQueueSpec = new JobQueueFilterSpecification(projectId, null);
                    break;
                case JobQueueFilterType.Current:
                    jobQueueSpec = new JobQueueFilterSpecification(projectId, _inProgressJobStatus);
                    break;
                case JobQueueFilterType.Past:
                    jobQueueSpec = new JobQueueFilterSpecification(projectId, _pastJobStatus);
                    break;
                case JobQueueFilterType.Succeeded:
                    jobQueueSpec = new JobQueueFilterSpecification(projectId, new string[] { JobStatus.Completed });
                    break;
                case JobQueueFilterType.Failed:
                    jobQueueSpec = new JobQueueFilterSpecification(projectId, new string[] { JobStatus.Error });
                    break;
                default:
                    throw new FilterTypeNotFoundException(filter);
            }

            jobQueueSpec.Includes.Add(q => q.JobDefinition);

            var jobQueues = await _jobQueueRepository.GetBySpec(jobQueueSpec, cancellationToken);

            return jobQueues.ToList();
        }

        public async Task<List<JobQueue>> GetJobQueuesByStatus(string status, CancellationToken cancellationToken = default(CancellationToken))
        {
            var jobQueueSpec = new JobQueueFilterSpecification(0, null, status);
            var jobQueues = await _jobQueueRepository.GetBySpec(jobQueueSpec, cancellationToken);
            return jobQueues.ToList();
        }

        public async Task<JobQueue> GetJobQueueById(int projectId, int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var jobQueueSpec = new JobQueueFilterSpecification(projectId, id);
            jobQueueSpec.Includes.Add(q => q.JobDefinition);
            var jobQueue = await _jobQueueRepository.GetSingleBySpec(jobQueueSpec, cancellationToken);

            return jobQueue;
        }

        public async Task<JobQueue> GetJobQueueByCode(int projectId, string jobQueueCode, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var jobQueueSpec = new JobQueueFilterSpecification(projectId, jobQueueCode, null);
            jobQueueSpec.Includes.Add(q => q.JobDefinition);
            var jobQueue = await _jobQueueRepository.GetSingleBySpec(jobQueueSpec, cancellationToken);

            return jobQueue;
        }

        public async Task<List<JobTaskStatus>> GetJobTaskStatus(int id, string filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var jobQueue = await _jobQueueRepository.GetById(id, cancellationToken);

            if (!string.IsNullOrEmpty(jobQueue?.JobTasksStatus))
            {
                var tasks = JsonConvert.DeserializeObject<List<JobTaskStatus>>(jobQueue.JobTasksStatus);
                switch (filter?.ToLower())
                {
                    case JobTaskStatusFilterType.All:
                        return tasks;
                    case JobTaskStatusFilterType.Latest:
                        var latestTask = tasks.OrderByDescending(t => t.Sequence)
                                             .FirstOrDefault(t => t.Status != JobTaskStatusType.NotExecuted) ?? tasks.OrderByDescending(t => t.Sequence)
                                             .FirstOrDefault();

                        return new List<JobTaskStatus>
                        {
                            latestTask
                        };

                    default:
                        throw new FilterTypeNotFoundException(filter);
                }
            }

            return new List<JobTaskStatus>();
        }

        public async Task<JobQueue> GetFirstUnassignedQueuedJob(string engine, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var queuedJobSpec = new JobQueueFilterSpecification();
            queuedJobSpec.Includes.Add(q => q.JobDefinition);
            queuedJobSpec.Includes.Add(q => q.Project);
            var jobQueue = await _jobQueueRepository.GetSingleBySpec(queuedJobSpec, cancellationToken);

            if (!string.IsNullOrEmpty(engine))
            {
                if (jobQueue == null)
                {
                    var pendingJobSpec = new JobQueueFilterSpecification(0, null, JobStatus.Queued, engine);
                    pendingJobSpec.Includes.Add(q => q.JobDefinition);
                    queuedJobSpec.Includes.Add(q => q.Project);
                    jobQueue = await _jobQueueRepository.GetSingleBySpec(pendingJobSpec, cancellationToken);
                }

                try
                {
                    jobQueue.CatapultEngineId = engine;
                    await _jobQueueRepository.Update(jobQueue, cancellationToken);
                }
                catch (Exception)
                {
                    jobQueue = null;
                }
            }

            return jobQueue;
        }

        private async Task<string> GetNextJobCode()
        {
            var sequence = await _jobCounterService.GetNextSequence();
            return $"{DateTime.UtcNow:yyyyMMdd}.{sequence}";
        }

        public async Task<string> GetJobLogs(int projectId, int jobQueueId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var jobQueue = await GetJobQueueById(projectId, jobQueueId);
            if (jobQueue != null)
                return await _textWriter.Read($"{JobQueueLog.FolderNamePrefix}_{projectId}_{jobQueueId}", null);
            
            return "";
        }

        public async Task<string> GetTaskLogs(int projectId, int jobQueueId, string taskName, CancellationToken cancellationToken = default(CancellationToken))
        {
            var jobQueue = await GetJobQueueById(projectId, jobQueueId);
            if (jobQueue != null)
                return await _textWriter.Read($"{JobQueueLog.FolderNamePrefix}_{projectId}_{jobQueueId}", taskName);

            return "";
        }

        public async Task SendNotification(int jobQueueId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var jobQueueSpec = new JobQueueFilterSpecification(0, jobQueueId);
            jobQueueSpec.Includes.Add(q => q.Project.Members);
            jobQueueSpec.Includes.Add(q => q.JobDefinition);
            var jobQueue = await _jobQueueRepository.GetSingleBySpec(jobQueueSpec, cancellationToken);

            var users = await _userRepository.GetUsersByIds(jobQueue.Project.Members.Select(m => m.UserId).ToArray());

            var jobQueueWebUrl = $"{_configuration[ConfigurationKey.WebUrl]}/project/{jobQueue.ProjectId}/job-queue/{jobQueueId}";
            await _notificationProvider.SendNotification(new SendNotificationRequest
            {
                MessageType = NotificationConfig.JobQueueCompleted,
                Emails = users.Select(u => u.Email).Distinct().ToList()
            }, new Dictionary<string, string>
                    {
                        {MessageParameter.JobCode, jobQueue.Code},
                        {MessageParameter.JobDefinitionName, jobQueue.JobDefinition.Name},
                        {MessageParameter.ProjectName, jobQueue.Project.Name},
                        {MessageParameter.JobStatus, jobQueue.Status},
                        {MessageParameter.Remarks, jobQueue.Remarks},
                        {MessageParameter.WebUrl, jobQueueWebUrl},
                        {MessageParameter.JobTaskStatus, GenerateJobTaskStatusNotificationMessage(jobQueue.JobTasksStatus)}
                    });
        }

        private string GenerateJobTaskStatusNotificationMessage(string jobTaskStatus)
        {
            if (string.IsNullOrEmpty(jobTaskStatus))
                return string.Empty;

            var jobTaskStatusList = JsonConvert.DeserializeObject<List<JobTaskStatus>>(jobTaskStatus);

            var sb = new StringBuilder("<ul>");
            foreach (var taskStatus in jobTaskStatusList.OrderBy(j => j.Sequence))
            {
                sb.AppendLine("<li>");
                sb.AppendLine($"<div>{taskStatus.Sequence}. {taskStatus.TaskName}: {taskStatus.Status}</div>");
                sb.AppendLine($"<div>{taskStatus.Remarks}</div>");
                sb.AppendLine("</li>");
            }

            sb.AppendLine("</ul>");

            return sb.ToString();
        }
    }
}
