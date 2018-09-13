// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.JobQueue;
using Polyrific.Catapult.Shared.Service;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Polyrific.Catapult.Cli.Commands.Queue
{
    [Command(Description = "Add job to queue")]
    public class AddCommand : BaseCommand
    {
        private readonly IProjectService _projectService;
        private readonly IJobDefinitionService _jobDefinitionService;
        private readonly IJobQueueService _jobQueueService;

        public AddCommand(IConsole console, ILogger<AddCommand> logger, IProjectService projectService, IJobDefinitionService jobDefinitionService, IJobQueueService jobQueueService) : base(console, logger)
        {
            _projectService = projectService;
            _jobDefinitionService = jobDefinitionService;
            _jobQueueService = jobQueueService;
        }

        [Required]
        [Option("-p|--project <PROJECT>", "Name of the project", CommandOptionType.SingleValue)]
        public string Project { get; set; }

        [Required]
        [Option("-j|--job <JOB>", "Name of the job definition", CommandOptionType.SingleValue)]
        public string Job { get; set; }

        public override string Execute()
        {
            string message = string.Empty;

            var project = _projectService.GetProjectByName(Project).Result;

            if (project != null)
            {
                var job = _jobDefinitionService.GetJobDefinitionByName(project.Id, Job).Result;

                if (job != null)
                {
                    var queue = _jobQueueService.CreateJobQueue(project.Id, new NewJobDto
                    {
                        ProjectId = project.Id,
                        JobDefinitionId = job.Id,
                        OriginUrl = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Last(a => a.AddressFamily == AddressFamily.InterNetwork).ToString()
                    }).Result;

                    message = queue.ToCliString($"Job {Job} queued:");
                    Logger.LogInformation(message);
                    return message;
                }
            }

            message = $"Failed to add queue. Make sure the project and job definition names are correct.";

            return message;
        }
    }
}