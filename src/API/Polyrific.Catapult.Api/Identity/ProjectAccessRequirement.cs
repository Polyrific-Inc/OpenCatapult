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

        public bool IsAllowedByMemberRole(string userRole)
        {
            if (string.IsNullOrEmpty(MemberRole))
                return true;

            var userRoleRank = GetMemberRoleRank(userRole);
            var expectedRoleRank = GetMemberRoleRank(MemberRole);

            return userRoleRank > 0 && userRoleRank <= expectedRoleRank;
        }

        private int GetMemberRoleRank(string member)
        {
            switch (member)
            {
                case Shared.Dto.Constants.MemberRole.Owner:
                    return 1;
                case Shared.Dto.Constants.MemberRole.Maintainer:
                    return 2;
                case Shared.Dto.Constants.MemberRole.Contributor:
                    return 3;
                case Shared.Dto.Constants.MemberRole.Member:
                    return 4;
                default:
                    return 0;
            }
        }
    }
}
