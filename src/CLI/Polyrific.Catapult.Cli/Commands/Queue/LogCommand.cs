// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Queue
{
    [Command(Description = "Get complete logs of a queued job")]
    public class LogCommand : BaseCommand
    {
        private readonly IProjectService _projectService;
        private readonly IJobQueueService _jobQueueService;
        private readonly IJobQueueLogListener _jobQueueLogListener;

        public LogCommand(IConsole console, ILogger<LogCommand> logger, IProjectService projectService, IJobQueueService jobQueueService, IJobQueueLogListener jobQueueLogListener) : base(console, logger)
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
        public string Number { get; set; }

        [Option("-w|--wait", "Wait for the log stream to coming through", CommandOptionType.NoValue)]
        public bool Wait { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to get queue \"{Number}\" in project {Project}...");

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
                    switch (queue.Status)
                    {
                        case JobStatus.Processing:
                            _jobQueueLogListener.Listen(queue.Id, OnLogReceived, OnLogError).Wait();
                            message = "";
                            return message;
                        case JobStatus.Completed:
                        case JobStatus.Error:
                        case JobStatus.Pending:
                        case JobStatus.Cancelled:
                            message = _jobQueueService.GetJobLogs(project.Id, queue.Id).Result;
                            return message;
                        case JobStatus.Queued:
                            if (Wait)
                            {
                                Console.WriteLine("Waiting for log stream...");
                                _jobQueueLogListener.Listen(queue.Id, OnLogReceived, OnLogError).Wait();
                                message = "";
                            }
                            else
                            {
                                message = $"Queue {Number} is queued";
                            }
                            
                            return message;
                        default:
                            message = $"Queue {Number} has unknown status";
                            return message;
                    }
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
