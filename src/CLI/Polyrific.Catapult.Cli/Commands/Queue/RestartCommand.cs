// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.JobQueue;
using Polyrific.Catapult.Shared.Service;
using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Cli.Commands.Queue
{
    [Command(Description = "Restart a pending queued job")]
    public class RestartCommand : BaseCommand
    {
        private readonly IProjectService _projectService;
        private readonly IJobDefinitionService _jobDefinitionService;
        private readonly IJobQueueService _jobQueueService;

        public RestartCommand(IConsole console, ILogger<RestartCommand> logger, IProjectService projectService, IJobDefinitionService jobDefinitionService, IJobQueueService jobQueueService) : base(console, logger)
        {
            _projectService = projectService;
            _jobDefinitionService = jobDefinitionService;
            _jobQueueService = jobQueueService;
        }

        [Required]
        [Option("-p|--project <PROJECT>", "Name of the project", CommandOptionType.SingleValue)]
        public string Project { get; set; }

        [Required]
        [Option("-n|--number <NUMBER>", "Queue number", CommandOptionType.SingleValue)]
        public int Number { get; set; }

        public override string Execute()
        {
            string message = string.Empty;
            
            var project = _projectService.GetProjectByName(Project).Result;

            if (project != null)
            {
                var queue = _jobQueueService.GetJobQueue(project.Id, Number).Result;

                if (queue != null)
                {
                    _jobQueueService.UpdateJobQueue(queue.Id, new UpdateJobDto
                    {
                        Id = Number,
                        Status = JobStatus.Queued
                    }).Wait();

                    message = $"Queue {Number} restarted";
                    Logger.LogInformation(message);
                    return message;
                }
            }

            message = $"Failed to restart queue. Make sure the project name and queue number are correct.";

            return message;
        }
    }
}