// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.JobDefinition;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Task
{
    [Command(Description = "Add task to a job definition")]
    public class AddCommand : BaseCommand
    {
        private readonly IConsoleReader _consoleReader;
        private readonly IProjectService _projectService;
        private readonly IJobDefinitionService _jobDefinitionService;
        private readonly IPluginService _pluginService;
        private readonly IExternalServiceService _externalServiceService;
        private readonly IExternalServiceTypeService _externalServiceTypeService;

        public AddCommand(IConsole console, ILogger<AddCommand> logger, IConsoleReader consoleReader,
            IProjectService projectService, IJobDefinitionService jobDefinitionService, IPluginService pluginService, 
            IExternalServiceService externalServiceService, IExternalServiceTypeService externalServiceTypeService) : base(console, logger)
        {
            _consoleReader = consoleReader;
            _projectService = projectService;
            _jobDefinitionService = jobDefinitionService;
            _pluginService = pluginService;
            _externalServiceService = externalServiceService;
            _externalServiceTypeService = externalServiceTypeService;
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

        [Required]
        [Option("-prov|--provider <PROVIDER>", "Provider of the job task definition", CommandOptionType.SingleValue)]
        public string Provider { get; set; }

        [Required]
        [Option("-t|--type <TYPE>", "Type of the task", CommandOptionType.SingleValue)]
        [AllowedValues(JobTaskDefinitionType.Clone, JobTaskDefinitionType.Generate, JobTaskDefinitionType.Push, JobTaskDefinitionType.Merge, JobTaskDefinitionType.Build, 
            JobTaskDefinitionType.PublishArtifact, JobTaskDefinitionType.Deploy, JobTaskDefinitionType.DeployDb, JobTaskDefinitionType.Test, IgnoreCase = true)]
        public string Type { get; set; } = JobTaskDefinitionType.Generate;

        [Option("-prop|--property <KEY>:<PROPERTY>", "Property of the task", CommandOptionType.MultipleValue)]
        public (string, string)[] Property { get; set; }

        [Option("-s|--sequence <SEQUENCE>", "Sequence order of the job task definition", CommandOptionType.SingleValue)]
        public int? Sequence { get; set; }

        private bool _firstConfigPrompt = false;

        public override string Execute()
        {
            Console.WriteLine($"Trying to add new task to job definition {Job}...");

            string message;

            var project = _projectService.GetProjectByName(Project).Result;

            if (project != null)
            {
                var job = _jobDefinitionService.GetJobDefinitionByName(project.Id, Job).Result;

                if (job != null)
                {
                    var secretProperties = new List<string>();
                    Dictionary<string, string> additionalConfigs = null;
                    
                    var plugin = _pluginService.GetPluginByName(Provider).Result;
                    if (plugin == null)
                    {
                        message = $"The provider \"{Provider}\" is not installed";
                        return message;
                    }

                    var properties = PromptTaskConfig();

                    if (plugin.RequiredServices != null && plugin.RequiredServices.Length > 0)
                    {
                        Console.WriteLine($"The provider \"{Provider}\" requires the following service(s): {string.Join(", ", plugin.RequiredServices)}.");
                        foreach (var service in plugin.RequiredServices)
                        {
                            var externalServiceName = Console.GetString($"{service} external service name:");

                            if (string.IsNullOrEmpty(externalServiceName))
                            {
                                message = $"The {service} external service is required for the provider {Provider}. If you do not have it in the system, please add them using \"service add\" command";
                                return message;
                            }

                            var externalService = _externalServiceService.GetExternalServiceByName(externalServiceName).Result;

                            if (externalService == null)
                            {
                                message = $"The external service {externalServiceName} was not found.";
                                return message;
                            }

                            if (externalService.ExternalServiceTypeName != service)
                            {
                                message = $"The entered external service is not a {service} service";
                                return message;
                            }

                            properties.Add(($"{service}ExternalService", externalServiceName));
                        }
                    }

                    if (plugin.AdditionalConfigs != null && plugin.AdditionalConfigs.Length > 0)
                    {
                        additionalConfigs = new Dictionary<string, string>();
                        Console.WriteLine($"The provider \"{plugin.Name}\" have some additional config(s):");
                        foreach (var additionalConfig in plugin.AdditionalConfigs)
                        {
                            string input = string.Empty;
                            string prompt = $"{(!string.IsNullOrEmpty(additionalConfig.Label) ? additionalConfig.Label : additionalConfig.Name)}{(additionalConfig.IsRequired ? " (Required):" : ":")}";

                            do
                            {
                                if (additionalConfig.IsSecret)
                                    input = _consoleReader.GetPassword(prompt);
                                else
                                    input = Console.GetString(prompt);

                            } while (additionalConfig.IsRequired && string.IsNullOrEmpty(input));

                            additionalConfigs.Add(additionalConfig.Name, input);

                            if (additionalConfig.IsSecret)
                                secretProperties.Add(additionalConfig.Name);
                        }
                    }
                    
                    var task = _jobDefinitionService.CreateJobTaskDefinition(project.Id, job.Id, new CreateJobTaskDefinitionDto
                    {
                        Name = Name,
                        Provider = Provider,
                        Type = Type,
                        Sequence = Sequence,
                        Configs = properties.Count > 0 ? properties.ToDictionary(x => x.Item1, x => x.Item2) : null,
                        AdditionalConfigs = additionalConfigs
                    }).Result;

                    message = task.ToCliString($"Task has been added:", secretProperties.ToArray());
                    Logger.LogInformation(message);
                    return message;
                }
            }

            message = $"Failed to add task {Name}. Make sure the project and job definition names are correct.";

            return message;
        }

        public override string GetHelpFooter()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Available task:");
            sb.AppendLine($"  - Type: {JobTaskDefinitionType.Clone}");
            sb.AppendLine("    Properties:");
            sb.AppendLine("      - Repository");
            sb.AppendLine("      - IsPrivateRepository (\"true\" or \"false\")");
            sb.AppendLine("      - CloneLocation");
            sb.AppendLine("      - BaseBranch");

            sb.AppendLine($"  - Type: {JobTaskDefinitionType.Generate}");
            sb.AppendLine("    Properties:");
            sb.AppendLine("      - OutputLocation");

            sb.AppendLine($"  - Type: {JobTaskDefinitionType.Push}");
            sb.AppendLine("    Properties:");
            sb.AppendLine("      - SourceLocation");
            sb.AppendLine("      - Repository");
            sb.AppendLine("      - Branch");
            sb.AppendLine("      - CreatePullRequest (\"true\" or \"false\")");
            sb.AppendLine("      - PullRequestTargetBranch");
            sb.AppendLine("      - CommitMessage");
            sb.AppendLine("      - Author");
            sb.AppendLine("      - Email");

            sb.AppendLine($"  - Type: {JobTaskDefinitionType.Merge}");
            sb.AppendLine("    Properties:");
            sb.AppendLine("      - Repository");

            sb.AppendLine($"  - Type: {JobTaskDefinitionType.Build}");
            sb.AppendLine("    Properties:");
            sb.AppendLine("      - SourceLocation");
            sb.AppendLine("      - OutputArtifactLocation");

            sb.AppendLine($"  - Type: {JobTaskDefinitionType.PublishArtifact}");

            sb.AppendLine($"  - Type: {JobTaskDefinitionType.Deploy}");
            sb.AppendLine("    Properties:");
            sb.AppendLine("      - ArtifactLocation");

            sb.AppendLine($"  - Type: {JobTaskDefinitionType.DeployDb}");
            sb.AppendLine("    Properties:");
            sb.AppendLine("      - MigrationLocation");

            sb.AppendLine($"  - Type: {JobTaskDefinitionType.Test}");
            sb.AppendLine("    Properties:");
            sb.AppendLine("      - TestLocation");
            sb.AppendLine("      - ContinueWhenFailed (\"true\" or \"false\")");

            sb.AppendLine();
            sb.AppendLine("Available task providers:");
            var providers = _pluginService.GetPlugins().Result;
            foreach (var provider in providers)
                sb.AppendLine($"  - Provider: {provider.Name}");

            return sb.ToString();
        }

        private List<(string, string)> PromptTaskConfig()
        {
            var taskConfigs = Property?.ToList() ?? new List<(string, string)>();

            var typeCapitalized = Type.First().ToString().ToUpper() + Type.Substring(1).ToLower();

            if (Type.ToLower() == JobTaskDefinitionType.Clone.ToLower())
            {
                PromptIfNotSet(taskConfigs, "Repository", true);
                PromptIfNotSet(taskConfigs, "IsPrivateRepository", true, new string[] { "true", "false" });
                PromptIfNotSet(taskConfigs, "CloneLocation");
                PromptIfNotSet(taskConfigs, "BaseBranch");
            }
            else if (Type.ToLower() == JobTaskDefinitionType.Generate.ToLower())
            {
                PromptIfNotSet(taskConfigs, "OutputLocation");
            }
            else if (Type.ToLower() == JobTaskDefinitionType.Push.ToLower())
            {
                PromptIfNotSet(taskConfigs, "Repository", true);
                PromptIfNotSet(taskConfigs, "SourceLocation");
                PromptIfNotSet(taskConfigs, "Branch");
                PromptIfNotSet(taskConfigs, "CreatePullRequest", allowedValues: new string[] { "true", "false" });
                PromptIfNotSet(taskConfigs, "PullRequestTargetBranch");
                PromptIfNotSet(taskConfigs, "CommitMessage");
                PromptIfNotSet(taskConfigs, "Author");
                PromptIfNotSet(taskConfigs, "Email");
            }
            else if (Type.ToLower() == JobTaskDefinitionType.Merge.ToLower())
            {
                PromptIfNotSet(taskConfigs, "Repository", true);
            }
            else if (Type.ToLower() == JobTaskDefinitionType.Build.ToLower())
            {
                PromptIfNotSet(taskConfigs, "SourceLocation");
                PromptIfNotSet(taskConfigs, "OutputArtifactLocation");
            }
            else if (Type.ToLower() == JobTaskDefinitionType.Deploy.ToLower())
            {
                PromptIfNotSet(taskConfigs, "ArtifactLocation");
            }
            else if (Type.ToLower() == JobTaskDefinitionType.DeployDb.ToLower())
            {
                PromptIfNotSet(taskConfigs, "MigrationLocation");
            }
            else if (Type.ToLower() == JobTaskDefinitionType.Test.ToLower())
            {
                PromptIfNotSet(taskConfigs, "TestLocation");
                PromptIfNotSet(taskConfigs, "ContinueWhenFailed", allowedValues: new string[] { "true", "false" });
            }

            if (_firstConfigPrompt)
                Console.WriteLine();

            return taskConfigs;
        }

        private void PromptIfNotSet(List<(string, string)> properties, string propertyName, bool required = false, string[] allowedValues = null)
        {
            string input = null;
            bool validInput;

            string prompt = $"{propertyName}{(required ? " (Required):" : ":")}";
            if (!properties.Any(p => p.Item1 == propertyName))
            {
                if (!_firstConfigPrompt)
                {
                    Console.WriteLine($"{Type} task config:");
                    _firstConfigPrompt = true;
                }                    

                do
                {
                    input = Console.GetString(prompt);

                    if (allowedValues != null && allowedValues.Length > 0 && !string.IsNullOrEmpty(input) && !allowedValues.Contains(input))
                    {
                        Console.WriteLine($"Input is not valid. Please enter the allowed values: {string.Join(',', allowedValues)}");
                        validInput = false;
                    }
                    else
                    {
                        validInput = true;
                    }
                } while (!validInput || (required && string.IsNullOrEmpty(input)));

                if (!string.IsNullOrEmpty(input))
                {
                    properties.Add((propertyName, input));
                }
            }
        }
    }
}
