// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.ExternalServiceType;

namespace Polyrific.Catapult.Api.AutoMapperProfiles
{
    public class ExternalServicePropertyAutoMapperProfile : Profile
    {
        public ExternalServicePropertyAutoMapperProfile()
        {
            CreateMap<ExternalServiceProperty, ExternalServicePropertyDto>()
                .ForMember(dest => dest.AllowedValues, opt => opt.MapFrom(src => src.AllowedValues.Split(DataDelimiter.Comma, System.StringSplitOptions.None)));
        }
    }
}
