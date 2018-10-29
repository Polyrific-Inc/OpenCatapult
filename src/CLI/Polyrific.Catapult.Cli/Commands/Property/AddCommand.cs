// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Property
{
    [Command(Description = "Add a project data model property")]
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
        [Option("-m|--model <MODEL>", "Name of the data model", CommandOptionType.SingleValue)]
        public string Model { get; set; }

        [Required]
        [Option("-n|--name <NAME>", "Name of the data model property", CommandOptionType.SingleValue)]
        public string Name { get; set; }

        [Option("-l|--label <LABEL>", "Label of the data model property", CommandOptionType.SingleValue)]
        public string Label { get; set; }

        [Option("-t|--type <TYPE>", "Data type", CommandOptionType.SingleValue)]
        [AllowedValues(PropertyDataType.String, PropertyDataType.Integer, PropertyDataType.Short, PropertyDataType.Float, PropertyDataType.Double, PropertyDataType.Decimal,
            PropertyDataType.Boolean, PropertyDataType.DateTime, PropertyDataType.Byte, PropertyDataType.Guid, PropertyDataType.DbGeography)]
        public string Type { get; set; } = PropertyDataType.String;

        [Option("-ct|--controltype <CONTROLTYPE>", "Control type", CommandOptionType.SingleValue)]
        [AllowedValues(PropertyControlType.InputText, PropertyControlType.InputNumber, PropertyControlType.InputFile, PropertyControlType.Textarea, PropertyControlType.Checkbox,
            PropertyControlType.CheckboxList, PropertyControlType.Select, PropertyControlType.Radio, PropertyControlType.Calendar, PropertyControlType.Image)]
        public string ControlType { get; set; } = PropertyControlType.InputText;

        [Option("-r|--relational <RELATIONAL>", "Relational property", CommandOptionType.SingleValue)]
        public string Relational { get; set; }

        [Option("-rt|--relationaltype <RELATIONALTYPE>", "Relationship type", CommandOptionType.SingleValue)]
        [AllowedValues(PropertyRelationalType.OneToOne, PropertyRelationalType.OneToMany, PropertyRelationalType.ManyToMany)]
        public string RelationalType { get; set; } = PropertyRelationalType.OneToOne;

        [Option("-req|--required", "Is the property required?", CommandOptionType.NoValue)]
        public bool Required { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to add property \"{Name}\" to model {Model}...");
            string message;

            var project = _projectService.GetProjectByName(Project).Result;

            if (project != null)
            {
                var model = _projectDataModelService.GetProjectDataModelByName(project.Id, Model).Result;

                int? relatedModelId = null;
                string relationalType = null;
                if (!string.IsNullOrEmpty(Relational))
                {
                    var relatedModel = _projectDataModelService.GetProjectDataModelByName(project.Id, Relational).Result;

                    if (relatedModel != null)
                    {
                        relatedModelId = relatedModel.Id;
                        relationalType = RelationalType;
                    }
                    else
                    {
                        message = $"Related model {Relational} was not found";
                        return message;
                    }
                }

                if (model != null)
                {
                    var newProperty = _projectDataModelService.CreateProjectDataModelProperty(project.Id, model.Id,
                        new CreateProjectDataModelPropertyDto
                        {
                            Name = Name,
                            DataType = Type,
                            Label = Label,
                            IsRequired = Required,
                            ControlType = ControlType,
                            RelatedProjectDataModelId = relatedModelId,
                            RelationalType = relationalType
                        }).Result;

                    message = newProperty.ToCliString($"Property {Name} was added to model {Model}:", excludedFields: new string[]
                        {
                            "ProjectDataModelId",
                            "RelatedProjectDataModelId"
                        });
                    Logger.LogInformation(message);
                    return message;
                }
            }

            message = $"Failed to add property {Name}. Make sure the project and model names are correct.";

            return message;
        }
    }
}
