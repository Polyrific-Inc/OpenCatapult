// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class ProviderNotFoundException : Exception
    {
        public ProviderNotFoundException(int providerId)
            : base($"Provider \"{providerId}\" was not found.")
        {
            ProviderId = providerId;
        }

        public int ProviderId { get; }
    }
}
