// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using AutoMapper;
using Newtonsoft.Json;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.AutoMapperProfiles
{
    public class ProjectTemplateAutoMapperProfile : Profile
    {
        public ProjectTemplateAutoMapperProfile()
        {
            CreateMap<Project, ProjectTemplate>()
                .ForMember(dest => dest.Config, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<Dictionary<string, string>>(src.ConfigString)));
            CreateMap<ProjectDataModel, ProjectDataModelTemplate>();
            CreateMap<ProjectDataModelProperty, ProjectDataModelPropertyTemplate>();
            CreateMap<JobDefinition, JobDefinitionTemplate>();
            CreateMap<JobTaskDefinition, JobTaskDefinitionTemplate>()
                .ForMember(dest => dest.Config, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<Dictionary<string, string>>(src.ConfigString)));
        }
    }
}
