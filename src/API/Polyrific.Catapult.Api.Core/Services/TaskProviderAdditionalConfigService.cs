// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Specifications;

namespace Polyrific.Catapult.Api.Core.Services
{
    public class TaskProviderAdditionalConfigService : ITaskProviderAdditionalConfigService
    {
        private readonly ITaskProviderRepository _taskProviderRepository;
        private readonly ITaskProviderAdditionalConfigRepository _taskProviderAdditionalConfigRepository;

        public TaskProviderAdditionalConfigService(ITaskProviderRepository taskProviderRepository, ITaskProviderAdditionalConfigRepository taskProviderAdditionalConfigRepository)
        {
            _taskProviderRepository = taskProviderRepository;
            _taskProviderAdditionalConfigRepository = taskProviderAdditionalConfigRepository;
        }

        public async Task<List<int>> AddAdditionalConfigs(int taskProviderId, List<TaskProviderAdditionalConfig> additionalConfigs, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var taskProvider = await _taskProviderRepository.GetById(taskProviderId, cancellationToken);
            if (taskProvider == null)
            {
                throw new TaskProviderNotFoundException(taskProviderId);
            }
            
            additionalConfigs.ForEach(j => j.TaskProviderId = taskProviderId);

            return await _taskProviderAdditionalConfigRepository.AddRange(additionalConfigs, cancellationToken);
        }

        public async Task<List<TaskProviderAdditionalConfig>> GetByTaskProvider(int taskProviderId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var spec = new TaskProviderAdditionalConfigFilterSpecification(taskProviderId);
            var result = await _taskProviderAdditionalConfigRepository.GetBySpec(spec, cancellationToken);

            return result.ToList();
        }

        public async Task<List<TaskProviderAdditionalConfig>> GetByTaskProviderName(string taskProviderName, CancellationToken cancellationToken = default(CancellationToken))
        {
            var spec = new TaskProviderAdditionalConfigFilterSpecification(taskProviderName);
            var result = await _taskProviderAdditionalConfigRepository.GetBySpec(spec, cancellationToken);

            return result.ToList();
        }
    }
}
