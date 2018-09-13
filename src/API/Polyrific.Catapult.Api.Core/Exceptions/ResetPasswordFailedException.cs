// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class ResetPasswordFailedException : Exception
    {
        public int UserId { get; set; }

        public ResetPasswordFailedException(int userId)
            : base($"Failed to reset password for user \"{userId}\".")
        {
            UserId = userId;
        }

        public ResetPasswordFailedException(int userId, Exception ex)
            : base($"Failed to reset password for user \"{userId}\".", ex)
        {
            UserId = userId;
        }
    }
}