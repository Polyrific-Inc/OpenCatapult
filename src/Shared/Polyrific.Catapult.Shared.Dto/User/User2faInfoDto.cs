// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Shared.Dto.User
{
    public class User2faInfoDto
    {
        /// <summary>
        /// Is the two factor authentication enabled for the user?
        /// </summary>
        public bool Is2faEnabled { get; set; }

        /// <summary>
        /// The number of recovery code left for the authenticator
        /// </summary>
        public int RecoveryCodesLeft { get; set; }

        /// <summary>
        /// Has the authenticator been set up for the user?
        /// </summary>
        public bool HasAuthenticator { get; set; }
    }
}
