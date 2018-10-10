// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace Polyrific.Catapult.Api.AutoMapperProfiles
{
    public class ProjectDataModelPropertyAutoMapperProfile : Profile
    {
        public ProjectDataModelPropertyAutoMapperProfile()
        {
            CreateMap<ProjectDataModelProperty, ProjectDataModelPropertyDto>()
                .ForMember(dest => dest.RelatedProjectDataModelName, opt => opt.MapFrom(src => src.RelatedProjectDataModel.Name));
            CreateMap<UpdateProjectDataModelPropertyDto, ProjectDataModelProperty>();
            CreateMap<CreateProjectDataModelPropertyDto, ProjectDataModelPropertyDto>();
            CreateMap<CreateProjectDataModelPropertyDto, ProjectDataModelProperty>();
        }
    }
}
