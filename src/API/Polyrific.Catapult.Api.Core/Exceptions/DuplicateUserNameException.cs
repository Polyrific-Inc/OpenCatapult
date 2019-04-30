// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class DuplicateUserNameException : Exception
    {
        public string UserName { get; set; }

        public DuplicateUserNameException(string userName)
            : base($"User with UserName \"{userName}\" already exists.")
        {
            UserName = userName;
        }
    }
}
