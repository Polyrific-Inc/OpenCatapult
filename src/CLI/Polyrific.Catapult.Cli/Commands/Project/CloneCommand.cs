// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.Project;
using Polyrific.Catapult.Shared.Service;
using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Cli.Commands.Project
{
    [Command(Description = "Create project by copying from existing")]
    public class CloneCommand : BaseCommand
    {
        private readonly IProjectService _projectService;

        public CloneCommand(IConsole console, ILogger<CloneCommand> logger, IProjectService projectService) : base(console, logger)
        {
            _projectService = projectService;
        }

        [Required]
        [Option("-p|--project <PROJECT>", "Source project name", CommandOptionType.SingleValue)]
        public string Project { get; set; }

        [Required]
        [Option("-n|--name <NAME>", "New project name", CommandOptionType.SingleValue)]
        public string Name { get; set; }

        [Option("-m|--includemember", "Include project members", CommandOptionType.NoValue)]
        public bool IncludeMembers { get; set; }

        [Option("-j|--includejob", "Include job definitions", CommandOptionType.NoValue)]
        public bool IncludeJobDefinitions { get; set; }

        public override string Execute()
        {
            string message = string.Empty;
            
            var project = _projectService.GetProjectByName(Project).Result;

            if (project != null)
            {
                var clonedProject = _projectService.CloneProject(project.Id, new CloneProjectOptionDto
                {
                    IncludeMembers = IncludeMembers,
                    IncludeJobDefinitions = IncludeJobDefinitions,
                    NewProjectName = Name
                }).Result;

                message = clonedProject.ToCliString("Project cloned:");
                Logger.LogInformation(message);
            }
            else
            {
                message = $"Project {Project} is not found";
            }

            return message;
        }
    }
}