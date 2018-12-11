// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Model
{
    [Command(Description = "Update project data model")]
    public class UpdateCommand : BaseCommand
    {
        private readonly IProjectService _projectService;
        private readonly IProjectDataModelService _projectDataModelService;

        public UpdateCommand(IConsole console, ILogger<UpdateCommand> logger,
            IProjectService projectService, IProjectDataModelService projectDataModelService) : base(console, logger)
        {
            _projectService = projectService;
            _projectDataModelService = projectDataModelService;
        }

        [Required]
        [Option("-p|--project <PROJECT>", "Name of the project", CommandOptionType.SingleValue)]
        public string Project { get; set; }

        [Required]
        [Option("-n|--name <NAME>", "Name of the data model", CommandOptionType.SingleValue)]
        public string Name { get; set; }

        [Option("-r|--rename <RENAME>", "New name of the data model", CommandOptionType.SingleValue)]
        public string Rename { get; set; }

        [Option("-d|--description <DESCRIPTION>", "Description of the data model", CommandOptionType.SingleValue)]
        public string Description { get; set; }

        [Option("-l|--label <LABEL>", "Label of the data model", CommandOptionType.SingleValue)]
        public string Label { get; set; }

        [Option("-mg|--managed", "Is the model managed in the UI?", CommandOptionType.SingleOrNoValue)]
        public bool? Managed { get; set; }

        [Option("-s|--selectkey <SELECTKEY>", "Select key of the model", CommandOptionType.SingleValue)]
        public string SelectKey { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to update model \"{Name}\"...");

            string message;

            var project = _projectService.GetProjectByName(Project).Result;
            if (project != null)
            {
                var model = _projectDataModelService.GetProjectDataModelByName(project.Id, Name).Result;
                if (model != null)
                {
                    _projectDataModelService.UpdateProjectDataModel(project.Id, model.Id, new UpdateProjectDataModelDto
                    {
                        Id = model.Id,
                        Name = Rename ?? Name,
                        Description = Description ?? model.Description,
                        Label = Label ?? model.Label,
                        IsManaged = Managed ?? model.IsManaged,
                        SelectKey = SelectKey ?? model.SelectKey
                    }).Wait();

                    message = $"Model {Name} has been updated successfully";
                    Logger.LogInformation(message);
                    return message;
                }
            }

            message = $"Failed to update model {Name}. Make sure the project and model names are correct.";

            return message;
        }
    }
}
