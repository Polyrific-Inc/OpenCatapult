// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.Plugin;

namespace Polyrific.Catapult.Api.AutoMapperProfiles
{
    public class PluginAdditionalConfigAutoMapperProfile : Profile
    {
        public PluginAdditionalConfigAutoMapperProfile()
        {
            CreateMap<PluginAdditionalConfigDto, PluginAdditionalConfig>();
            CreateMap<PluginAdditionalConfig, PluginAdditionalConfigDto>();
        }
    }
}
