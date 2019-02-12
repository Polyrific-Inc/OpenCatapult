// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.Project;

namespace Polyrific.Catapult.Api.AutoMapperProfiles
{
    public class ProjectAutoMapperProfile : Profile
    {
        public ProjectAutoMapperProfile()
        {
            CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IsArchived ? ProjectStatusFilterType.Archived: ProjectStatusFilterType.Active));

            CreateMap<UpdateProjectDto, Project>();
        }
    }
}
