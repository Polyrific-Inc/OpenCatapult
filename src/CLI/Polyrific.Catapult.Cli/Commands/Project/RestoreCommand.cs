// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Service;
using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Cli.Commands.Project
{
    [Command(Description = "Restore an archived project")]
    public class RestoreCommand : BaseCommand
    {
        private readonly IProjectService _projectService;

        public RestoreCommand(IConsole console, ILogger<RestoreCommand> logger, IProjectService projectService) : base(console, logger)
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
                _projectService.RestoreProject(project.Id).Wait();

                message = $"Project {Name} restored";
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