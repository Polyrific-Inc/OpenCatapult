// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Api.Core.Entities
{
    public class User2faInfo
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
