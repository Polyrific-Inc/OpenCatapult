// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.Plugin;

namespace Polyrific.Catapult.Api.AutoMapperProfiles
{
    public class PluginAdditionalConfigAutoMapperProfile : Profile
    {
        public PluginAdditionalConfigAutoMapperProfile()
        {
            CreateMap<PluginAdditionalConfigDto, PluginAdditionalConfig>()
                .ForMember(dest => dest.AllowedValues, opt => opt.MapFrom(src => string.Join(DataDelimiter.Comma, src.AllowedValues)));
            CreateMap<PluginAdditionalConfig, PluginAdditionalConfigDto>()
                .ForMember(dest => dest.AllowedValues, opt => opt.MapFrom(src => src.AllowedValues.Split(DataDelimiter.Comma, System.StringSplitOptions.None)));
        }
    }
}
