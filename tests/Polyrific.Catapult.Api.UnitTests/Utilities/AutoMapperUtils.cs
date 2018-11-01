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
                        cfg.AddProfile<ExternalServiceAutoMapperProfile>();
                        cfg.AddProfile<JobDefinitionAutoMapperProfile>();
                        cfg.AddProfile<JobTaskDefinitionAutoMapperProfile>();
                        cfg.AddProfile<JobQueueAutoMapperProfile>();
                        cfg.AddProfile<PluginAdditionalConfigAutoMapperProfile>();
                        cfg.AddProfile<PluginAutoMapperProfile>();
                        cfg.AddProfile<ProjectAutoMapperProfile>();
                        cfg.AddProfile<ProjectDataModelAutoMapperProfile>();
                        cfg.AddProfile<ProjectDataModelPropertyAutoMapperProfile>();
                        cfg.AddProfile<ProjectMemberAutoMapperProfile>();
                    });

                    _isInitialized = true;
                }
            }

            return Mapper.Instance;
        }
    }
}
