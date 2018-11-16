// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Engine.Core;
using Polyrific.Catapult.Engine.Utility;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.Project;

namespace Polyrific.Catapult.Engine.Commands.Task
{
    [Command(Description = "Run a job task (intended for plugin testing only)")]
    public class RunCommand : BaseCommand
    {
        private readonly ICatapultEngine _engine;
        private readonly IProjectTemplateReader _templateReader;

        public RunCommand(ICatapultEngine engine, IProjectTemplateReader templateReader, IConsole console, ILogger<RunCommand> logger) : base(console, logger)
        {
            _engine = engine;
            _templateReader = templateReader;
        }

        [Option("-t|--type <TYPE>", "Type of the job task", CommandOptionType.SingleValue)]
        [Required]
        [AllowedValues(JobTaskDefinitionType.Build, JobTaskDefinitionType.Clone, JobTaskDefinitionType.Deploy,
            JobTaskDefinitionType.DeployDb, JobTaskDefinitionType.Generate, JobTaskDefinitionType.Merge,
            JobTaskDefinitionType.PublishArtifact, JobTaskDefinitionType.Push, JobTaskDefinitionType.Test)]
        public string Type { get; set; }

        [Option("-p|--provider <PROVIDER>", "Name of the provider", CommandOptionType.SingleValue)]
        [Required]
        public string Provider { get; set; }

        [Option("-c|--config <KEY>:<VALUE>", "Configuration items", CommandOptionType.MultipleValue)]
        public (string, string)[] Configs { get; set; }

        [Option("-pt|--projecttemplate <PROJECT_TEMPLATE>", "Location of project template", CommandOptionType.SingleValue)]
        public string ProjectTemplate { get; set; }

        public override string Execute()
        {
            var inputConfigs = Configs?.ToDictionary(c => c.Item1, c => c.Item2) ?? new Dictionary<string, string>();

            NewProjectDto project = null;
            if (!string.IsNullOrEmpty(ProjectTemplate))
                project = _templateReader.Read(ProjectTemplate);

            _engine.ExecuteTask(Type, Provider, inputConfigs, project).Wait();

            return "Completed";
        }
    }
}
