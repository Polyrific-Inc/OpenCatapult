// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.ProjectMember;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Member
{
    [Command("add", Description = "Add user as a project member")]
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
            Console.WriteLine($"Trying to add {User} as member of project {Project}...");

            string message;

            var project = _projectService.GetProjectByName(Project).Result;
            var user = _accountService.GetUserByUserName(User).Result;
            var roleId = MemberRole.GetMemberRoleId(Role);

            if (project != null)
            {
                var newMember = new NewProjectMemberDto
                {
                    UserId = user != null ? int.Parse(user.Id) : 0,
                    Email = User,
                    ProjectId = project.Id,
                    ProjectMemberRoleId = roleId
                };

                if (user == null)
                {
                    var externalAccountTypes = _accountService.GetExternalAccountTypes().Result;

                    Console.WriteLine("You are creating a new user. Please fill in the user info.");

                    newMember.FirstName = Console.GetString("First Name (Optional):");
                    newMember.LastName = Console.GetString("Last Name (Optional):");

                    var externalAccountIds = new Dictionary<string, string>();
                    foreach (var externalAccountType in externalAccountTypes)
                    {
                        var input = Console.GetString($"{externalAccountType.Label} (Optional):");
                        if (!string.IsNullOrEmpty(input))
                        {
                            externalAccountIds.Add(externalAccountType.Key, input);
                        }
                    }

                    if (externalAccountIds.Count > 0)
                        newMember.ExternalAccountIds = externalAccountIds;
                }

                var projectMember = _projectMemberService.CreateProjectMember(project.Id, newMember).Result;

                var addedMember = new MemberViewModel
                {
                    UserId = projectMember.UserId,
                    Username = projectMember.Username,
                    Role = projectMember.ProjectMemberRoleName
                };

                message = addedMember.ToCliString($"User has been added to project {Project}:");
                Logger.LogInformation(message);
            }
            else
            {
                message = $"Project {Project} was not found";
            }

            return message;
        }

        public class MemberViewModel
        {
            public int UserId { get; set; }
            public string Username { get; set; }
            public string Role { get; set; }
        }
    }
}
