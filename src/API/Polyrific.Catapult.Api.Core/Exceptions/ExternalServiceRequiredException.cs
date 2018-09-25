// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using Polyrific.Catapult.Api.Core.Services;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class ExternalServiceRequiredException : Exception
    {
        public string ExternalServiceTypeName { get; set; }
        public string ProviderName { get; set; }

        public ExternalServiceRequiredException(string externalServiceTypeName, string providerName)
            : base($"Provider {providerName} require \"{JobDefinitionService.GetServiceTaskConfigKey(externalServiceTypeName)}\" in the task config")
        {
            ExternalServiceTypeName = externalServiceTypeName;
            ProviderName = providerName;
        }
    }
}
