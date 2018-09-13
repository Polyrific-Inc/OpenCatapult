// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Service;
using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Cli.Commands.Project
{
    [Command(Description = "Archive a project")]
    public class ArchiveCommand : BaseCommand
    {
        private readonly IProjectService _projectService;

        public ArchiveCommand(IConsole console, ILogger<ArchiveCommand> logger, IProjectService projectService) : base(console, logger)
        {
            _projectService = projectService;
        }

        [Required]
        [Option("-n|--name <NAME>", "Name of the project", CommandOptionType.SingleValue)]
        public string Name { get; set; }

        public override string Execute()
        {
            string message = string.Empty;

            var project = _projectService.GetProjectByName(Name).Result;

            if (project != null)
            {
                _projectService.ArchiveProject(project.Id).Wait();

                message = $"Project {Name} archived";
                Logger.LogInformation(message);
            }
            else
            {
                message = $"Project {Name} is not found";
            }

            return message;
        }
    }
}