// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.JobQueue;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Queue
{
    [Command(Description = "Get a job queue")]
    public class GetCommand : BaseCommand
    {
        private readonly IProjectService _projectService;
        private readonly IJobQueueService _jobQueueService;

        public GetCommand(IProjectService projectService, IJobQueueService jobQueueService, IConsole console, ILogger<GetCommand> logger) : base(console, logger)
        {
            _projectService = projectService;
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
            Console.WriteLine($"Trying to get queue \"{Number}\" in project {Project}...");

            var message = "";

            var code = "";
            if (!int.TryParse(Number, out var id))
                code = Number;

            JobDto queue = null;

            var project = _projectService.GetProjectByName(Project).Result;
            if (project != null)
            {
                queue = !string.IsNullOrEmpty(code) ? _jobQueueService.GetJobQueue(project.Id, code).Result : _jobQueueService.GetJobQueue(project.Id, id).Result;
                if (queue != null)
                {
                    message = queue.ToCliString($"Job Queue {Number}", excludedFields: new string[]
                    {
                        "ProjectId",
                        "JobDefinitionId",
                        "JobTasksStatus"
                    });

                    if (queue.JobTasksStatus?.Count > 0)
                    {
                        var sb = new System.Text.StringBuilder();
                        sb.AppendLine();
                        sb.AppendLine("  ------------------------------------------------------------------");
                        sb.AppendLine("  Job Task Status:");

                        foreach (var taskStatus in queue.JobTasksStatus.OrderBy(t => t.Sequence))
                        {
                            sb.AppendLine();
                            sb.AppendLine($"    {taskStatus.Sequence}. Task Name: {taskStatus.TaskName}");
                            sb.AppendLine($"       Status: {taskStatus.Status}");
                            sb.AppendLine($"       Remarks: {taskStatus.Remarks}");
                        }

                        sb.AppendLine();
                        sb.AppendLine("  ------------------------------------------------------------------");

                        message += sb.ToString();
                    }
                }
            }

            if (queue == null)
                message = $"Failed getting queue {Number}. Make sure the project name and queue number are correct.";

            return message;
        }
    }
}
