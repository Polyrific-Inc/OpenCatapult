// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Job
{
    [Command("setdefault", Description = "set a job definition as default")]
    public class SetDefaultCommand : BaseCommand
    {
        private readonly IProjectService _projectService;
        private readonly IJobDefinitionService _jobDefinitionService;

        public SetDefaultCommand(IConsole console, ILogger<SetDefaultCommand> logger,
            IProjectService projectService, IJobDefinitionService jobDefinitionService) : base(console, logger)
        {
            _projectService = projectService;
            _jobDefinitionService = jobDefinitionService;
        }

        [Required]
        [Option("-p|--project <PROJECT>", "Name of the project", CommandOptionType.SingleValue)]
        public string Project { get; set; }

        [Required]
        [Option("-n|--name <NAME>", "Name of the job definition", CommandOptionType.SingleValue)]
        public string Name { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to set job definition \"{Name}\" from project {Project} as default...");

            string message;

            var project = _projectService.GetProjectByName(Project).Result;

            if (project != null)
            {
                var job = _jobDefinitionService.GetJobDefinitionByName(project.Id, Name).Result;

                if (job != null)
                {
                    _jobDefinitionService.SetJobDefinitionAsDefault(project.Id, job.Id).Wait();

                    message = $"Job definition {Name} has been set to default";
                    Logger.LogInformation(message);
                    return message;
                }
            }

            message = $"Failed to set job definition {Name} as default. Make sure the project and job definition names are correct.";

            return message;
        }
    }
}
