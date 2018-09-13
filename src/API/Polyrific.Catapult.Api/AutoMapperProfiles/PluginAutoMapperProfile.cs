// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.Plugin;

namespace Polyrific.Catapult.Api.AutoMapperProfiles
{
    public class PluginAutoMapperProfile : Profile
    {
        public PluginAutoMapperProfile()
        {
            CreateMap<Plugin, PluginDto>()
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.Created));

            CreateMap<NewPluginDto, Plugin>();
        }
    }
}
