// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.Identity;

namespace Polyrific.Catapult.Api.Data.Identity
{
    public sealed class ApplicationUserRole : IdentityUserRole<int>
    {
        public ApplicationUserRole()
        {
            
        }

        public ApplicationUserRole(int userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public ApplicationRole Role { get; set; }
    }
}
