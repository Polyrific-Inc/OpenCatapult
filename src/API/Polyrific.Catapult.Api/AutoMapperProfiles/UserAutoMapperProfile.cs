// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Shared.Dto.User;

namespace Polyrific.Catapult.Api.AutoMapperProfiles
{
    public class UserAutoMapperProfile : Profile
    {
        public UserAutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UpdateUserDto, User>();
        }
    }
}
