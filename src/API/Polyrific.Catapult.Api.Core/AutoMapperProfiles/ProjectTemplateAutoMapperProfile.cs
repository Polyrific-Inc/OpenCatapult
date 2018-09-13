// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.AutoMapperProfiles
{
    public class ProjectTemplateAutoMapperProfile : Profile
    {
        public ProjectTemplateAutoMapperProfile()
        {
            CreateMap<Project, ProjectTemplate>();
            CreateMap<ProjectDataModel, ProjectDataModelTemplate>();
            CreateMap<ProjectDataModelProperty, ProjectDataModelPropertyTemplate>();
            CreateMap<JobDefinition, JobDefinitionTemplate>();
            CreateMap<JobTaskDefinition, JobTaskDefinitionTemplate>();
        }
    }
}
