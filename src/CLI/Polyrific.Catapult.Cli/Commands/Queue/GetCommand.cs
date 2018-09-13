// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Service;
using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Cli.Commands.Queue
{
    [Command(Description = "Get complete logs of a queued job")]
    public class GetCommand : BaseCommand
    {
        private readonly IProjectService _projectService;
        private readonly IJobQueueService _jobQueueService;
        private readonly IJobQueueLogListener _jobQueueLogListener;

        public GetCommand(IConsole console, ILogger<GetCommand> logger, IProjectService projectService, IJobQueueService jobQueueService, IJobQueueLogListener jobQueueLogListener) : base(console, logger)
        {
            _projectService = projectService;
            _jobQueueService = jobQueueService;
            _jobQueueLogListener = jobQueueLogListener;
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
                    switch (queue.Status)
                    {
                        case JobStatus.Processing:
                            _jobQueueLogListener.Listen(queue.Id, OnLogReceived, OnLogError).Wait();
                            break;
                        case JobStatus.Completed:
                        case JobStatus.Error:
                        case JobStatus.Pending:
                        case JobStatus.Cancelled:
                            message = _jobQueueService.GetJobLogs(project.Id, queue.Id).Result;
                            break;
                        case JobStatus.Queued:
                            message = $"Queue {Number} is queued";
                            break;
                        default:
                            message = $"Queue {Number} has unknown status";
                            return message;
                    }

                    return message;
                }
            }

            message = $"Failed getting queue {Number}. Make sure the project name and queue number are correct.";

            return message;
        }
        
        private void OnLogReceived(string logMessage)
        {
            Console.WriteLine(logMessage);
        }

        private void OnLogError(string errorMessage)
        {
            Console.WriteLine(errorMessage);
        }
    }
}