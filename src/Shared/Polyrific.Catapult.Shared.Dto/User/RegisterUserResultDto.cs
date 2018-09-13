// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Shared.Dto.User
{
    public class RegisterUserResultDto
    {
        /// <summary>
        /// Id of the new user
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Token code which can be used to confirm user email
        /// </summary>
        public string ConfirmToken { get; set; }
    }
}