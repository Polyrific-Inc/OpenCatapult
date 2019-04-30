// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.HelpContext;

namespace Polyrific.Catapult.Api.AutoMapperProfiles
{
    public class HelpContextAutoMapperProfile : Profile
    {
        public HelpContextAutoMapperProfile()
        {
            CreateMap<HelpContext, HelpContextDto>();
            CreateMap<HelpContextDto, HelpContext>();
        }
    }
}
