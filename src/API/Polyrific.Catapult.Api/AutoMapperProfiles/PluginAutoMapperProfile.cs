// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using AutoMapper;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.Plugin;

namespace Polyrific.Catapult.Api.AutoMapperProfiles
{
    public class PluginAutoMapperProfile : Profile
    {
        public PluginAutoMapperProfile()
        {
            CreateMap<Plugin, PluginDto>()
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.Created))
                .ForMember(dest => dest.RequiredServices, opt => opt.MapFrom(src => src.RequiredServicesString.Split(DataDelimiter.Comma, StringSplitOptions.None)));

            CreateMap<NewPluginDto, Plugin>();
        }
    }
}
