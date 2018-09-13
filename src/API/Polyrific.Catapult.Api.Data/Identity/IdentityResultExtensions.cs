// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Polyrific.Catapult.Api.Data.Identity
{
    public static class IdentityResultExtensions
    {
        public static void ThrowErrorException(this IdentityResult result)
        {
            if (result.Errors.Any())
            {
                throw new Exception(string.Join(" ", result.Errors.Select(err => err.Description)));
            }
        }
    }
}