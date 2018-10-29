// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Job
{
    [Command(Description = "Get a single job definition record")]
    public class GetCommand : BaseCommand
    {
        private readonly IProjectService _projectService;
        private readonly IJobDefinitionService _jobDefinitionService;
        private readonly IPluginService _pluginService;

        public GetCommand(IConsole console, ILogger<GetCommand> logger,
            IProjectService projectService, IJobDefinitionService jobDefinitionService, IPluginService pluginService) : base(console, logger)
        {
            _projectService = projectService;
            _jobDefinitionService = jobDefinitionService;
            _pluginService = pluginService;
        }

        [Required]
        [Option("-p|--project <PROJECT>", "Name of the project", CommandOptionType.SingleValue)]
        public string Project { get; set; }

        [Required]
        [Option("-n|--name <NAME>", "Name of the job definition", CommandOptionType.SingleValue)]
        public string Name { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to get job definition {Name} in project {Project}...");

            string message;

            var project = _projectService.GetProjectByName(Project).Result;

            if (project != null)
            {
                var job = _jobDefinitionService.GetJobDefinitionByName(project.Id, Name).Result;

                if (job != null)
                {
                    var secretConfig = new List<string>();

                    if (job.Tasks?.Count > 0)
                    {
                        foreach (var task in job.Tasks)
                        {
                            var configs = _pluginService.GetPluginAdditionalConfigByPluginName(task.Provider).Result;
                            secretConfig.AddRange(configs.Where(c => c.IsSecret).Select(c => c.Name));
                        }
                    }

                    message = job.ToCliString($"Job definition {Name}", secretConfig.ToArray(), excludedFields: new string[]
                        {
                            "ProjectId",
                            "JobDefinitionId"
                        });
                    return message;
                }
            }

            message = $"Failed to get job definition {Name}. Make sure the project and job definition names are correct.";

            return message;
        }
    }
}
