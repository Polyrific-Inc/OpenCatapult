// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.JobQueue;

namespace Polyrific.Catapult.Api.AutoMapperProfiles
{
    public class JobQueueAutoMapperProfile : Profile
    {
        public JobQueueAutoMapperProfile()
        {
            CreateMap<JobQueue, JobDto>();

            CreateMap<NewJobDto, JobDto>();

            CreateMap<UpdateJobDto, JobQueue>();
        }
    }
}