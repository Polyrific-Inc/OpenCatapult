// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Newtonsoft.Json;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.Project;
using System.Collections.Generic;

namespace Polyrific.Catapult.Api.AutoMapperProfiles
{
    public class ProjectAutoMapperProfile : Profile
    {
        public ProjectAutoMapperProfile()
        {
            CreateMap<Project, ProjectDto>()
                .ForMember(
                    dest => dest.Config,
                    opt => opt.MapFrom(src => JsonConvert.DeserializeObject<Dictionary<string, string>>(src.ConfigString))
                );

            CreateMap<UpdateProjectDto, Project>()
                .ForMember(dest => dest.ConfigString, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Config)));
        }
    }
}
