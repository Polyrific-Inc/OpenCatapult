// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Member
{
    [Command(Description = "List members of a project")]
    public class ListCommand : BaseCommand
    {
        private readonly IProjectMemberService _projectMemberService;
        private readonly IProjectService _projectService;

        public ListCommand(IConsole console, ILogger<ListCommand> logger, 
            IProjectMemberService projectMemberService, IProjectService projectService) : base(console, logger)
        {
            _projectMemberService = projectMemberService;
            _projectService = projectService;
        }

        [Required]
        [Option("-p|--project <PROJECT>", "Name of the project", CommandOptionType.SingleValue)]
        public string Project { get; set; }

        [Option("-r|--role <ROLE>", "Role of the project member", CommandOptionType.SingleValue)]
        [AllowedValues(MemberRole.All, MemberRole.Member, MemberRole.Contributor, MemberRole.Maintainer, MemberRole.Owner, IgnoreCase = true)]
        public string Role { get; set; } = MemberRole.All;

        public override string Execute()
        {
            Console.WriteLine($"Trying to get members of project {Project}...");

            string message;

            var project = _projectService.GetProjectByName(Project).Result;
            if (project != null)
            {
                var roleId = MemberRole.GetMemberRoleId(Role ?? string.Empty);
                var projectMembers = _projectMemberService.GetProjectMembers(project.Id, roleId).Result;

                message = projectMembers.ToListCliString($"Found {projectMembers.Count} project member(s):");
            }
            else
            {
                message = $"Project {Project} was not found";
            }

            return message;
        }
    }
}
