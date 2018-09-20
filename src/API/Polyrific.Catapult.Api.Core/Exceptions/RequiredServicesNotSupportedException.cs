// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class RequiredServicesNotSupportedException : Exception
    {
        public string[] RequiredServices { get; set; }

        public RequiredServicesNotSupportedException(string[] requiredServices)
            : base($"The following required services are not supported: {string.Join(", ", requiredServices)}")
        {
            RequiredServices = requiredServices;
        }
    }
}
