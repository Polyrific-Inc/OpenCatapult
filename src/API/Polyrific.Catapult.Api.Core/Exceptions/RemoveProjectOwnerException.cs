// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class RemoveProjectOwnerException : Exception
    {
        public int CurrentUserId { get; set; }

        public RemoveProjectOwnerException(int currentUserId)
            : base($"User {currentUserId} cannot remove the project owner.")
        {
            CurrentUserId = currentUserId;
        }
    }
}
