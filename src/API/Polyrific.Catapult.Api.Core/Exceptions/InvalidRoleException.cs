// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class InvalidRoleException : Exception
    {
        public InvalidRoleException(string roleName) : base($"Role \"{roleName}\" is not valid.")
        {
            RoleName = roleName;
        }

        public string RoleName { get; }
    }
}