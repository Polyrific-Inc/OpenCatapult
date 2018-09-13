// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.Authorization;

namespace Polyrific.Catapult.Api.Identity
{
    public class ProjectAccessRequirement : IAuthorizationRequirement
    {
        public string MemberRole { get; set; }

        public ProjectAccessRequirement()
        {
            
        }

        public ProjectAccessRequirement(string memberRole)
        {
            MemberRole = memberRole;
        }
    }
}