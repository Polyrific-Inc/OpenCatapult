// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class TaskProviderNotInstalledException : Exception
    {
        public string ProviderName { get; set; }

        public TaskProviderNotInstalledException(string providerName)
            : base($"Task Provider \"{providerName}\" is not installed.")
        {
            ProviderName = providerName;
        }
    }
}
