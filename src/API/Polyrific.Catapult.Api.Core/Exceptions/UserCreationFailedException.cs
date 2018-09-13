// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class UserCreationFailedException : Exception
    {
        public string UserName { get; set; }

        public UserCreationFailedException(string userName)
            : base($"User \"{userName}\" was failed to create.")
        {
            UserName = userName;
        }

        public UserCreationFailedException(string userName, Exception ex)
            : base($"User \"{userName}\" was failed to create.", ex)
        {
            UserName = userName;
        }
    }
}