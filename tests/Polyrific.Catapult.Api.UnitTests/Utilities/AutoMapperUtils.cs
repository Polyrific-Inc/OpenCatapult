// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Polyrific.Catapult.Api.AutoMapperProfiles;
using Polyrific.Catapult.Api.Core.AutoMapperProfiles;

namespace Polyrific.Catapult.Api.UnitTests.Utilities
{
    public static class AutoMapperUtils
    {
        private static readonly object Lock = new object();
        private static bool _isInitialized;

        public static IMapper GetMapper()
        {
            lock (Lock)
            {
                if (!_isInitialized)
                {
                    Mapper.Initialize(cfg =>
                    {
                        cfg.AddProfile<ProjectTemplateAutoMapperProfile>();
                        cfg.AddProfile<UserAutoMapperProfile>();
                    });

                    _isInitialized = true;
                }
            }

            return Mapper.Instance;
        }
    }
}
