// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.JobDefinition;
using Polyrific.Catapult.Shared.Service;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Polyrific.Catapult.Cli.Commands.Task
{
    [Command(Description = "Update a job task definition")]
    public class UpdateCommand : BaseCommand
    {
        private readonly IProjectService _projectService;
        private readonly IJobDefinitionService _jobDefinitionService;

        public UpdateCommand(IConsole console, ILogger<UpdateCommand> logger,
            IProjectService projectService, IJobDefinitionService jobDefinitionService) : base(console, logger)
        {
            _projectService = projectService;
            _jobDefinitionService = jobDefinitionService;
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

        [Option("-rn|--rename <RENAME>", "New name of the job task definition", CommandOptionType.SingleValue)]
        public string Rename { get; set; }

        [Option("-t|--type <TYPE>", "Type of the task", CommandOptionType.SingleValue)]
        [AllowedValues(JobTaskDefinitionType.Generate, JobTaskDefinitionType.Push, JobTaskDefinitionType.Build,
            JobTaskDefinitionType.Deploy, JobTaskDefinitionType.DeployDb, IgnoreCase = true)]
        public string Type { get; set; }

        [Option("-prov|--Provider <PROVIDER>", "Provider of the job task definition", CommandOptionType.SingleValue)]
        public string Provider { get; set; }

        [Option("-prop|--property <KEY>:<PROPERTY>", "Property of the task", CommandOptionType.MultipleValue)]
        public (string, string)[] Property { get; set; }

        [Option("-s|--Sequence <SEQUENCE>", "Sequence order of the job task definition", CommandOptionType.SingleValue)]
        public int? Sequence { get; set; }

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
                        _jobDefinitionService.UpdateJobTaskDefinition(project.Id, job.Id, task.Id, new UpdateJobTaskDefinitionDto
                        {
                            Id = task.Id,
                            Type = Type ?? task.Type,
                            Provider = Provider ?? task.Provider,
                            Name = Rename ?? task.Name,
                            Sequence = Sequence ?? task.Sequence,
                            Configs = Property?.Length > 0 ? Property.ToDictionary(x => x.Item1, x => x.Item2) : task.Configs
                        });

                        message = $"Task {Name} was updated";
                        Logger.LogInformation(message);
                        return message;
                    }
                }

            }

            message = $"Failed updating task {Name}. Make sure the project and job definition names are correct.";

            return message;
        }
    }
}
