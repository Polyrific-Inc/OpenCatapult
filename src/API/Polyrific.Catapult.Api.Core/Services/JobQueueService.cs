// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Newtonsoft.Json;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Specifications;
using Polyrific.Catapult.Shared.Common.Interface;
using Polyrific.Catapult.Shared.Dto.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Core.Services
{
    public class JobQueueService : IJobQueueService
    {
        private readonly IJobQueueRepository _jobQueueRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IJobCounterService _jobCounterService;
        private readonly ITextWriter _textWriter;

        private readonly string[] _inProgressJobStatus = { JobStatus.Queued, JobStatus.Processing, JobStatus.Pending };
        private readonly string[] _pastJobStatus = { JobStatus.Completed, JobStatus.Error, JobStatus.Cancelled };

        public JobQueueService(IJobQueueRepository jobQueueRepository, IProjectRepository projectRepository, IJobCounterService jobCounterService, ITextWriter textWriter)
        {
            _jobQueueRepository = jobQueueRepository;
            _projectRepository = projectRepository;
            _jobCounterService = jobCounterService;
            _textWriter = textWriter;
        }

        public async Task<int> AddJobQueue(int projectId, string originUrl, string jobType, int? jobDefinitionId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var project = await _projectRepository.GetById(projectId, cancellationToken);
            if (project == null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            var inProgressJobSpec = new JobQueueFilterSpecification(projectId, _inProgressJobStatus);
            if (await _jobQueueRepository.CountBySpec(inProgressJobSpec, cancellationToken) > 0)
            {
                throw new JobQueueInProgressException(projectId);
            }

            // detemine the job type if it is not provided by the caller
            if (string.IsNullOrWhiteSpace(jobType))
            {
                jobType = JobType.Create;

                var hasBeenCreatedBefore = (await _jobQueueRepository.CountBySpec(new JobQueueFilterSpecification(projectId, JobStatus.Completed))) > 0;

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

        public async Task UpdateJobQueue(JobQueue updatedJob, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var job = await _jobQueueRepository.GetById(updatedJob.Id, cancellationToken);

            if (job != null)
            {
                if (!string.IsNullOrEmpty(job.CatapultEngineId) && job.CatapultEngineId != updatedJob.CatapultEngineId)
                {
                    throw new JobProcessedByOtherEngineException(job.Id);
                }

                job.CatapultEngineId = updatedJob.CatapultEngineId;
                job.CatapultEngineIPAddress = updatedJob.CatapultEngineIPAddress;
                job.CatapultEngineMachineName = updatedJob.CatapultEngineMachineName;
                job.Status = updatedJob.Status;
                job.JobTasksStatus = updatedJob.JobTasksStatus;
                job.JobType = updatedJob.JobType;
                job.OutputValues = updatedJob.OutputValues;
                await _jobQueueRepository.Update(job, cancellationToken);
            }
        }

        public async Task CancelJobQueue(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var job = await _jobQueueRepository.GetById(id, cancellationToken);

            if (job != null)
            {
                if (!_inProgressJobStatus.Contains(job.Status))
                {
                    throw new CancelCompletedJobException(id);
                }

                job.Status = JobStatus.Cancelled;
                await _jobQueueRepository.Update(job, cancellationToken);
            }
        }

        public async Task<List<JobQueue>> GetJobQueues(int projectId, string filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            JobQueueFilterSpecification queueSpec;
            switch (filter?.ToLower())
            {
                case JobQueueFilterType.All:
                    queueSpec = new JobQueueFilterSpecification(projectId);
                    break;
                case JobQueueFilterType.Current:
                    queueSpec = new JobQueueFilterSpecification(projectId, _inProgressJobStatus);
                    break;
                case JobQueueFilterType.Past:
                    queueSpec = new JobQueueFilterSpecification(projectId, _pastJobStatus);
                    break;
                case JobQueueFilterType.Succeeded:
                    queueSpec = new JobQueueFilterSpecification(projectId, JobStatus.Completed);
                    break;
                case JobQueueFilterType.Failed:
                    queueSpec = new JobQueueFilterSpecification(projectId, JobStatus.Error);
                    break;
                default:
                    throw new FilterTypeNotFoundException(filter);
            }

            var jobQueues = await _jobQueueRepository.GetBySpec(queueSpec, cancellationToken);

            return jobQueues.ToList();
        }

        public async Task<List<JobQueue>> GetJobQueuesByStatus(string status, CancellationToken cancellationToken = default(CancellationToken))
        {
            var queueSpec = new JobQueueFilterSpecification(status);
            var jobQueues = await _jobQueueRepository.GetBySpec(queueSpec, cancellationToken);
            return jobQueues.ToList();
        }

        public async Task<JobQueue> GetJobQueueById(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _jobQueueRepository.GetById(id, cancellationToken);
        }

        public async Task<List<JobTaskStatus>> GetJobTaskStatus(int id, string filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var job = await _jobQueueRepository.GetById(id, cancellationToken);

            if (!string.IsNullOrEmpty(job?.JobTasksStatus))
            {
                var tasks = JsonConvert.DeserializeObject<List<JobTaskStatus>>(job.JobTasksStatus);
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
            var job = await _jobQueueRepository.GetSingleBySpec(queuedJobSpec, cancellationToken);

            if (!string.IsNullOrEmpty(engine))
            {
                if (job == null)
                {
                    var pendingJobSpec = new JobQueueFilterSpecification(JobStatus.Queued, engine);
                    job = await _jobQueueRepository.GetSingleBySpec(pendingJobSpec, cancellationToken);
                }

                try
                {
                    job.CatapultEngineId = engine;
                    await _jobQueueRepository.Update(job, cancellationToken);
                }
                catch (Exception)
                {
                    job = null;
                }
            }

            return job;
        }

        private async Task<string> GetNextJobCode()
        {
            var sequence = await _jobCounterService.GetNextSequence();
            return $"{DateTime.UtcNow:yyyyMMdd}.{sequence}";
        }

        public Task<string> GetJobLogs(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _textWriter.Read($"{JobQueueLog.FolderNamePrefix}{id}", null);
        }
    }
}
