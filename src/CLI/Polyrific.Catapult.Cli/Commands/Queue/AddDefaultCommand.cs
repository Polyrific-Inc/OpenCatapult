// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.JobQueue;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Queue
{
    [Command("adddefault", Description = "Add job to queue")]
    public class AddDefaultCommand : BaseCommand
    {
        private readonly IProjectService _projectService;
        private readonly IJobQueueService _jobQueueService;

        public AddDefaultCommand(IConsole console, ILogger<AddDefaultCommand> logger, IProjectService projectService, IJobQueueService jobQueueService) : base(console, logger)
        {
            _projectService = projectService;
            _jobQueueService = jobQueueService;
        }

        [Required]
        [Option("-p|--project <PROJECT>", "Name of the project", CommandOptionType.SingleValue)]
        public string Project { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to queue default job in project {Project}...");

            string message;

            var project = _projectService.GetProjectByName(Project).Result;

            if (project != null)
            {
                var queue = _jobQueueService.CreateDefaultJobQueue(project.Id, new NewJobDto
                {
                    ProjectId = project.Id,
                    OriginUrl = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Last(a => a.AddressFamily == AddressFamily.InterNetwork).ToString()
                }).Result;

                message = queue.ToCliString($"Default job \"{queue.JobDefinitionName}\" has been queued successfully:", excludedFields: new string[]
                {
                        "ProjectId",
                        "JobDefinitionId",
                        "JobTasksStatus",
                        "OutputValues",
                        "CatapultEngineId",
                        "CatapultEngineMachineName",
                        "CatapultEngineIPAddress",
                        "CatapultEngineVersion"
                });
                Logger.LogInformation(message);

                message += "\nThe job will be picked up by a running engine shortly.";
                return message;
            }

            message = $"Project {Project} was not found";

            return message;
        }
    }
}
