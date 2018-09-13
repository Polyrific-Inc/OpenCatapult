// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class UserDeletionFailedException : Exception
    {
        public int UserId { get; set; }

        public UserDeletionFailedException(int userId)
            : base($"User \"{userId}\" was failed to delete.")
        {
            UserId = userId;
        }

        public UserDeletionFailedException(int userId, Exception ex)
            : base($"User \"{userId}\" was failed to delete.", ex)
        {
            UserId = userId;
        }
    }
}