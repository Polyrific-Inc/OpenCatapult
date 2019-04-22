// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Linq;
using AutoMapper;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.Provider;

namespace Polyrific.Catapult.Api.AutoMapperProfiles
{
    public class ProviderAutoMapperProfile : Profile
    {
        public ProviderAutoMapperProfile()
        {
            CreateMap<TaskProvider, TaskProviderDto>()
                .ForMember(dest => dest.RequiredServices, opt => opt.MapFrom(src => src.RequiredServicesString.Split(DataDelimiter.Comma, StringSplitOptions.None)))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Tag.Name).ToArray()));

            CreateMap<NewTaskProviderDto, TaskProvider>();
        }
    }
}
