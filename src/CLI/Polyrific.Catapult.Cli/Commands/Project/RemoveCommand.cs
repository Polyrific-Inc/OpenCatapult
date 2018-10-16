﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Service;
using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Cli.Commands.Project
{
    [Command(Description = "Remove a project")]
    public class RemoveCommand : BaseCommand
    {
        private readonly IProjectService _projectService;

        public RemoveCommand(IConsole console, ILogger<RemoveCommand> logger, IProjectService projectService) : base(console, logger)
        {
            _projectService = projectService;
        }

        [Required]
        [Option("-n|--name <NAME>", "Name of the project", CommandOptionType.SingleValue)]
        public string Name { get; set; }

        [Option("-ac|--autoconfirm", "Execute the command without the need of confirmation prompt", CommandOptionType.NoValue)]
        public bool AutoConfirm { get; set; }

        public override string Execute()
        {
            if (!(AutoConfirm || Console.GetYesNo($"Are you sure you want to remove project {Name}?", false)))
                return string.Empty;

            string message = string.Empty;

            var project = _projectService.GetProjectByName(Name).Result;

            if (project != null)
            {
                _projectService.DeleteProject(project.Id).Wait();

                message = $"Project {Name} was removed";
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
