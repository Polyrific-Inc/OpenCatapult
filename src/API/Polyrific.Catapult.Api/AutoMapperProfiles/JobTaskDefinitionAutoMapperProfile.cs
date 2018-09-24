// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Newtonsoft.Json;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.JobDefinition;
using System.Collections.Generic;

namespace Polyrific.Catapult.Api.AutoMapperProfiles
{
    public class JobTaskDefinitionAutoMapperProfile : Profile
    {
        public JobTaskDefinitionAutoMapperProfile()
        {
            CreateMap<JobTaskDefinition, JobTaskDefinitionDto>()
                .ForMember(
                    dest => dest.Configs, 
                    opt => opt.MapFrom(src => JsonConvert.DeserializeObject<Dictionary<string, string>>(src.ConfigString))
                );
            CreateMap<CreateJobTaskDefinitionDto, JobTaskDefinition>()
                .ForMember(dest => dest.ConfigString, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Configs)));
            CreateMap<UpdateJobTaskDefinitionDto, JobTaskDefinition>()
                .ForMember(dest => dest.ConfigString, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Configs)));;
        }
        
    }
}
