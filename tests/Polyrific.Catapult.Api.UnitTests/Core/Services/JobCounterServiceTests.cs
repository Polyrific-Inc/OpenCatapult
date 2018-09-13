// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Moq;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Core.Services
{
    public class JobCounterServiceTests
    {
        private readonly List<JobCounter> _data;
        private readonly Mock<IJobCounterRepository> _jobCounterRepository;

        public JobCounterServiceTests()
        {
            _data = new List<JobCounter>
            {
                new JobCounter
                {
                    Id = 1,
                    Date = DateTime.UtcNow.Date,
                    Count = 1
                }
            };

            _jobCounterRepository = new Mock<IJobCounterRepository>();
            _jobCounterRepository.Setup(r =>
                    r.GetSingleBySpec(It.IsAny<JobCounterFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobCounterFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.FirstOrDefault(spec.Criteria.Compile()));
            _jobCounterRepository.Setup(r => r.Create(It.IsAny<JobCounter>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(2).Callback((JobCounter entity, CancellationToken cancellationToken) =>
                {
                    entity.Id = 2;
                    _data.Add(entity);
                });
            _jobCounterRepository.Setup(r => r.Update(It.IsAny<JobCounter>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Callback((JobCounter entity, CancellationToken cancellationToken) =>
                {
                    var oldEntity = _data.FirstOrDefault(d => d.Id == entity.Id);
                    if (oldEntity != null)
                    {
                        _data.Remove(oldEntity);
                        _data.Add(entity);
                    }
                });
        }

        [Fact]
        public async void GetNextSequence_UpdateCurrentDateCounter()
        {
            var jobCounterService = new JobCounterService(_jobCounterRepository.Object);
            int nextSequence = await jobCounterService.GetNextSequence();

            Assert.Equal(2, nextSequence);
        }

        [Fact]
        public async void GetNextSequence_AddNewDateCounter()
        {
            _data.Clear();

            var jobCounterService = new JobCounterService(_jobCounterRepository.Object);
            int nextSequence = await jobCounterService.GetNextSequence();

            Assert.Equal(1, nextSequence);
        }
    }
}