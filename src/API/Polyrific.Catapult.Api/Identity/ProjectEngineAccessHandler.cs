// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Polyrific.Catapult.Shared.Dto.Constants;

namespace Polyrific.Catapult.Api.Identity
{
    public class ProjectEngineAccessHandler : AuthorizationHandler<ProjectAccessRequirement>
    {
        private const string AllowedMemberRole = MemberRole.Contributor;

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProjectAccessRequirement requirement)
        {
            if (!(context.Resource is AuthorizationFilterContext mvcContext))
                return Task.CompletedTask;

            if (context.User.IsInRole(UserRole.Engine) && requirement.IsAllowedByMemberRole(AllowedMemberRole))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
