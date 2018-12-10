// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Project
{
    [Command(Description = "Get a single project record")]
    public class GetCommand : BaseCommand
    {
        private readonly IProjectService _projectService;

        public GetCommand(IConsole console, ILogger<GetCommand> logger, IProjectService projectService) : base(console, logger)
        {
            _projectService = projectService;
        }

        [Required]
        [Option("-n|--name <NAME>", "Name of the project", CommandOptionType.SingleValue)]
        public string Name { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to get project {Name}...");

            string message;

            var project = _projectService.GetProjectByName(Name).Result;

            if (project != null)
                message = project.ToCliString($"Project {Name}");
            else
                message = $"Project {Name} was not found";

            return message;
        }
    }
}
