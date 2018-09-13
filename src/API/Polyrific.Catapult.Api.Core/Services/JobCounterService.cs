// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Specifications;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Core.Services
{
    public class JobCounterService : IJobCounterService
    {
        private readonly IJobCounterRepository _jobCounterRepository;
        
        public JobCounterService(IJobCounterRepository jobCounterRepository)
        {
            _jobCounterRepository = jobCounterRepository;
        }

        public async Task<int> GetNextSequence(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var today = DateTime.UtcNow.Date;
            var jobCounterSpec = new JobCounterFilterSpecification(today);
            var todaysCounter = await _jobCounterRepository.GetSingleBySpec(jobCounterSpec, cancellationToken);

            if (todaysCounter == null)
            {
                todaysCounter = new JobCounter
                {
                    Date = today,
                    Count = 1
                };

                await _jobCounterRepository.Create(todaysCounter, cancellationToken);
            }
            else
            {
                todaysCounter.Count++;
                await _jobCounterRepository.Update(todaysCounter, cancellationToken);
            }


            return todaysCounter.Count;
        }
    }
}