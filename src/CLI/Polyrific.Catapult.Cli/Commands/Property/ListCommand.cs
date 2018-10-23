// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Property
{
    [Command(Description = "List properties of a data model")]
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

        [Required]
        [Option("-m|--model <MODEL>", "Name of the data model", CommandOptionType.SingleValue)]
        public string Model { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to get properties of model {Model}...");
            string message;

            var project = _projectService.GetProjectByName(Project).Result;

            if (project != null)
            {
                var model = _projectDataModelService.GetProjectDataModelByName(project.Id, Model).Result;

                if (model != null)
                {
                    var properties = _projectDataModelService.GetProjectDataModelProperties(project.Id, model.Id).Result;

                    message = properties.ToListCliString(properties.Count == 1 ? 
                        "Found 1 property:" : $"Found {properties.Count} properties:");
                    return message;
                }
            }

            message = $"Failed to fetch properties. Make sure the project and model names are correct.";

            return message;
        }
    }
}
