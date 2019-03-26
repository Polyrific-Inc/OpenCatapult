// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.JobDefinition;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Shared.ApiClient
{
    public class JobDefinitionService : BaseService, IJobDefinitionService
    {
        public JobDefinitionService(IApiClient api) : base(api)
        {
        }

        public async Task<JobDefinitionDto> CreateJobDefinition(int projectId, CreateJobDefinitionDto dto)
        {
            var path = $"project/{projectId}/job";

            return await Api.Post<CreateJobDefinitionDto, JobDefinitionDto>(path, dto);
        }

        public async Task<JobTaskDefinitionDto> CreateJobTaskDefinition(int projectId, int jobId, CreateJobTaskDefinitionDto dto)
        {
            var path = $"project/{projectId}/job/{jobId}/task";

            return await Api.Post<CreateJobTaskDefinitionDto, JobTaskDefinitionDto>(path, dto);
        }

        public async Task<List<JobTaskDefinitionDto>> CreateJobTaskDefinitions(int projectId, int jobId, List<CreateJobTaskDefinitionDto> dto)
        {
            var path = $"project/{projectId}/job/{jobId}/tasks";

            return await Api.Post<List<CreateJobTaskDefinitionDto>, List<JobTaskDefinitionDto>>(path, dto);
        }

        public async Task DeleteJobDefinition(int projectId, int jobId)
        {
            var path = $"project/{projectId}/job/{jobId}";

            await Api.Delete(path);
        }

        public async Task DeleteJobTaskDefinition(int projectId, int jobId, int taskId)
        {
            var path = $"project/{projectId}/job/{jobId}/task/{taskId}";

            await Api.Delete(path);
        }

        public async Task<JobDefinitionDto> GetJobDefinition(int projectId, int jobId)
        {
            var path = $"project/{projectId}/job/{jobId}";

            return await Api.Get<JobDefinitionDto>(path);
        }

        public async Task<JobDefinitionDto> GetJobDefinitionByName(int projectId, string jobName)
        {
            var path = $"project/{projectId}/job/name/{jobName}";

            return await Api.Get<JobDefinitionDto>(path);
        }

        public async Task<JobDefinitionDto> GetDeletionJobDefinition(int projectId)
        {
            var path = $"project/{projectId}/job/deletion";

            return await Api.Get<JobDefinitionDto>(path);
        }

        public async Task<List<JobDefinitionDto>> GetJobDefinitions(int projectId)
        {
            var path = $"project/{projectId}/job";

            return await Api.Get<List<JobDefinitionDto>>(path);
        }

        public async Task<JobTaskDefinitionDto> GetJobTaskDefinition(int projectId, int jobId, int taskId)
        {
            var path = $"project/{projectId}/job/{jobId}/task/{taskId}";

            return await Api.Get<JobTaskDefinitionDto>(path);
        }

        public async Task<JobTaskDefinitionDto> GetJobTaskDefinitionByName(int projectId, int jobId, string taskName)
        {
            var path = $"project/{projectId}/job/{jobId}/task/name/{taskName}";

            return await Api.Get<JobTaskDefinitionDto>(path);
        }

        public async Task<List<JobTaskDefinitionDto>> GetJobTaskDefinitions(int projectId, int jobId)
        {
            var path = $"project/{projectId}/job/{jobId}/task";

            return await Api.Get<List<JobTaskDefinitionDto>>(path);
        }

        public async Task UpdateJobDefinition(int projectId, int jobId, UpdateJobDefinitionDto dto)
        {
            var path = $"project/{projectId}/job/{jobId}";

            await Api.Put(path, dto);
        }

        public async Task UpdateJobTaskDefinition(int projectId, int jobId, int taskId, UpdateJobTaskDefinitionDto dto)
        {
            var path = $"project/{projectId}/job/{jobId}/task/{taskId}";

            await Api.Put(path, dto);
        }
    }
}
