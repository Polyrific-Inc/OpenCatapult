// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class DuplicateUserEmailException : Exception
    {
        public string Email { get; set; }

        public DuplicateUserEmailException(string email)
            : base($"User with email \"{email}\" already exists.")
        {
            Email = email;
        }
    }
}
