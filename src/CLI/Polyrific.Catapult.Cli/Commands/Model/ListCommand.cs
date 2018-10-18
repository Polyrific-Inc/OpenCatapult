// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Model
{
    [Command(Description = "List data models in a project")]
    public class ListCommand : BaseCommand
    {
        private readonly IProjectService _projectService;
        private readonly IProjectDataModelService _projectDataModelService;

        public ListCommand(IConsole console, ILogger<ListCommand> logger, 
            IProjectService projectService, IProjectDataModelService projectDataModelService) : base(console, logger)
        {
            _projectService = projectService;
            _projectDataModelService = projectDataModelService;
        }

        [Required]
        [Option("-p|--project <PROJECT>", "Name of the project", CommandOptionType.SingleValue)]
        public string Project { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to get data models for project {Project}...");

            string message;

            var project = _projectService.GetProjectByName(Project).Result;
            if (project != null)
            {
                var models = _projectDataModelService.GetProjectDataModels(project.Id).Result;
                
                message = models.ToListCliString($"Found {models.Count} data model(s):");
            }
            else
            {
                message = $"Project {Project} was not found";
            }

            return message;
        }
    }
}
