// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Shared.Dto.User
{
    public class TwoFactorKeyDto
    {
        /// <summary>
        /// Shared key for the two factor auth
        /// </summary>
        public string SharedKey { get; set; }

        /// <summary>
        /// Authenticator uri for generating qr code of two factor auth
        /// </summary>
        public string AuthenticatorUri { get; set; }
    }
}
