// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.ProjectMember;
using Polyrific.Catapult.Shared.Service;
using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Cli.Commands.Member
{
    [Command(Description = "Add user as a project member")]
    public class AddCommand : BaseCommand
    {
        private readonly IProjectMemberService _projectMemberService;
        private readonly IProjectService _projectService;
        private readonly IAccountService _accountService;

        public AddCommand(IConsole console, ILogger<AddCommand> logger,
            IProjectMemberService projectMemberService, IProjectService projectService, IAccountService accountService) : base(console, logger)
        {
            _projectMemberService = projectMemberService;
            _projectService = projectService;
            _accountService = accountService;
        }

        [Required]
        [Option("-p|--project <PROJECT>", "Name of the project", CommandOptionType.SingleValue)]
        public string Project { get; set; }

        [Required]
        [Option("-u|--user <USER>", "Username (email) of the user", CommandOptionType.SingleValue)]
        public string User { get; set; }

        [Option("-r|--role <ROLE>", "Role of the project member", CommandOptionType.SingleValue)]
        [AllowedValues(MemberRole.Member, MemberRole.Contributor, MemberRole.Maintainer, MemberRole.Owner, IgnoreCase = true)]
        public string Role { get; set; } = MemberRole.Member;

        public override string Execute()
        {
            string message = string.Empty;

            var project = _projectService.GetProjectByName(Project).Result;
            var user = _accountService.GetUserByUserName(User).Result;
            var roleId = MemberRole.GetMemberRoleId(Role);

            if (project != null)
            {
                var projectMember = _projectMemberService.CreateProjectMember(project.Id, new NewProjectMemberDto
                {
                    UserId = user != null ? int.Parse(user.Id) : 0,
                    Email = User,
                    ProjectId = project.Id,
                    ProjectMemberRoleId = roleId
                }).Result;

                message = projectMember.ToCliString($"User {User} is added to project {Project} as {Role}:");
                Logger.LogInformation(message);
            }
            else
            {
                message = $"Project {Project} is not found";
            }

            return message;
        }
    }
}