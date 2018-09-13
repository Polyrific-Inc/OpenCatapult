// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace Polyrific.Catapult.Api.AutoMapperProfiles
{
    public class ProjectDataModelAutoMapperProfile : Profile
    {
        public ProjectDataModelAutoMapperProfile()
        {
            CreateMap<ProjectDataModel, ProjectDataModelDto>();
            CreateMap<UpdateProjectDataModelDto, ProjectDataModel>();
            CreateMap<CreateProjectDataModelDto, ProjectDataModelDto>();
            CreateMap<CreateProjectDataModelWithPropertiesDto, ProjectDataModel>();
        }
    }
}
