// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;
using Polyrific.Catapult.Shared.Service;
using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Cli.Commands.Property
{
    [Command(Description = "Update a project data model property")]
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
        [Option("-m|--model <MODEL>", "Name of the data model", CommandOptionType.SingleValue)]
        public string Model { get; set; }

        [Required]
        [Option("-n|--name <NAME>", "Name of the data model property", CommandOptionType.SingleValue)]
        public string Name { get; set; }

        [Option("-rn|--rename <RENAME>", "New name of the data model property", CommandOptionType.SingleValue)]
        public string Rename { get; set; }

        [Option("-l|--label <LABEL>", "Label of the data model property", CommandOptionType.SingleValue)]
        public string Label { get; set; }

        [Option("-t|--type <TYPE>", "Data type", CommandOptionType.SingleValue)]
        [AllowedValues(PropertyDataType.String, PropertyDataType.Integer, PropertyDataType.Short, PropertyDataType.Float, PropertyDataType.Double, PropertyDataType.Decimal,
            PropertyDataType.Boolean, PropertyDataType.DateTime, PropertyDataType.Byte, PropertyDataType.Guid, PropertyDataType.DbGeography)]
        public string Type { get; set; }

        [Option("-ct|--controltype <CONTROLTYPE>", "Control type", CommandOptionType.SingleValue)]
        [AllowedValues(PropertyControlType.InputText, PropertyControlType.InputNumber, PropertyControlType.InputFile, PropertyControlType.Textarea, PropertyControlType.Checkbox,
            PropertyControlType.Select, PropertyControlType.Radio, PropertyControlType.Calendar, PropertyControlType.Image)]
        public string ControlType { get; set; }

        [Option("-r|--relational <RELATIONAL>", "Relational property", CommandOptionType.SingleValue)]
        public string Relational { get; set; }

        [Option("-rt|--relationaltype <RELATIONALTYPE>", "Relationship type", CommandOptionType.SingleValue)]
        [AllowedValues(PropertyRelationalType.OneToOne, PropertyRelationalType.OneToMany, PropertyRelationalType.ManyToMany)]
        public string RelationalType { get; set; }

        [Option("-req|--required <REQUIRED>", "Is the property required?", CommandOptionType.SingleValue)]
        public bool? Required { get; set; }

        public override string Execute()
        {
            string message = string.Empty;

            var project = _projectService.GetProjectByName(Project).Result;

            if (project != null)
            {
                var model = _projectDataModelService.GetProjectDataModelByName(project.Id, Model).Result;

                if (model != null)
                {
                    var property = _projectDataModelService.GetProjectDataModelPropertyByName(project.Id, model.Id, Name).Result;

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
                            message = $"Related model {Relational} is not found";
                            Logger.LogInformation(message);
                            return message;
                        }
                    }

                    if (property != null)
                    {
                        _projectDataModelService.UpdateProjectDataModelProperty(project.Id, model.Id, property.Id, new UpdateProjectDataModelPropertyDto
                        {
                            Id = property.Id,
                            Name = Rename ?? property.Name,
                            Label = Label ?? property.Label,
                            DataType = Type ?? property.DataType,
                            IsRequired = Required ?? property.IsRequired,
                            ControlType = ControlType ?? property.ControlType,
                            RelatedProjectDataModelId = relatedModelId ?? property.RelatedProjectDataModelId,
                            RelationalType = relationalType ?? property.RelationalType
                        }).Wait();

                        message = $"Property {Name} was updated";
                        return message;
                    }
                }
            }

            message = $"Failed updating property {Name}. Make sure the project, model, and property names are correct.";

            return message;
        }
    }
}