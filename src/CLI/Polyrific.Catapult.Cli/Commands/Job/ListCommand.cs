// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Job
{
    [Command(Description = "List project definitions")]
    public class ListCommand : BaseCommand
    {
        private readonly IProjectService _projectService;
        private readonly IJobDefinitionService _jobDefinitionService;

        public ListCommand(IConsole console, ILogger<ListCommand> logger,
            IProjectService projectService, IJobDefinitionService jobDefinitionService) : base(console, logger)
        {
            _projectService = projectService;
            _jobDefinitionService = jobDefinitionService;
        }

        [Required]
        [Option("-p|--project <PROJECT>", "Name of the project", CommandOptionType.SingleValue)]
        public string Project { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to get job definitions for project {Project}...");

            string message;

            var project = _projectService.GetProjectByName(Project).Result;

            if (project != null)
            {
                var jobs = _jobDefinitionService.GetJobDefinitions(project.Id).Result;
                message = jobs.ToListCliString($"Found {jobs.Count} job definition(s):");
            }
            else
            {
                message = $"Project {Project} was not found";
            }

            return message;
        }
    }
}
