// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class TaskProviderAdditionalConfigRequiredException : Exception
    {
        public string AdditionalConfigName { get; set; }
        public string ProviderName { get; set; }

        public TaskProviderAdditionalConfigRequiredException(string additionalConfigName, string providerName)
            : base($"Task Provider {providerName} require additional config \"{additionalConfigName}\"")
        {
            AdditionalConfigName = additionalConfigName;
            ProviderName = providerName;
        }
    }
}
