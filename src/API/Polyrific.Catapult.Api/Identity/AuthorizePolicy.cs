// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Api.Identity
{
    public static class AuthorizePolicy
    {
        public const string ProjectAccess = "ProjectAccess";
        public const string ProjectOwnerAccess = "ProjectOwnerAccess";
        public const string ProjectMaintainerAccess = "ProjectMaintainerAccess";
        public const string ProjectContributorAccess = "ProjectContributorAccess";
        public const string ProjectMemberAccess = "ProjectMemberAccess";

        public const string UserRoleAdminAccess = "UserRoleAdminAccess";
        public const string UserRoleBasicAccess = "UserRoleBasicAccess";
        public const string UserRoleGuestAccess = "UserRoleGuestAccess";
        public const string UserRoleEngineAccess = "UserRoleEngineAccess";
    }
}