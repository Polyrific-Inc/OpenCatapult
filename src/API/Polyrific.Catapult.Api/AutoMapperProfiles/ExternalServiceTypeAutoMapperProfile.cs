// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.ExternalServiceType;

namespace Polyrific.Catapult.Api.AutoMapperProfiles
{
    public class ExternalServiceTypeAutoMapperProfile : Profile
    {
        public ExternalServiceTypeAutoMapperProfile()
        {
            CreateMap<ExternalServiceType, ExternalServiceTypeDto>();
        }
    }
}
