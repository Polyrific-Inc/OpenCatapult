// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Service;
using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Cli.Commands.Project
{
    [Command(Description = "Export project into a yaml file")]
    public class ExportCommand : BaseCommand
    {
        private readonly IProjectService _projectService;
        private readonly ITemplateWriter _templateWriter;

        public ExportCommand(IConsole console, ILogger<ExportCommand> logger, IProjectService projectService, ITemplateWriter templateWriter) : base(console, logger)
        {
            _projectService = projectService;
            _templateWriter = templateWriter;
        }

        [Required]
        [Option("-n|--name <NAME>", "Name of the project", CommandOptionType.SingleValue)]
        public string Name { get; set; }

        [Option("-o|--Output <NAME>", "Output file location", CommandOptionType.SingleValue)]
        public string Output { get; set; }

        public override string Execute()
        {
            string message = string.Empty;

            var project = _projectService.GetProjectByName(Name).Result;

            if (project != null)
            {
                var projectYaml = _projectService.ExportProject(project.Id).Result;

                string filePath;
                if (string.IsNullOrEmpty(Output))
                {
                    filePath = Name;
                }
                else
                {
                    filePath = Output;
                }

                message = $"Project exported to {_templateWriter.Write(filePath, projectYaml)}";
            }
            else
            {
                message = $"Project {Name} is not found";
            }

            return message;
        }
    }
}
