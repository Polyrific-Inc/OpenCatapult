// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class PluginAdditionalConfigRequiredException : Exception
    {
        public string AdditionalConfigName { get; set; }
        public string ProviderName { get; set; }

        public PluginAdditionalConfigRequiredException(string additionalConfigName, string providerName)
            : base($"Provider {providerName} require additional config \"{additionalConfigName}\"")
        {
            AdditionalConfigName = additionalConfigName;
            ProviderName = providerName;
        }
    }
}
