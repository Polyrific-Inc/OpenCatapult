// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.JobDefinition;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Task
{
    [Command(Description = "Update a job task definition")]
    public class UpdateCommand : BaseCommand
    {
        private readonly IConsoleReader _consoleReader;
        private readonly IProjectService _projectService;
        private readonly IJobDefinitionService _jobDefinitionService;
        private readonly IPluginService _pluginService;
        private readonly IExternalServiceService _externalServiceService;
        private readonly IExternalServiceTypeService _externalServiceTypeService;

        public UpdateCommand(IConsole console, ILogger<UpdateCommand> logger, IConsoleReader consoleReader,
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

        [Option("-rn|--rename <RENAME>", "New name of the job task definition", CommandOptionType.SingleValue)]
        public string Rename { get; set; }

        [Option("-t|--type <TYPE>", "Type of the task", CommandOptionType.SingleValue)]
        [AllowedValues(JobTaskDefinitionType.Clone, JobTaskDefinitionType.Generate, JobTaskDefinitionType.Push, JobTaskDefinitionType.Merge, JobTaskDefinitionType.Build,
            JobTaskDefinitionType.PublishArtifact, JobTaskDefinitionType.Deploy, JobTaskDefinitionType.DeployDb, JobTaskDefinitionType.Test)]
        public string Type { get; set; }

        [Option("-prov|--provider <PROVIDER>", "Provider of the job task definition", CommandOptionType.SingleValue)]
        public string Provider { get; set; }

        [Option("-prop|--property <KEY>:<PROPERTY>", "Property of the task", CommandOptionType.MultipleValue)]
        public (string, string)[] Property { get; set; }

        [Option("-s|--sequence <SEQUENCE>", "Sequence order of the job task definition", CommandOptionType.SingleValue)]
        public int? Sequence { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to update task \"{Name}\"...");

            string message;

            var project = _projectService.GetProjectByName(Project).Result;

            if (project != null)
            {
                var job = _jobDefinitionService.GetJobDefinitionByName(project.Id, Job).Result;

                if (job != null)
                {
                    var task = _jobDefinitionService.GetJobTaskDefinitionByName(project.Id, job.Id, Name).Result;

                    if (task != null)
                    {
                        var properties = new List<(string, string)>();
                        var provider = !string.IsNullOrEmpty(Provider) ? Provider : task.Provider;
                        if (!string.IsNullOrEmpty(provider))
                        {
                            var plugin = _pluginService.GetPluginByName(provider).Result;

                            if (plugin == null)
                            {
                                message = $"The provider \"{provider}\" is not installed";
                                return message;
                            }

                            // making sure it's equal case sensitive
                            provider = plugin.Name;

                            if (plugin.RequiredServices != null && plugin.RequiredServices.Length > 0)
                            {
                                Console.WriteLine($"The provider \"{provider}\" requires the following service(s): {string.Join(", ", plugin.RequiredServices)}.");

                                if (task.Provider == provider)
                                {
                                    Console.WriteLine("Leave blank if no changes for the external service");
                                }

                                foreach (var service in plugin.RequiredServices)
                                {
                                    var externalServiceKey = $"{service}ExternalService";
                                    var externalServiceName = Console.GetString($"{service} external service name:");

                                    if (!(task.Provider == provider && string.IsNullOrEmpty(externalServiceName)))
                                    {
                                        if (string.IsNullOrEmpty(externalServiceName))
                                        {
                                            message = $"The {service} external service is required for the provider {provider}. If you do not have it in the system, please add them using \"service add\" command";
                                            return message;
                                        }

                                        var externalService = _externalServiceService.GetExternalServiceByName(externalServiceName).Result;

                                        if (externalService == null)
                                        {
                                            message = $"The external service {externalServiceName} was not found.";
                                            return message;
                                        }

                                        if (externalService.ExternalServiceTypeName != service && externalService.ExternalServiceTypeName.ToLower() != ExternalServiceTypeName.Generic.ToLower())
                                        {
                                            message = $"The entered external service is not a {service} service";
                                            return message;
                                        }

                                        properties.Add(($"{service}ExternalService", externalServiceName));
                                    }                                    
                                }
                            }
                            
                            if (task.Provider != provider)
                            {
                                task.AdditionalConfigs = new Dictionary<string, string>();
                            }

                            if (plugin.AdditionalConfigs != null && plugin.AdditionalConfigs.Length > 0)
                            {
                                Console.WriteLine($"The provider \"{plugin.Name}\" have some additional config(s):");
                                foreach (var additionalConfig in plugin.AdditionalConfigs)
                                {
                                    string input = null;
                                    string prompt = $"{(!string.IsNullOrEmpty(additionalConfig.Label) ? additionalConfig.Label : additionalConfig.Name)}:";
                                    bool validInput = true;
                                    do
                                    {
                                        if (additionalConfig.Type == ConfigType.Boolean)
                                            input = Console.GetYesNoNullable(prompt)?.ToString();
                                        else if (additionalConfig.IsSecret && (additionalConfig.IsInputMasked ?? true))
                                            input = _consoleReader.GetPassword(prompt);
                                        else
                                            input = Console.GetString(prompt);

                                        if (!string.IsNullOrEmpty(input))
                                        {
                                            if (additionalConfig.Type == ConfigType.Number && !double.TryParse(input, out var inputNumber))
                                            {
                                                Console.WriteLine($"Input is not valid. Please enter valid number value.");
                                                validInput = false;
                                            }
                                            else if (additionalConfig.AllowedValues?.Length > 0 && !additionalConfig.AllowedValues.Contains(input))
                                            {
                                                Console.WriteLine($"Input is not valid. Please enter the allowed values: {string.Join(',', additionalConfig.AllowedValues)}");
                                                validInput = false;
                                            }
                                            else
                                            {
                                                if (task.Provider == provider)
                                                    task.AdditionalConfigs[additionalConfig.Name] = input;
                                                else
                                                    task.AdditionalConfigs.Add(additionalConfig.Name, input);

                                                validInput = true;
                                            }
                                        }
                                    } while (!validInput || (additionalConfig.IsRequired && task.AdditionalConfigs.GetValueOrDefault(additionalConfig.Name) == null));
                                }
                            }
                        }

                        if (Property != null)
                        {
                            properties.InsertRange(0, Property);
                        }

                        _jobDefinitionService.UpdateJobTaskDefinition(project.Id, job.Id, task.Id, new UpdateJobTaskDefinitionDto
                        {
                            Id = task.Id,
                            Type = Type ?? task.Type,
                            Provider = provider,
                            Name = Rename ?? task.Name,
                            Sequence = Sequence ?? task.Sequence,
                            Configs = properties.Count > 0 ? properties.ToDictionary(x => x.Item1, x => x.Item2) : task.Configs,
                            AdditionalConfigs = task.AdditionalConfigs
                        }).Wait();

                        message = $"Task {Name} has been updated successfully";
                        Logger.LogInformation(message);
                        return message;
                    }
                }

            }

            message = $"Failed to update task {Name}. Make sure the project and job definition names are correct.";

            return message;
        }
    }
}
