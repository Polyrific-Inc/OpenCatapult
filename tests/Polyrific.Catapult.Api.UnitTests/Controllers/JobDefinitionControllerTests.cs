// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Api.Controllers;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.UnitTests.Utilities;
using Polyrific.Catapult.Shared.Dto.JobDefinition;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Controllers
{
    public class JobDefinitionControllerTests
    {
        private readonly Mock<IJobDefinitionService> _jobDefinitionService;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<JobDefinitionController>> _logger;

        public JobDefinitionControllerTests()
        {
            _jobDefinitionService = new Mock<IJobDefinitionService>();

            _mapper = AutoMapperUtils.GetMapper();

            _logger = LoggerMock.GetLogger<JobDefinitionController>();
        }

        [Fact]
        public async void GetJobDefinitions_ReturnsJobDefinitionList()
        {
            _jobDefinitionService.Setup(s => s.GetJobDefinitions(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<JobDefinition>
                {
                    new JobDefinition
                    {
                        Id = 1,
                        Name = "Default"
                    }
                });

            var controller = new JobDefinitionController(_jobDefinitionService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetJobDefinitions(1);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<JobDefinitionDto>>(okActionResult.Value);
            Assert.NotEmpty(returnValue);
        }

        [Fact]
        public async void CreateJobDefinition_ReturnsCreatedJobDefinition()
        {
            _jobDefinitionService
                .Setup(s => s.AddJobDefinition(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var controller = new JobDefinitionController(_jobDefinitionService.Object, _mapper, _logger.Object);

            var dto = new CreateJobDefinitionDto
            {
                Name = "Default"
            };
            var result = await controller.CreateJobDefinition(1, dto);

            var okActionResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<JobDefinitionDto>(okActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void GetJobDefinitionById_ReturnsJobDefinition()
        {
            _jobDefinitionService.Setup(s => s.GetJobDefinitionById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) =>
                    new JobDefinition
                    {
                        Id = id,
                        Name = "Default"
                    });

            var controller = new JobDefinitionController(_jobDefinitionService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetJobDefinition(1, 1);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<JobDefinitionDto>(okActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void GetJobDefinitionByName_ReturnsJobDefinition()
        {
            _jobDefinitionService.Setup(s => s.GetJobDefinitionByName(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int projectId, string name, CancellationToken cancellationToken) =>
                    new JobDefinition
                    {
                        Id = 1,
                        Name = name,
                        ProjectId = projectId
                    });

            var controller = new JobDefinitionController(_jobDefinitionService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetJobDefinition(1, "Default");

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<JobDefinitionDto>(okActionResult.Value);
            Assert.Equal("Default", returnValue.Name);
            Assert.Equal(1, returnValue.ProjectId);
        }

        [Fact]
        public async void UpdateJobDefinition_ReturnsSuccess()
        {
            _jobDefinitionService.Setup(s => s.RenameJobDefinition(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var controller = new JobDefinitionController(_jobDefinitionService.Object, _mapper, _logger.Object);

            var result = await controller.UpdateJobDefinition(1, 1, new UpdateJobDefinitionDto()
            {
                Id = 1
            });

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void UpdateJobDefinition_ReturnsBadRequest()
        {
            var controller = new JobDefinitionController(_jobDefinitionService.Object, _mapper, _logger.Object);

            var result = await controller.UpdateJobDefinition(1, 1, new UpdateJobDefinitionDto());

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Job Id doesn't match.", badRequestResult.Value);
        }

        [Fact]
        public async void DeleteJobDefinition_ReturnsNoContent()
        {
            _jobDefinitionService.Setup(s => s.DeleteJobDefinition(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var controller = new JobDefinitionController(_jobDefinitionService.Object, _mapper, _logger.Object);

            var result = await controller.DeleteJobDefinition(1, 1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void DeleteJobDefinitions_ReturnsNoContent()
        {
            _jobDefinitionService.Setup(s => s.DeleteJobDefinitions(It.IsAny<int>(), It.IsAny<int[]>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var controller = new JobDefinitionController(_jobDefinitionService.Object, _mapper, _logger.Object);

            var result = await controller.DeleteJobDefinitions(1, new int[] { 1, 2 });

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void CreateJobTaskDefinition_ReturnsCreatedJobTaskDefinition()
        {
            _jobDefinitionService
                .Setup(s => s.AddJobTaskDefinition(It.IsAny<JobTaskDefinition>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var controller = new JobDefinitionController(_jobDefinitionService.Object, _mapper, _logger.Object);

            var dto = new CreateJobTaskDefinitionDto
            {
                Name = "Generate"
            };
            var result = await controller.CreateJobTaskDefinition(1, 1, dto);

            var okActionResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<JobTaskDefinitionDto>(okActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void CreateJobTaskDefinitions_ReturnsCreatedJobTaskDefinitions()
        {
            _jobDefinitionService
                .Setup(s => s.AddJobTaskDefinitions(It.IsAny<int>(), It.IsAny<List<JobTaskDefinition>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<int> { 1 });

            var controller = new JobDefinitionController(_jobDefinitionService.Object, _mapper, _logger.Object);

            var dto = new List<CreateJobTaskDefinitionDto>
            {
                new CreateJobTaskDefinitionDto
                {
                    Name = "Generate"
                }
            };

            var result = await controller.CreateJobTaskDefinitions(1, 1, dto);

            var okActionResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<List<JobTaskDefinitionDto>>(okActionResult.Value);
            Assert.NotEmpty(returnValue);
        }

        [Fact]
        public async void GetJobTaskDefinitions_ReturnsJobTaskDefinitionList()
        {
            _jobDefinitionService.Setup(s => s.GetJobTaskDefinitions(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<JobTaskDefinition>
                {
                    new JobTaskDefinition
                    {
                        Id = 1,
                        Name = "Generate"
                    }
                });

            var controller = new JobDefinitionController(_jobDefinitionService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetJobTaskDefinitions(1);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<JobTaskDefinitionDto>>(okActionResult.Value);
            Assert.NotEmpty(returnValue);
        }

        [Fact]
        public async void GetJobTaskDefinitionById_ReturnsJobTaskDefinition()
        {
            _jobDefinitionService.Setup(s => s.GetJobTaskDefinitionById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) =>
                    new JobTaskDefinition
                    {
                        Id = id,
                        Name = "Generate"
                    });

            var controller = new JobDefinitionController(_jobDefinitionService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetJobTaskDefinition(1, 1, 1);

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<JobTaskDefinitionDto>(okActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void GetJobTaskDefinitionByName_ReturnsJobTaskDefinition()
        {
            _jobDefinitionService.Setup(s => s.GetJobTaskDefinitionByName(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int jobDefinitionId, string name, CancellationToken cancellationToken) =>
                    new JobTaskDefinition
                    {
                        Id = 1,
                        Name = name,
                        JobDefinitionId = jobDefinitionId
                    });

            var controller = new JobDefinitionController(_jobDefinitionService.Object, _mapper,
                _logger.Object);

            var result = await controller.GetJobTaskDefinition(1, 1, "Generate");

            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<JobTaskDefinitionDto>(okActionResult.Value);
            Assert.Equal("Generate", returnValue.Name);
            Assert.Equal(1, returnValue.JobDefinitionId);
        }

        [Fact]
        public async void UpdateJobTaskDefinition_ReturnsSuccess()
        {
            _jobDefinitionService.Setup(s => s.UpdateJobTaskDefinition(It.IsAny<JobTaskDefinition>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var controller = new JobDefinitionController(_jobDefinitionService.Object, _mapper, _logger.Object);

            var result = await controller.UpdateJobTaskDefinition(1, 1, 1, new UpdateJobTaskDefinitionDto()
            {
                Id = 1
            });

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void UpdateJobTaskDefinition_ReturnsBadRequest()
        {
            var controller = new JobDefinitionController(_jobDefinitionService.Object, _mapper, _logger.Object);

            var result = await controller.UpdateJobTaskDefinition(1, 1, 1, new UpdateJobTaskDefinitionDto());

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Task Id doesn't match.", badRequestResult.Value);
        }

        [Fact]
        public async void UpdateJobTaskConfig_ReturnsSuccess()
        {
            _jobDefinitionService.Setup(s => s.UpdateJobTaskConfig(It.IsAny<int>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var controller = new JobDefinitionController(_jobDefinitionService.Object, _mapper, _logger.Object);

            var result = await controller.UpdateJobTaskConfig(1, 1, 1, new UpdateJobTaskConfigDto()
            {
                Id = 1
            });

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void UpdateJobTaskConfig_ReturnsBadRequest()
        {
            var controller = new JobDefinitionController(_jobDefinitionService.Object, _mapper, _logger.Object);

            var result = await controller.UpdateJobTaskConfig(1, 1, 1, new UpdateJobTaskConfigDto());

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Task Id doesn't match.", badRequestResult.Value);
        }

        [Fact]
        public async void DeleteJobTaskDefinition_ReturnsNoContent()
        {
            _jobDefinitionService.Setup(s => s.DeleteJobTaskDefinition(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var controller = new JobDefinitionController(_jobDefinitionService.Object, _mapper, _logger.Object);

            var result = await controller.DeleteJobTaskDefinition(1, 1, 1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
