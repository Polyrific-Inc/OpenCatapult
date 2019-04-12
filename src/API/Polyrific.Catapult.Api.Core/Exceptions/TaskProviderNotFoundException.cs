// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class TaskProviderNotFoundException : Exception
    {
        public TaskProviderNotFoundException(int providerId)
            : base($"Task Provider \"{providerId}\" was not found.")
        {
            ProviderId = providerId;
        }

        public int ProviderId { get; }
    }
}
