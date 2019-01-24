// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Api.Controllers;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.UnitTests.Utilities;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.JobQueue;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Controllers
{
    public class JobQueueControllerTests
    {
        private readonly Mock<IJobQueueService> _jobQueueService;
        private readonly Mock<ICatapultEngineService> _catapultEngineService;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<JobQueueController>> _logger;

        public JobQueueControllerTests()
        {
            _jobQueueService = new Mock<IJobQueueService>();

            _catapultEngineService = new Mock<ICatapultEngineService>();

            _mapper = AutoMapperUtils.GetMapper();

            _logger = LoggerMock.GetLogger<JobQueueController>();
        }

        [Fact]
        public async void GetJobQueues_ReturnsJobQueueList()
        {
            _jobQueueService.Setup(s => s.GetJobQueues(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<JobQueue>
                {
                    new JobQueue
                    {
                        Id = 1
                    }
                });

            var controller = new JobQueueController(_jobQueueService.Object, _catapultEngineService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetJobQueues(1);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<JobDto>>(okActionResult.Value);
            Assert.NotEmpty(returnValue);
        }

        [Fact]
        public async void GetJobQueue_ReturnsJobQueue()
        {
            _jobQueueService.Setup(s => s.GetJobQueueById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) =>
                    new JobQueue
                    {
                        Id = id
                    });

            var controller = new JobQueueController(_jobQueueService.Object, _catapultEngineService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetJobQueue(1, 1);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<JobDto>(okActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void GetJobQueueByCode_ReturnsJobQueue()
        {
            _jobQueueService.Setup(s => s.GetJobQueueByCode(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string code, CancellationToken cancellationToken) =>
                    new JobQueue
                    {
                        Id = 1,
                        Code = code
                    });

            var controller = new JobQueueController(_jobQueueService.Object, _catapultEngineService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetJobQueueByCode(1, "20181101.1");

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<JobDto>(okActionResult.Value);
            Assert.Equal("20181101.1", returnValue.Code);
        }

        [Fact]
        public async void CreateJobQueue_ReturnsCreatedJobQueue()
        {
            _jobQueueService
                .Setup(s => s.AddJobQueue(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
            _jobQueueService.Setup(s => s.GetJobQueueById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) =>
                    new JobQueue
                    {
                        Id = id
                    });

            var controller = new JobQueueController(_jobQueueService.Object, _catapultEngineService.Object, _mapper, _logger.Object);

            var result = await controller.CreateJobQueue(1, new NewJobDto
            {
                ProjectId = 1
            });

            var createAtRouteActionResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<JobDto>(createAtRouteActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void CreateJobQueue_ReturnsBadRequest()
        {
            var controller = new JobQueueController(_jobQueueService.Object, _catapultEngineService.Object, _mapper, _logger.Object);

            var result = await controller.CreateJobQueue(1, new NewJobDto());

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Project Id doesn't match.", badRequestResult.Value);
        }

        [Fact]
        public async void CancelJobQueue_ReturnsSuccess()
        {
            _jobQueueService.Setup(s => s.CancelJobQueue(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var controller = new JobQueueController(_jobQueueService.Object, _catapultEngineService.Object, _mapper, _logger.Object);

            var result = await controller.CancelJobQueue(1, 1);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void CheckJob_ReturnsSuccess()
        {
            _jobQueueService.Setup(s => s.GetFirstUnassignedQueuedJob(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string engineName, CancellationToken cancellationToken) =>
                    new JobQueue
                    {
                        CatapultEngineId = engineName
                    });

            var httpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new[]
                {
                    new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Name, "Engine01"),
                        new Claim(ClaimTypes.Role, UserRole.Engine)
                    })
                }),
            };

            var controller = new JobQueueController(_jobQueueService.Object, _catapultEngineService.Object, _mapper, _logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };

            var result = await controller.CheckJob();

            Assert.IsType<OkObjectResult>(result);

            _catapultEngineService.Verify(s => s.UpdateLastSeen("Engine01", null, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async void UpdateJobQueue_ReturnsSuccess()
        {
            _jobQueueService.Setup(s => s.UpdateJobQueue(It.IsAny<JobQueue>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var controller = new JobQueueController(_jobQueueService.Object, _catapultEngineService.Object, _mapper, _logger.Object);

            var result = await controller.UpdateJobQueue(1, new UpdateJobDto
            {
                Id = 1
            });

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void UpdateJobQueue_ReturnsBadRequest()
        {
            _jobQueueService.Setup(s => s.UpdateJobQueue(It.IsAny<JobQueue>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var controller = new JobQueueController(_jobQueueService.Object, _catapultEngineService.Object, _mapper, _logger.Object);

            var result = await controller.UpdateJobQueue(1, new UpdateJobDto());

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Queue Id doesn't match.", badRequestResult.Value);
        }

        [Fact]
        public async void GetJobQueueStatus_ReturnsobQueueStatusist()
        {
            _jobQueueService.Setup(s => s.GetJobTaskStatus(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<JobTaskStatus>
                {
                    new JobTaskStatus
                    {
                        TaskName = "Generate"
                    }
                });

            var controller = new JobQueueController(_jobQueueService.Object, _catapultEngineService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetJobQueueStatus(1, 1);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<JobTaskStatusDto>>(okActionResult.Value);
            Assert.NotEmpty(returnValue);
        }

        [Fact]
        public async void GetJobLogs_ReturnsJobQueue()
        {
            _jobQueueService.Setup(s => s.GetJobLogs(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("logs");

            var controller = new JobQueueController(_jobQueueService.Object, _catapultEngineService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetJobLogs(1, 1);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<string>(okActionResult.Value);
            Assert.Equal("logs", returnValue);
        }
    }
}
