// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.JobQueue;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Queue
{
    [Command(Description = "Cancelling a processing job")]
    public class CancelCommand : BaseCommand
    {
        private readonly IProjectService _projectService;
        private readonly IJobDefinitionService _jobDefinitionService;
        private readonly IJobQueueService _jobQueueService;

        private readonly string[] CancellablleJobStatus = new string[] { JobStatus.Processing, JobStatus.Pending };

        public CancelCommand(IConsole console, ILogger<CancelCommand> logger, IProjectService projectService, IJobDefinitionService jobDefinitionService, IJobQueueService jobQueueService) : base(console, logger)
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

        [Option("-r|--reason <REASON>", "Cancellation Reason", CommandOptionType.SingleValue)]
        public string Reason { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to cancel queue \"{Number}\" in project {Project}...");

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
                    if (!CancellablleJobStatus.Contains(queue.Status))
                    {
                        message = $"Cannot cancel queue {Number} with status {queue.Status}";
                        return message;
                    }

                    if (queue.Status == JobStatus.Processing && !string.IsNullOrEmpty(queue.CatapultEngineId))
                        if (!Console.GetYesNo($"The job queue {Number} was being processed by engine {queue.CatapultEngineId}. Make sure the engine process has stopped. Continue the job cancellation?", false))
                            return string.Empty;

                    _jobQueueService.UpdateJobQueue(queue.Id, new UpdateJobDto
                    {
                        Id = queue.Id,
                        Status = JobStatus.Cancelled,
                        CatapultEngineId = queue.CatapultEngineId,
                        CatapultEngineIPAddress = queue.CatapultEngineIPAddress,
                        CatapultEngineMachineName = queue.CatapultEngineMachineName,
                        CatapultEngineVersion = queue.CatapultEngineVersion,
                        JobTasksStatus = queue.JobTasksStatus,
                        JobType = queue.JobType,
                        OutputValues = queue.OutputValues,
                        Remarks = Reason ?? queue.Remarks
                    }).Wait();

                    message = $"Queue {Number} has been cancelled successfully";
                    Logger.LogInformation(message);
                    return message;
                }
            }

            message = $"Failed to cancel queue {Number}. Make sure the project name and queue number are correct.";

            return message;
        }
    }
}
