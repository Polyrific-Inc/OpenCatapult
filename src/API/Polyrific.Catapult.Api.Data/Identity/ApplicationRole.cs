// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.Identity;

namespace Polyrific.Catapult.Api.Data.Identity
{
    public sealed class ApplicationRole : IdentityRole<int>
    {
        public ApplicationRole()
        {
            
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
            
        }

        public ApplicationRole(int roleId, string roleName) : base(roleName)
        {
            Id = roleId;
            NormalizedName = roleName.ToUpper();
        }
    }
}