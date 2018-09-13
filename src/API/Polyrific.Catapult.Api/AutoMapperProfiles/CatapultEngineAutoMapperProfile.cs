// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.CatapultEngine;

namespace Polyrific.Catapult.Api.AutoMapperProfiles
{
    public class CatapultEngineAutoMapperProfile : Profile
    {
        public CatapultEngineAutoMapperProfile()
        {
            CreateMap<CatapultEngine, CatapultEngineDto>();
            CreateMap<CatapultEngineDto, CatapultEngine>();
        }
    }
}
