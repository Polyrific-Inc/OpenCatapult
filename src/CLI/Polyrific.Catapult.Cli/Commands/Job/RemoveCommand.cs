// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Job
{
    [Command(Description = "Remove a job definition")]
    public class RemoveCommand : BaseCommand
    {
        private readonly IProjectService _projectService;
        private readonly IJobDefinitionService _jobDefinitionService;

        public RemoveCommand(IConsole console, ILogger<RemoveCommand> logger,
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

        [Option("-ac|--autoconfirm", "Execute the command without the need of confirmation prompt", CommandOptionType.NoValue)]
        public bool AutoConfirm { get; set; }

        public override string Execute()
        {
            if (!(AutoConfirm || Console.GetYesNo($"Are you sure you want to remove job {Name} from project {Project}?", false)))
                return string.Empty;

            Console.WriteLine($"Trying to remove job definition \"{Name}\" from project {Project}...");

            string message;

            var project = _projectService.GetProjectByName(Project).Result;

            if (project != null)
            {
                var job = _jobDefinitionService.GetJobDefinitionByName(project.Id, Name).Result;

                if (job != null)
                {
                    _jobDefinitionService.DeleteJobDefinition(project.Id, job.Id).Wait();

                    message = $"Job definition {Name} has been removed successfully";
                    Logger.LogInformation(message);
                    return message;
                }
            }

            message = $"Failed to remove job definition {Name}. Make sure the project and job definition names are correct.";

            return message;
        }
    }
}
