// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Property
{
    [Command(Description = "Remove a project data model property")]
    public class RemoveCommand : BaseCommand
    {
        private readonly IProjectService _projectService;
        private readonly IProjectDataModelService _projectDataModelService;

        public RemoveCommand(IConsole console, ILogger<RemoveCommand> logger,
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

        [Required]
        [Option("-n|--name <NAME>", "Name of the data model property", CommandOptionType.SingleValue)]
        public string Name { get; set; }

        [Option("-ac|--autoconfirm", "Execute the command without the need of confirmation prompt", CommandOptionType.NoValue)]
        public bool AutoConfirm { get; set; }

        public override string Execute()
        {
            if (!(AutoConfirm || Console.GetYesNo($"Are you sure you want to remove property {Name} from model {Model}?", false)))
                return string.Empty;

            Console.WriteLine($"Trying to remove property \"{Name}\" from model {Model}...");

            string message;

            var project = _projectService.GetProjectByName(Project).Result;

            if (project != null)
            {
                var model = _projectDataModelService.GetProjectDataModelByName(project.Id, Model).Result;

                if (model != null)
                {
                    var property = _projectDataModelService.GetProjectDataModelPropertyByName(project.Id, model.Id, Name).Result;

                    if (property != null)
                    {
                        _projectDataModelService.DeleteProjectDataModelProperty(project.Id, model.Id, property.Id).Wait();

                        message = $"Property {Name} has been removed successfully";
                        Logger.LogInformation(message);
                        return message;
                    }
                }
            }

            message = $"Failed to remove property {Name}. Make sure the project, model, and property names are correct.";

            return message;
        }
    }
}
