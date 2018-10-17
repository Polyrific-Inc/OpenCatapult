// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Service;

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
            Console.WriteLine($"Trying to export project {Name}...");

            string message;

            var project = _projectService.GetProjectByName(Name).Result;
            if (project != null)
            {
                var projectYaml = _projectService.ExportProject(project.Id).Result;

                var filePath = string.IsNullOrEmpty(Output) ? Name : Output;

                message = $"Project has been exported to {_templateWriter.Write(filePath, projectYaml)}";
            }
            else
            {
                message = $"Project {Name} was not found";
            }

            return message;
        }
    }
}
