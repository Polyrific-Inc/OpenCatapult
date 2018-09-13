// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.ProjectMember;

namespace Polyrific.Catapult.Api.AutoMapperProfiles
{
    public class ProjectMemberAutoMapperProfile : Profile
    {
        public ProjectMemberAutoMapperProfile()
        {
            CreateMap<ProjectMember, ProjectMemberDto>();
            CreateMap<NewProjectMemberDto, ProjectMemberDto>();
        }
    }
}
