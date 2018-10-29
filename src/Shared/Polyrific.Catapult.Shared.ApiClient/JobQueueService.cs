// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.JobQueue;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Shared.ApiClient
{
    public class JobQueueService : BaseService, IJobQueueService
    {
        public JobQueueService(IApiClient api) : base(api)
        {
        }

        public async Task CancelJobQueue(int projectId, int queueId)
        {
            var path = $"project/{projectId}/queue/{queueId}/cancel";

            await Api.Post<object, object>(path, null);
        }

        public async Task<JobDto> CheckJob()
        {
            var path = $"queue";

            return await Api.Get<JobDto>(path);
        }

        public async Task<JobDto> CreateJobQueue(int projectId, NewJobDto newJobQueue)
        {
            var path = $"project/{projectId}/queue";

            return await Api.Post<NewJobDto, JobDto>(path, newJobQueue);
        }

        public async Task<string> GetJobLogs(int projectId, int queueId)
        {
            var path = $"project/{projectId}/queue/{queueId}/logs";

            return await Api.Get<string>(path);
        }

        public async Task<JobDto> GetJobQueue(int projectId, int queueId)
        {
            var path = $"project/{projectId}/queue/{queueId}";

            return await Api.Get<JobDto>(path);
        }

        public async Task<JobDto> GetJobQueue(int projectId, string queueCode)
        {
            var path = $"project/{projectId}/queue/code/{queueCode}";

            return await Api.Get<JobDto>(path);
        }

        public async Task<List<JobDto>> GetJobQueues(int projectId, string filter)
        {
            var path = $"project/{projectId}/queue?filter={filter}";

            return await Api.Get<List<JobDto>>(path);
        }

        public async Task<List<JobTaskStatusDto>> GetJobQueueStatus(int projectId, int queueId, string filter)
        {
            var path = $"project/{projectId}/queue/{queueId}/status?filter={filter}";
            return await Api.Get<List<JobTaskStatusDto>>(path);
        }

        public async Task UpdateJobQueue(int queueId, UpdateJobDto job)
        {
            var path = $"queue/{queueId}";

            await Api.Put(path, job);
        }
    }
}
