// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Queue
{
    [Command(Description = "List queued jobs")]
    public class ListCommand : BaseCommand
    {
        private readonly IProjectService _projectService;
        private readonly IJobQueueService _jobQueueService;

        public ListCommand(IConsole console, ILogger<ListCommand> logger, IProjectService projectService, IJobQueueService jobQueueService) : base(console, logger)
        {
            _projectService = projectService;
            _jobQueueService = jobQueueService;
        }

        [Required]
        [Option("-p|--project <PROJECT>", "Name of the project", CommandOptionType.SingleValue)]
        public string Project { get; set; }

        [Option("-s|--status <STATUS>", "Filter the queued jobs by their status", CommandOptionType.SingleValue)]
        [AllowedValues(JobQueueFilterType.All, JobQueueFilterType.Current, JobQueueFilterType.Succeeded, JobQueueFilterType.Failed, IgnoreCase = true)]
        public string Status { get; set; } = JobQueueFilterType.All;

        public override string Execute()
        {
            Console.WriteLine($"Trying to get queues of project {Project}...");

            string message;

            var project = _projectService.GetProjectByName(Project).Result;

            if (project != null)
            {
                var queueList = _jobQueueService.GetJobQueues(project.Id, Status).Result;

                message = queueList.ToListCliString($"Found {queueList.Count} queue(s):", excludedFields: new string[]
                    {
                        "ProjectId",
                        "JobDefinitionId",
                        "JobTasksStatus",
                        "OutputValues"
                    });
                return message;
            }
            else
            {
                message = $"Project {Project} was not found.";
            }

            return message;
        }
    }
}
