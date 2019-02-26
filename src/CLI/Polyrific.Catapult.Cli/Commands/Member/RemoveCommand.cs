// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Member
{
    [Command("remove", Description = "Remove a project member")]
    public class RemoveCommand : BaseCommand
    {
        private readonly IProjectMemberService _projectMemberService;
        private readonly IProjectService _projectService;
        private readonly IAccountService _accountService;

        public RemoveCommand(IConsole console, ILogger<RemoveCommand> logger,
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

        [Option("-ac|--autoconfirm", "Execute the command without the need of confirmation prompt", CommandOptionType.NoValue)]
        public bool AutoConfirm { get; set; }

        public override string Execute()
        {
            if (!(AutoConfirm || Console.GetYesNo($"Are you sure you want to remove user {User} from project {Project}?", false)))
                return string.Empty;

            Console.WriteLine($"Trying to remove {User} from project {Project}...");

            string message = string.Empty;

            var project = _projectService.GetProjectByName(Project).Result;
            var user = _accountService.GetUserByUserName(User).Result;

            if (project != null && user != null)
            {
                var projectMember = _projectMemberService.GetProjectMemberByUserId(project.Id, int.Parse(user.Id)).Result;

                if (projectMember != null)
                {
                    _projectMemberService.RemoveProjectMember(project.Id, projectMember.Id).Wait();
                    message = $"User {User} has been removed from project {Project}";
                    Logger.LogInformation(message);
                    return message;
                }
            }
            else
            {
                message = $"Failed to remove user {User}. Make sure the project name and user email are correct.";
            }

            return message;
        }
    }
}
