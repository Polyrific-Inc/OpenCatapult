// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class TaskProviderAdditionalConfigAllowedValuesException : Exception
    {
        public string AdditionalConfigName { get; set; }
        public string ProviderName { get; set; }
        public string AllowedValues { get; set; }

        public TaskProviderAdditionalConfigAllowedValuesException(string additionalConfigName, string providerName, string allowedValues)
            : base($"Additional config \"{additionalConfigName}\" in Task Provider {providerName} only allows the follwing values: {allowedValues}")
        {
            AdditionalConfigName = additionalConfigName;
            ProviderName = providerName;
            AllowedValues = allowedValues;
        }
    }
}
