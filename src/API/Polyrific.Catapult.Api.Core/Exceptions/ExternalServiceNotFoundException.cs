// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class ExternalServiceNotFoundException : Exception
    {
        public string ExternalServiceName { get; set; }

        public ExternalServiceNotFoundException(string externalServiceName)
            : base($"External service {externalServiceName} is not found")
        {
            ExternalServiceName = externalServiceName;
        }
    }
}
