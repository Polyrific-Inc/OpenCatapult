// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Security.Claims;

namespace Polyrific.Catapult.Api.Identity
{
    public static class ClaimPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            var userIdClaim = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userIdClaim, out var userId))
                return userId;

            return 0;
        }
    }
}
