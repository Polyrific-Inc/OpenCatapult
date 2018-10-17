// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.ProjectMember;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Member
{
    [Command(Description = "Update the role of a project member")]
    public class UpdateCommand : BaseCommand
    {
        private readonly IProjectMemberService _projectMemberService;
        private readonly IProjectService _projectService;
        private readonly IAccountService _accountService;

        public UpdateCommand(IConsole console, ILogger<UpdateCommand> logger,
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

        [Required]
        [Option("-r|--role <ROLE>", "Role of the project member", CommandOptionType.SingleValue)]
        [AllowedValues(MemberRole.Member, MemberRole.Contributor, MemberRole.Maintainer, MemberRole.Owner, IgnoreCase = true)]
        public string Role { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to update the role of {User} in project {Project}...");

            string message;

            var project = _projectService.GetProjectByName(Project).Result;
            var user = _accountService.GetUserByUserName(User).Result;
            var roleId = MemberRole.GetMemberRoleId(Role);

            if (project != null && user != null && roleId > 0)
            {
                var projectMember = _projectMemberService.GetProjectMemberByUserId(project.Id, int.Parse(user.Id)).Result;

                if (projectMember != null)
                {
                    _projectMemberService.UpdateProjectMember(project.Id, projectMember.Id, new UpdateProjectMemberDto
                    {
                        Id = projectMember.Id,
                        UserId = int.Parse(user.Id),
                        ProjectMemberRoleId = roleId
                    }).Wait();

                    message = $"User {User} has been assigned as {Role} in project {Project}";
                    Logger.LogInformation(message);
                    return message;
                }
            }

            message = $"Failed to update user {User}. Make sure the project name and user email are correct.";

            return message;
        }
    }
}
