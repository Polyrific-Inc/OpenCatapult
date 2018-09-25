// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Service;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Polyrific.Catapult.Cli.Commands.Task
{
    [Command(Description = "Get a single job task definition")]
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
        [Option("-j|--job <JOB>", "Name of the job definition", CommandOptionType.SingleValue)]
        public string Job { get; set; }

        [Required]
        [Option("-n|--name <NAME>", "Name of the job task definition", CommandOptionType.SingleValue)]
        public string Name { get; set; }

        [Option("-t|--type <TYPE>", "Type of the task", CommandOptionType.SingleValue)]
        [AllowedValues(JobTaskDefinitionType.Generate, JobTaskDefinitionType.Push, JobTaskDefinitionType.Build,
            JobTaskDefinitionType.Deploy, JobTaskDefinitionType.DeployDb, IgnoreCase = true)]
        public string Type { get; set; } = JobTaskDefinitionType.Generate;

        public override string Execute()
        {
            string message = string.Empty;

            var project = _projectService.GetProjectByName(Project).Result;

            if (project != null)
            {
                var job = _jobDefinitionService.GetJobDefinitionByName(project.Id, Job).Result;

                if (job != null)
                {
                    var task = _jobDefinitionService.GetJobTaskDefinitionByName(project.Id, job.Id, Name).Result;

                    if (task != null)
                    {
                        var configs = _pluginService.GetPluginAdditionalConfigByPluginName(task.Provider).Result;
                        var secretConfigs = configs.Where(c => c.IsSecret).Select(c => c.Name).ToArray();
                        message = task.ToCliString($"Task {Name} in job {Job}:", secretConfigs);
                        return message;
                    }
                }

            }

            message = $"Failed fetching task {Name}. Make sure the project, job definition, and task names are correct.";

            return message;
        }
    }
}
