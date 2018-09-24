// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class IncorrectExternalServiceTypeException : Exception
    {
        public string ExternalServiceName { get; set; }
        public string RequiredServiceName { get; set; }

        public IncorrectExternalServiceTypeException(string externalServiceName, string requiredServiceName)
            : base($"The external service {externalServiceName} is not a {requiredServiceName} service")
        {
            ExternalServiceName = externalServiceName;
            RequiredServiceName = requiredServiceName;
        }
    }
}
