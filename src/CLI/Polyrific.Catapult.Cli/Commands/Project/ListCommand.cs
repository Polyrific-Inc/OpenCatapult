// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Project
{
    [Command("list", Description = "List projects which the user has access to")]
    public class ListCommand : BaseCommand
    {
        private readonly IProjectService _projectService;

        public ListCommand(IConsole console, ILogger<ListCommand> logger, IProjectService projectService) : base(console, logger)
        {
            _projectService = projectService;
        }

        [Option("-s|--status <STATUS>", "Filter the projects by their status", CommandOptionType.SingleValue)]
        [AllowedValues(ProjectStatusFilterType.All, ProjectStatusFilterType.Active, ProjectStatusFilterType.Archived, ProjectStatusFilterType.Deleting, IgnoreCase = true)]
        public string Status { get; set; } = ProjectStatusFilterType.All;

        [Option("-a|--all", "Retrieve all projects", CommandOptionType.NoValue)]
        public bool GetAll { get; set; }

        public override string Execute()
        {
            Console.WriteLine("Trying to get list of projects...");

            var projects = _projectService.GetProjects(Status, GetAll).Result;

            return projects.ToListCliString($"Found {projects.Count} project(s):", excludedFields: new string[]
                {
                    "Config"
                });
        }
    }
}
