// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.JobDefinition;

namespace Polyrific.Catapult.Api.AutoMapperProfiles
{
    public class JobDefinitionAutoMapperProfile : Profile
    {
        public JobDefinitionAutoMapperProfile()
        {
            CreateMap<JobDefinition, JobDefinitionDto>();
            CreateMap<CreateJobDefinitionDto, JobDefinitionDto>();
            CreateMap<CreateJobDefinitionWithTasksDto, JobDefinition>();
        }
    }
}