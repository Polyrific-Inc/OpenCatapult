// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class UpdatePasswordFailedException : Exception
    {
        public int UserId { get; set; }

        public UpdatePasswordFailedException(int userId)
            : base($"Failed to update password for user \"{userId}\".")
        {
            UserId = userId;
        }

        public UpdatePasswordFailedException(int userId, Exception ex)
            : base($"Failed to update password for user \"{userId}\".", ex)
        {
            UserId = userId;
        }
    }
}