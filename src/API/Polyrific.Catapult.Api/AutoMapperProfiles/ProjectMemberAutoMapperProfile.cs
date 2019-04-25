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
            CreateMap<ProjectMember, ProjectMemberDto>()
                .ForMember(dest => dest.ProjectMemberRoleName, opt => opt.MapFrom(src => src.ProjectMemberRole.Name))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.ExternalAccountIds, opt => opt.MapFrom(src => src.User.ExternalAccountIds));
            CreateMap<NewProjectMemberDto, ProjectMemberDto>();
        }
    }
}
