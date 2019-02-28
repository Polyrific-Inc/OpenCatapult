// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Moq;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Core.Specifications;
using Polyrific.Catapult.Shared.Common.Interface;
using Polyrific.Catapult.Shared.Dto.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Core.Services
{
    public class JobQueueServiceTests
    {
        private readonly List<JobQueue> _data;
        private readonly Mock<IJobQueueRepository> _jobQueueRepository;
        private readonly Mock<IProjectRepository> _projectRepository;
        private readonly Mock<IJobCounterService> _jobCounterService;
        private readonly Mock<ITextWriter> _textWriter;

        public JobQueueServiceTests()
        {
            _data = new List<JobQueue>
            {
                new JobQueue
                {
                    Id = 1,
                    Code = "20180817.1",
                    ProjectId = 1,
                    JobType = JobType.Create,
                    Status = JobStatus.Completed
                }
            };
            
            _jobQueueRepository = new Mock<IJobQueueRepository>();
            _jobQueueRepository.Setup(r =>
                    r.GetBySpec(It.IsAny<JobQueueFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobQueueFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.Where(spec.Criteria.Compile()));
            _jobQueueRepository.Setup(r =>
                    r.GetSingleBySpec(It.IsAny<JobQueueFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobQueueFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.OrderBy(spec.OrderBy.Compile()).FirstOrDefault(spec.Criteria.Compile()));
            _jobQueueRepository.Setup(r =>
                    r.CountBySpec(It.IsAny<JobQueueFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobQueueFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.Count(spec.Criteria.Compile()));
            _jobQueueRepository.Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Callback((int id, CancellationToken cancellationToken) =>
                {
                    var entity = _data.FirstOrDefault(d => d.Id == id);
                    if (entity != null)
                        _data.Remove(entity);
                });
            _jobQueueRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => _data.FirstOrDefault(d => d.Id == id));
            _jobQueueRepository.Setup(r => r.Create(It.IsAny<JobQueue>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(2).Callback((JobQueue entity, CancellationToken cancellationToken) =>
                {
                    entity.Id = 2;
                    _data.Add(entity);
                });
            _jobQueueRepository.Setup(r => r.Update(It.IsAny<JobQueue>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Callback((JobQueue entity, CancellationToken cancellationToken) =>
                {
                    var oldEntity = _data.FirstOrDefault(d => d.Id == entity.Id);
                    if (oldEntity != null)
                    {
                        _data.Remove(oldEntity);
                        _data.Add(entity);
                    }
                });

            _projectRepository = new Mock<IProjectRepository>();
            _projectRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) =>
                    id == 1 ? new Project() {Id = id} : null);

            _jobCounterService = new Mock<IJobCounterService>();
            _jobCounterService.Setup(r => r.GetNextSequence(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _textWriter = new Mock<ITextWriter>();
        }

        [Fact]
        public async void AddJobQueue_ValidItem()
        {
            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            int newId = await jobQueueService.AddJobQueue(1, "localhost", JobType.Create, null);

            Assert.True(newId > 1);
            Assert.True(_data.Count > 1);
        }

        [Fact]
        public void AddJobQueue_QueueInProgressException()
        {
            _data.Add(new JobQueue
            {
                Id = 2,
                ProjectId = 1,
                JobType = JobType.Update,
                Status = JobStatus.Queued,
            });

            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            var exception = Record.ExceptionAsync(() => jobQueueService.AddJobQueue(1, "localhost", JobType.Create, null));

            Assert.IsType<JobQueueInProgressException>(exception?.Result);
        }

        [Fact]
        public void AddJobQueue_InvalidProject()
        {
            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            var exception = Record.ExceptionAsync(() => jobQueueService.AddJobQueue(2, "localhost", JobType.Create, null));

            Assert.IsType<ProjectNotFoundException>(exception?.Result);
        }

        [Fact]
        public async void GetJobQueues_FilterAll_ReturnItems()
        {
            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            var jobQueues = await jobQueueService.GetJobQueues(1, JobQueueFilterType.All);

            Assert.NotEmpty(jobQueues);
        }

        [Fact]
        public async void GetJobQueues_FilterCurrent_ReturnItems()
        {
            _data.Add(new JobQueue
            {
                Id = 2,
                ProjectId = 1,
                JobType = JobType.Update,
                Status = JobStatus.Queued,
            });

            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            var jobQueues = await jobQueueService.GetJobQueues(1, JobQueueFilterType.Current);

            Assert.NotEmpty(jobQueues);
        }

        [Fact]
        public async void GetJobQueues_FilterPast_ReturnItems()
        {
            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            var jobQueues = await jobQueueService.GetJobQueues(1, JobQueueFilterType.Past);

            Assert.NotEmpty(jobQueues);
        }

        [Fact]
        public async void GetJobQueues_FilterSucceeded_ReturnItems()
        {
            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            var jobQueues = await jobQueueService.GetJobQueues(1, JobQueueFilterType.Past);

            Assert.NotEmpty(jobQueues);
        }

        [Fact]
        public async void GetJobQueues_FilterFailed_ReturnItems()
        {
            _data.Add(new JobQueue
            {
                Id = 2,
                ProjectId = 1,
                JobType = JobType.Create,
                Status = JobStatus.Error,
            });

            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            var jobQueues = await jobQueueService.GetJobQueues(1, JobQueueFilterType.Past);

            Assert.NotEmpty(jobQueues);
        }

        [Fact]
        public async void GetJobQueues_ReturnEmpty()
        {
            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            var jobQueues = await jobQueueService.GetJobQueues(2, JobQueueFilterType.All);

            Assert.Empty(jobQueues);
        }

        [Fact]
        public void GetJobQueues_UnknownFilterType()
        {
            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            var exception = Record.ExceptionAsync(() => jobQueueService.GetJobQueues(1, "unknown"));

            Assert.IsType<FilterTypeNotFoundException>(exception?.Result);
        }

        [Fact]
        public async void GetJobQueueById_ReturnItem()
        {
            _jobQueueRepository.Setup(r =>
                    r.GetSingleBySpec(It.IsAny<JobQueueFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobQueueFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.FirstOrDefault(spec.Criteria.Compile()));

            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            var entity = await jobQueueService.GetJobQueueById(1, 1);

            Assert.NotNull(entity);
            Assert.Equal(1, entity.Id);
        }

        [Fact]
        public async void GetJobQueueById_ReturnNull()
        {
            _jobQueueRepository.Setup(r =>
                    r.GetSingleBySpec(It.IsAny<JobQueueFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobQueueFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.FirstOrDefault(spec.Criteria.Compile()));

            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            var jobQueue = await jobQueueService.GetJobQueueById(1, 2);

            Assert.Null(jobQueue);
        }

        [Fact]
        public async void GetJobQueueByCode_ReturnItem()
        {
            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            var entity = await jobQueueService.GetJobQueueByCode(1, "20180817.1");

            Assert.NotNull(entity);
            Assert.Equal(1, entity.Id);
        }

        [Fact]
        public async void GetJobQueueByCode_ReturnNull()
        {
            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            var entity = await jobQueueService.GetJobQueueByCode(1, "20180817.2");

            Assert.Null(entity);
        }

        [Fact]
        public async void CancelJobQueue_ValidItem()
        {
            _data.Add(new JobQueue
            {
                Id = 2,
                ProjectId = 1,
                JobType = JobType.Update,
                Status = JobStatus.Queued,
            });

            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            await jobQueueService.CancelJobQueue(2);

            var job = _data.First(d => d.Id == 2);
            Assert.Equal(JobStatus.Cancelled, job.Status);
        }

        [Fact]
        public void CancelJobQueue_JobCompletedException()
        {
            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            var exception = Record.ExceptionAsync(() => jobQueueService.CancelJobQueue(1));

            Assert.IsType<CancelCompletedJobException>(exception?.Result);
        }

        [Fact]
        public async void UpdateJobQueueWithProjectId_ValidItem()
        {
            _jobQueueRepository.Setup(r =>
                    r.GetSingleBySpec(It.IsAny<JobQueueFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobQueueFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.FirstOrDefault(spec.Criteria.Compile()));

            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            await jobQueueService.UpdateJobQueue(new JobQueue
            {
                Id = 1,
                ProjectId = 1,
                CatapultEngineId = "1"
            });

            var jobQueue = _data.First(d => d.Id == 1);

            Assert.Equal("1", jobQueue.CatapultEngineId);
        }

        [Fact]
        public async void UpdateJobQueue_ValidItem()
        {
            _jobQueueRepository.Setup(r =>
                    r.GetSingleBySpec(It.IsAny<JobQueueFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobQueueFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.FirstOrDefault(spec.Criteria.Compile()));

            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            await jobQueueService.UpdateJobQueue(new JobQueue
            {
                Id = 1,
                ProjectId = 1,
                CatapultEngineId = "1"
            });

            var jobQueue = _data.First(d => d.Id == 1);

            Assert.Equal("1", jobQueue.CatapultEngineId);
        }

        [Fact]
        public void UpdateJobQueue_JobProcessedByOtherEngineException()
        {
            _jobQueueRepository.Setup(r =>
                    r.GetSingleBySpec(It.IsAny<JobQueueFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobQueueFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.FirstOrDefault(spec.Criteria.Compile()));

            _data.Add(new JobQueue
            {
                Id = 2,
                ProjectId = 1,
                JobType = JobType.Update,
                Status = JobStatus.Processing,
                CatapultEngineId = "1"
            });

            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            var exception = Record.ExceptionAsync(() => jobQueueService.UpdateJobQueue(new JobQueue
            {
                Id = 2,
                ProjectId = 0,
                JobType = JobType.Update,
                Status = JobStatus.Processing,
                CatapultEngineId = "2"
            }));

            Assert.IsType<JobProcessedByOtherEngineException>(exception?.Result);
        }

        [Fact]
        public async void GetFirstQueuedJob_ReturnItem()
        {
            _data.Add(new JobQueue
            {
                Id = 2,
                ProjectId = 1,
                JobType = JobType.Update,
                Status = JobStatus.Queued,
                Created = DateTime.UtcNow.AddHours(1)
            });

            _data.Add(new JobQueue
            {
                Id = 3,
                ProjectId = 2,
                JobType = JobType.Update,
                Status = JobStatus.Queued,
                Created = DateTime.UtcNow
            });

            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            var job = await jobQueueService.GetFirstUnassignedQueuedJob("engine01");

            Assert.Equal(3, job.Id);
        }

        [Fact]
        public async void GetFirstQueuedJob_ReturnNull()
        {
            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            var job = await jobQueueService.GetFirstUnassignedQueuedJob("engine01");

            Assert.Null(job);
        }

        [Fact]
        public async void GetJobTaskStatus_FilterAll_ReturnItems()
        {
            _data.Add(new JobQueue
            {
                Id = 2,
                ProjectId = 1,
                JobType = JobType.Create,
                Status = JobStatus.Completed,
                JobTasksStatus = "[{\"Sequence\":1,\"TaskName\":\"Generate\",\"Status\":\"NotExecuted\",\"Remarks\":\"\"}]"
            });

            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            var jobQueues = await jobQueueService.GetJobTaskStatus(2, JobTaskStatusFilterType.All);

            Assert.NotEmpty(jobQueues);
        }

        [Fact]
        public async void GetJobTaskStatus_FilterLatest_ReturnItems()
        {
            _data.Add(new JobQueue
                {
                    Id = 2,
                    ProjectId = 1,
                    JobType = JobType.Create,
                    Status = JobStatus.Completed,
                    JobTasksStatus = "[{\"Sequence\":1,\"TaskName\":\"Generate\",\"Status\":\"Pending\",\"Remarks\":\"\"}, {\"Sequence\":1,\"TaskName\":\"Generate\",\"Status\":\"NotExecuted\",\"Remarks\":\"\"}]"
            });

            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            var jobQueues = await jobQueueService.GetJobTaskStatus(2, JobTaskStatusFilterType.Latest);

            Assert.Single(jobQueues);
            Assert.Equal(JobTaskStatusType.Pending, jobQueues[0].Status);
        }

        [Fact]
        public async void GetJobTaskStatus_ReturnEmpty()
        {
            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            var jobQueues = await jobQueueService.GetJobTaskStatus(1, JobTaskStatusFilterType.All);

            Assert.Empty(jobQueues);
        }

        [Fact]
        public void GetJobTaskStatus_FilterTypeNotFoundException()
        {
            _data.Add(new JobQueue
            {
                Id = 2,
                ProjectId = 1,
                JobType = JobType.Create,
                Status = JobStatus.Completed,
                JobTasksStatus = "[{\"Sequence\":1,\"TaskName\":\"Generate\",\"Status\":\"NotExecuted\",\"Remarks\":\"\"}]"
            });

            var jobQueueService = new JobQueueService(_jobQueueRepository.Object, _projectRepository.Object, _jobCounterService.Object, _textWriter.Object);
            var exception = Record.ExceptionAsync(() => jobQueueService.GetJobTaskStatus(2, "unknown"));

            Assert.IsType<FilterTypeNotFoundException>(exception?.Result);
        }
    }
}
