// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Engine.Core;
using Polyrific.Catapult.Shared.Dto.JobDefinition;
using Polyrific.Catapult.Shared.Dto.JobQueue;
using Polyrific.Catapult.Shared.Service;
using System.Collections.Generic;
using Xunit;

namespace Polyrific.Catapult.Engine.UnitTests.Core
{
    public class CatapultEngineTests
    {
        private readonly Mock<ICatapultEngineConfig> _engineConfig;
        private readonly Mock<ITaskRunner> _taskRunner;
        private readonly Mock<IHealthService> _healthService;
        private readonly Mock<IJobQueueService> _jobQueueService;
        private readonly Mock<IJobDefinitionService> _jobDefinitionService;
        private readonly Mock<ILogger<CatapultEngine>> _logger;

        public CatapultEngineTests()
        {
            _engineConfig = new Mock<ICatapultEngineConfig>();
            _taskRunner = new Mock<ITaskRunner>();
            _healthService = new Mock<IHealthService>();
            _jobQueueService = new Mock<IJobQueueService>();
            _jobDefinitionService = new Mock<IJobDefinitionService>();
            _logger = new Mock<ILogger<CatapultEngine>>();
        }

        [Fact]
        public async void CheckApiConnection_ReturnSuccess()
        {
            _healthService.Setup(s => s.CheckHealthSecure()).ReturnsAsync(true);

            var engine = new CatapultEngine(_engineConfig.Object, _taskRunner.Object, _healthService.Object,
                _jobQueueService.Object, _jobDefinitionService.Object, _logger.Object);

            var success = await engine.CheckApiConnection();

            Assert.True(success);
        }

        [Fact]
        public async void CheckApiConnection_ReturnFailed()
        {
            _healthService.Setup(s => s.CheckHealthSecure()).ReturnsAsync(false);

            var engine = new CatapultEngine(_engineConfig.Object, _taskRunner.Object, _healthService.Object,
                _jobQueueService.Object, _jobDefinitionService.Object, _logger.Object);

            var success = await engine.CheckApiConnection();

            Assert.False(success);
        }

        [Fact]
        public async void ExecuteJob_InvokeTaskRunner()
        {
            _jobQueueService.Setup(s => s.GetJobQueue(1, 1)).ReturnsAsync(new JobDto {Id = 1});
            _jobDefinitionService.Setup(s => s.GetJobTaskDefinitions(1, 1))
                .ReturnsAsync(new List<JobTaskDefinitionDto>());

            var engine = new CatapultEngine(_engineConfig.Object, _taskRunner.Object, _healthService.Object,
                _jobQueueService.Object, _jobDefinitionService.Object, _logger.Object);

            await engine.ExecuteJob(new JobDto{ProjectId = 1, Code = "20180817.1"});

            _taskRunner.Verify(tr => tr.Run(1, "20180817.1", It.IsAny<List<JobTaskDefinitionDto>>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void GetJobInQueue_ReturnItem()
        {
            _jobQueueService.Setup(s => s.CheckJob()).ReturnsAsync(new JobDto
            {
                Id = 1,
                ProjectId = 1
            });

            var engine = new CatapultEngine(_engineConfig.Object, _taskRunner.Object, _healthService.Object,
                _jobQueueService.Object, _jobDefinitionService.Object, _logger.Object);

            var result = await engine.GetJobInQueue();

            Assert.NotNull(result);
        }

        [Fact]
        public async void GetJobInQueue_ReturnEmpty()
        {
            _jobQueueService.Setup(s => s.CheckJob()).ReturnsAsync((JobDto)null);

            var engine = new CatapultEngine(_engineConfig.Object, _taskRunner.Object, _healthService.Object,
                _jobQueueService.Object, _jobDefinitionService.Object, _logger.Object);

            var result = await engine.GetJobInQueue();

            Assert.Null(result);
        }
    }
}