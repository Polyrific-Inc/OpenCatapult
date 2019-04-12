// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.Provider;

namespace Polyrific.Catapult.Api.AutoMapperProfiles
{
    public class ProviderAdditionalConfigAutoMapperProfile : Profile
    {
        public ProviderAdditionalConfigAutoMapperProfile()
        {
            CreateMap<TaskProviderAdditionalConfigDto, TaskProviderAdditionalConfig>()
                .ForMember(dest => dest.AllowedValues, opt => opt.MapFrom(src => string.Join(DataDelimiter.Comma, src.AllowedValues)));
            CreateMap<TaskProviderAdditionalConfig, TaskProviderAdditionalConfigDto>()
                .ForMember(dest => dest.AllowedValues, opt => opt.MapFrom(src => src.AllowedValues.Split(DataDelimiter.Comma, System.StringSplitOptions.None)));
        }
    }
}
