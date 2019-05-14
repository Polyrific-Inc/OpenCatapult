// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class InvalidDefaultJobDefinition : Exception
    {
        public InvalidDefaultJobDefinition()
            : base($"A deletion job definition cannot be set as the default job definition")
        {
        }
    }
}
