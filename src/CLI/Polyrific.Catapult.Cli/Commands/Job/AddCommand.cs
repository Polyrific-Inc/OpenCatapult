// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.JobDefinition;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Job
{
    [Command(Description = "Add a new job definition")]
    public class AddCommand : BaseCommand
    {
        private readonly IProjectService _projectService;
        private readonly IJobDefinitionService _jobDefinitionService;

        public AddCommand(IConsole console, ILogger<AddCommand> logger,
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
            Console.WriteLine($"Trying to add new job definition to project {Project}...");

            string message;

            var project = _projectService.GetProjectByName(Project).Result;

            if (project != null)
            {
                var job = _jobDefinitionService.CreateJobDefinition(project.Id, new CreateJobDefinitionDto
                {
                    Name = Name
                }).Result;

                message = job.ToCliString($"Job definition has been added:", excludedFields: new string[] {
                    "ProjectId"
                });
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
