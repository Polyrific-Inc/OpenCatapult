// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class ProviderNotInstalledException : Exception
    {
        public string ProviderName { get; set; }

        public ProviderNotInstalledException(string providerName)
            : base($"Provider \"{providerName}\" is not installed.")
        {
            ProviderName = providerName;
        }
    }
}
