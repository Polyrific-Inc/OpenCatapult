// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.ApplicationSetting;
using Polyrific.Catapult.Shared.Dto.Constants;

namespace Polyrific.Catapult.Api.AutoMapperProfiles
{
    public class ApplicationSettingAutoMapperProfile : Profile
    {
        public ApplicationSettingAutoMapperProfile()
        {
            CreateMap<ApplicationSetting, ApplicationSettingDto>()
                .ForMember(m => m.AllowedValues, opt => opt.MapFrom(src => src.AllowedValues.Split(DataDelimiter.Comma, System.StringSplitOptions.RemoveEmptyEntries)));
        }
    }
}
