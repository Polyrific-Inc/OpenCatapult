// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.JobDefinition;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Job
{
    [Command("update", Description = "Update a job definition")]
    public class UpdateCommand : BaseCommand
    {
        private readonly IProjectService _projectService;
        private readonly IJobDefinitionService _jobDefinitionService;

        public UpdateCommand(IConsole console, ILogger<UpdateCommand> logger,
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

        [Required]
        [Option("-rn|--rename <RENAME>", "New name of the job definition", CommandOptionType.SingleValue)]
        public string Rename { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to update job definition \"{Name}\" from project {Project}...");

            string message;

            var project = _projectService.GetProjectByName(Project).Result;

            if (project != null)
            {
                var job = _jobDefinitionService.GetJobDefinitionByName(project.Id, Name).Result;

                if (job != null)
                {
                    _jobDefinitionService.UpdateJobDefinition(project.Id, job.Id, new UpdateJobDefinitionDto
                    {
                        Id = job.Id,
                        Name = Rename ??  job.Name
                    }).Wait();

                    message = $"Job definition {Name} has been updated successfully";
                    Logger.LogInformation(message);
                    return message;
                }
            }

            message = $"Failed to update job definition {Name}. Make sure the project and job definition names are correct.";

            return message;
        }
    }
}
