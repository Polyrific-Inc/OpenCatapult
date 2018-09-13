// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class UserStatusNotFoundException : Exception
    {
        public string Status { get; set; }

        public UserStatusNotFoundException(string status)
            : base($"User status \"{status}\" was not found.")
        {
            Status = status;
        }
    }
}