// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.JobQueue;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Queue
{
    [Command("restart", Description = "Restart a pending queued job")]
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
        public string Number { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to restart queue \"{Number}\" in project {Project}...");

            string message;

            var code = "";
            if (!int.TryParse(Number, out var id))
                code = Number;

            var project = _projectService.GetProjectByName(Project).Result;

            if (project != null)
            {
                var queue = !string.IsNullOrEmpty(code) ? _jobQueueService.GetJobQueue(project.Id, code).Result : _jobQueueService.GetJobQueue(project.Id, id).Result;

                if (queue != null)
                {
                    _jobQueueService.UpdateJobQueue(queue.Id, new UpdateJobDto
                    {
                        Id = queue.Id,
                        Status = JobStatus.Queued,
                        CatapultEngineId = queue.CatapultEngineId,
                        CatapultEngineIPAddress = queue.CatapultEngineIPAddress,
                        CatapultEngineMachineName = queue.CatapultEngineMachineName,
                        CatapultEngineVersion = queue.CatapultEngineVersion,
                        JobTasksStatus = queue.JobTasksStatus,
                        JobType = queue.JobType,
                        OutputValues = queue.OutputValues,
                        Remarks = null
                    }).Wait();

                    message = $"Queue {Number} has been restarted successfully";
                    Logger.LogInformation(message);
                    return message;
                }
            }

            message = $"Failed to restart queue {Number}. Make sure the project name and queue number are correct.";

            return message;
        }
    }
}
