// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public string Name { get; set; }
        public int UserId { get; set; }

        public UserNotFoundException(string name)
            : base($"User \"{name}\" was not found.")
        {
            Name = name;
        }

        public UserNotFoundException(int userId)
            : base($"User Id \"{userId}\" was not found.")
        {
            UserId = userId;
        }
    }
}