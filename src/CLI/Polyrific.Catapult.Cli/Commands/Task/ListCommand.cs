// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Service;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Polyrific.Catapult.Cli.Commands.Task
{
    [Command(Description = "List job task definitions")]
    public class ListCommand : BaseCommand
    {
        private readonly IProjectService _projectService;
        private readonly IJobDefinitionService _jobDefinitionService;
        private readonly IPluginService _pluginService;

        public ListCommand(IConsole console, ILogger<ListCommand> logger,
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
                    var tasks = _jobDefinitionService.GetJobTaskDefinitions(project.Id, job.Id).Result;

                    var secretConfig = new List<string>();
                    foreach (var task in tasks)
                    {
                        var configs = _pluginService.GetPluginAdditionalConfigByPluginName(task.Provider).Result;
                        secretConfig.AddRange(configs.Where(c => c.IsSecret).Select(c => c.Name));
                    }

                    message = tasks.ToListCliString($"Job task definitions in job {Job}:", secretConfig.ToArray());
                    return message;
                }
            }

            message = $"Failed fetching tasks. Make sure the project and job names are correct.";

            return message;
        }
    }
}
