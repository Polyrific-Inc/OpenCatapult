// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Model
{
    [Command(Description = "Add a project data model")]
    public class AddCommand : BaseCommand
    {
        private readonly IProjectService _projectService;
        private readonly IProjectDataModelService _projectDataModelService;

        public AddCommand(IConsole console, ILogger<AddCommand> logger,
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

        [Option("-d|--description <DESCRIPTION>", "Description of the data model", CommandOptionType.SingleValue)]
        public string Description { get; set; }

        [Option("-l|--label <LABEL>", "Label of the data model", CommandOptionType.SingleValue)]
        public string Label { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to add new data model to project {Project}...");

            string message;

            var project = _projectService.GetProjectByName(Project).Result;
            if (project != null)
            {
                var model = _projectDataModelService.CreateProjectDataModel(project.Id, new CreateProjectDataModelDto
                {
                    Name = Name,
                    Description = Description,
                    Label = Label
                }).Result;

                message = model.ToCliString($"Model has been added:", null, 1);
                Logger.LogInformation(message);
            }
            else
            { 
                message = $"Project {Project} was not found";
            }

            return message;
        }
    }
}
