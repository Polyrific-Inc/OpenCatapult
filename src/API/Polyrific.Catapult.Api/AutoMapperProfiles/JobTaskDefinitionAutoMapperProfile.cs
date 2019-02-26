// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Newtonsoft.Json;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.JobDefinition;
using System.Collections.Generic;
using System.Linq;

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
                )
                .ForMember(
                    dest => dest.AdditionalConfigs,
                    opt => opt.MapFrom(src => JsonConvert.DeserializeObject<Dictionary<string, string>>(src.AdditionalConfigString))
                );
            CreateMap<CreateJobTaskDefinitionDto, JobTaskDefinition>()
                .ForMember(dest => dest.ConfigString, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Configs)))
                .ForMember(dest => dest.AdditionalConfigString, 
                    opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.AdditionalConfigs.Where(c => c.Value != null)
                        .ToDictionary(c => c.Key, c => c.Value))));

            CreateMap<UpdateJobTaskDefinitionDto, JobTaskDefinition>()
                .ForMember(dest => dest.ConfigString, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Configs)))
                .ForMember(dest => dest.AdditionalConfigString, 
                    opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.AdditionalConfigs.Where(c => c.Value != null)
                        .ToDictionary(c => c.Key, c => c.Value))));
        }
        
    }
}
