// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;
using Polyrific.Catapult.Engine.Core.Services;
using Polyrific.Catapult.Shared.Dto.JobQueue;

namespace Polyrific.Catapult.Engine.ApiService
{
    public class JobQueueService : BaseService, IJobQueueService
    {
        protected JobQueueService(ApiClient api) : base(api)
        {
        }

        public Task<JobDto> GetQueuedJob()
        {
            return Api.Get<JobDto>("/queue");
        }
    }
}