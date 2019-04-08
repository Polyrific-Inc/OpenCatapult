// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.ManagedFile;

namespace Polyrific.Catapult.Api.AutoMapperProfiles
{
    public class ManagedFileAutoMapperProfile : Profile
    {
        public ManagedFileAutoMapperProfile()
        {
            CreateMap<ManagedFile, ManagedFileDto>();
            CreateMap<ManagedFileDto, ManagedFile>();
        }
    }
}
