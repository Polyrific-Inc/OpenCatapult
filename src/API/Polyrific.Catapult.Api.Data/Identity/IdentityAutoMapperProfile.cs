﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Linq;
using AutoMapper;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Data.Identity
{
    public class IdentityAutoMapperProfile : Profile
    {
        public IdentityAutoMapperProfile()
        {
            CreateMap<User, ApplicationUser>()
                .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
                .ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
                .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore());
            CreateMap<ApplicationUser, User>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.UserProfile.IsActive))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.UserProfile.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.UserProfile.LastName))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Roles.FirstOrDefault().Role.Name));

            CreateMap<User, UserProfile>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));

            CreateMap<CatapultEngine, ApplicationUser>()
                .ForMember(dest => dest.IsCatapultEngine, opt => opt.UseValue(true))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Name));
            CreateMap<ApplicationUser, CatapultEngine>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.CatapultEngineProfile.IsActive))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.LastSeen, opt => opt.MapFrom(src => src.CatapultEngineProfile.LastSeen));
            CreateMap<CatapultEngine, CatapultEngineProfile>()
                .ForMember(dest => dest.LastSeen, opt => opt.MapFrom(src => src.LastSeen));
        }
    }
}
