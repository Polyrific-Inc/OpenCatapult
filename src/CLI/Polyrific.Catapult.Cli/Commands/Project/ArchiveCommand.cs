// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Project
{
    [Command("archive", Description = "Archive a project")]
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
            Console.WriteLine($"Trying to archive project {Name}...");

            string message;

            var project = _projectService.GetProjectByName(Name).Result;
            if (project != null)
            {
                _projectService.ArchiveProject(project.Id).Wait();

                message = $"Project {Name} has been archived successfully";
                Logger.LogInformation(message);
            }
            else
            {
                message = $"Project {Name} was not found";
            }

            return message;
        }
    }
}
