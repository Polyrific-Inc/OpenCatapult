// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.JobDefinition;
using Polyrific.Catapult.Shared.Dto.Provider;
using Polyrific.Catapult.Shared.Dto.Project;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;
using Polyrific.Catapult.Shared.Service;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Polyrific.Catapult.Cli.Commands.Project
{
    [Command(Description = "Create a project")]
    public class CreateCommand : BaseCommand
    {
        private readonly IConsoleReader _consoleReader;
        private readonly IProjectService _projectService;
        private readonly IProviderService _providerService;
        private readonly IExternalServiceService _externalServiceService;
        private readonly ITemplateWriter _templateWriter;

        public CreateCommand(IConsole console, ILogger<CreateCommand> logger, IConsoleReader consoleReader, IProjectService projectService, IProviderService providerService, IExternalServiceService externalServiceService, ITemplateWriter templateWriter) : base(console, logger)
        {
            _consoleReader = consoleReader;
            _projectService = projectService;
            _providerService = providerService;
            _externalServiceService = externalServiceService;
            _templateWriter = templateWriter;
        }

        [Required]
        [Option("-n|--name <NAME>", "Name of the project", CommandOptionType.SingleValue)]
        public string Name { get; set; }

        [Option("-d|--displayname <DISPLAYNAME>", "Display Name of the project", CommandOptionType.SingleValue)]
        public string DisplayName { get; set; }

        [Option("-c|--client <CLIENT>", "Name of the client", CommandOptionType.SingleValue)]
        public string Client { get; set; }

        [Option("-t|--template <TEMPLATE>", "Project template", CommandOptionType.SingleValue)]
        public string Template { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to create project \"{Name}\"...");

            NewProjectDto projectDto;
            if (!string.IsNullOrEmpty(Template))
            {
                var projectTemplate = _templateWriter.Read(Template);
                projectDto = DeserializeYaml<NewProjectDto>(projectTemplate);
                projectDto.Name = Name;
                projectDto.DisplayName = DisplayName;
                projectDto.Client = Client;
            }
            else
            {
                projectDto = new NewProjectDto
                {
                    Name = Name,
                    Client = Client,
                    DisplayName = DisplayName,
                    Members = new List<NewProjectMemberDto>()
                };
            }

            projectDto.Members = projectDto.Members ?? new List<NewProjectMemberDto>();
            projectDto.Models = projectDto.Models ?? new List<CreateProjectDataModelWithPropertiesDto>();
            projectDto.Jobs = projectDto.Jobs ?? new List<CreateJobDefinitionWithTasksDto>();

            var message = ValidateTask(projectDto.Jobs);
            if (!string.IsNullOrEmpty(message))
                return message;

            var project = _projectService.CreateProject(projectDto).Result;

            message = project.ToCliString("Project created:");
            Logger.LogInformation(message);
            
            return message;
        }

        private T DeserializeYaml<T>(string templateYaml)
        {
            var deserializer = new DeserializerBuilder().WithNamingConvention(new HyphenatedNamingConvention()).IgnoreUnmatchedProperties().Build();
            return deserializer.Deserialize<T>(templateYaml);
        }

        private string ValidateTask(List<CreateJobDefinitionWithTasksDto> jobs)
        {
            var tasks = jobs.SelectMany(j => j.Tasks).ToArray();

            var isProviderOk = ValidateProviders(tasks.Select(t => t.Provider), out var providers);
            if (!isProviderOk)
                return "Please register the required providers first by using \"provider register\" command.";

            var serviceTypeNames = providers.Where(p => p.RequiredServices != null).SelectMany(p => p.RequiredServices);
            var taskConfigs = tasks.Where(t => t.Configs != null)
                .SelectMany(t => t.Configs)
                .Where(tc => tc.Key.EndsWith("ExternalService"));
            
            var isExternalServiceOk = ValidateExternalServices(serviceTypeNames, taskConfigs.ToList());
            if (!isExternalServiceOk)
                return "Please add the required external services first by using \"service add\" command.";

            string repository = null;
            string isPrivateRepository = null;
            int count = 1;
            foreach (var task in tasks)
            {
                task.Sequence = count++;

                // prompt for Repository config
                if ((task.Type.ToLower() == JobTaskDefinitionType.Clone.ToLower() ||
                    task.Type.ToLower() == JobTaskDefinitionType.Push.ToLower() ||
                    task.Type.ToLower() == JobTaskDefinitionType.Merge.ToLower()) && !task.Configs.ContainsKey("Repository"))
                {
                    if (string.IsNullOrEmpty(repository))
                        repository = PromptTaskConfig("Repository");

                    task.Configs["Repository"] = repository;
                }

                // prompt for IsPrivateRepository config
                if (task.Type.ToLower() == JobTaskDefinitionType.Clone.ToLower() && !task.Configs.ContainsKey("IsPrivateRepository"))
                {
                    if (string.IsNullOrEmpty(isPrivateRepository))
                        isPrivateRepository = PromptTaskConfig("IsPrivateRepository", configType: ConfigType.Boolean);

                    task.Configs["IsPrivateRepository"] = isPrivateRepository;
                }

                // prompt for provider additional configs
                var provider = providers.FirstOrDefault(p => p.Name == task.Provider);
                if (provider?.AdditionalConfigs != null && provider.AdditionalConfigs.Length > 0)
                {
                    task.AdditionalConfigs = task.AdditionalConfigs ?? new Dictionary<string, string>();
                    Console.WriteLine($"The provider \"{provider.Name}\" of task {task.Name} has some additional config(s):");
                    foreach (var additionalConfig in provider.AdditionalConfigs)
                    {
                        string input;
                        string hint = !string.IsNullOrEmpty(additionalConfig.Hint) ? $" - {additionalConfig.Hint}" : "";
                        string requiredAndHintText = additionalConfig.IsRequired ? $"(Required{hint}):" : $"(Leave blank to use default value{hint}):";
                        string label = !string.IsNullOrEmpty(additionalConfig.Label) ? additionalConfig.Label : additionalConfig.Name;
                        string prompt = $"{label} {requiredAndHintText}";
                        bool validInput = true;

                        do
                        {
                            if (additionalConfig.Type == ConfigType.Boolean)
                                input = Console.GetYesNoNullable(prompt)?.ToString();
                            else
                                input = additionalConfig.IsSecret && (additionalConfig.IsInputMasked ?? true) ? _consoleReader.GetPassword(prompt) : Console.GetString(prompt);
                            
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
                                    validInput = true;
                                }
                            }
                        } while (!validInput || (additionalConfig.IsRequired && string.IsNullOrEmpty(input)));

                        if (!string.IsNullOrEmpty(input))
                            task.AdditionalConfigs[additionalConfig.Name] = input;
                    }

                    Console.WriteLine();
                }
            }

            return "";
        }

        private bool ValidateProviders(IEnumerable<string> providerNames, out List<ProviderDto> providers)
        {
            providers = new List<ProviderDto>();
            var notExistProviders = new List<string>();

            if (Verbose)
                Console.WriteLine("The project requires the following providers:");
            
            foreach (var providerName in providerNames.Distinct())
            {
                var provider = _providerService.GetProviderByName(providerName).Result;
                if (provider == null)
                    notExistProviders.Add(providerName);
                else
                    providers.Add(provider);

                if (Verbose)
                {
                    var status = provider != null ? "OK" : "NOT REGISTERED";
                    Console.WriteLine($"- {providerName}: {status}");
                }
            }

            if (notExistProviders.Count == 0)
            {
                Logger.LogInformation("All required providers are registered.");
                return true;
            }

            var message = $"The following providers need to be registered before continuing the project creation: {string.Join(", ", notExistProviders)}";
            Logger.LogInformation(message);
            Console.WriteLine(message);

            return false;
        }

        private bool ValidateExternalServices(IEnumerable<string> serviceTypeNames, List<KeyValuePair<string, string>> taskConfigs)
        {
            var notExistServices = new List<string>();
            
            if (Verbose)
                Console.WriteLine("The project requires the following external services:");

            foreach (var serviceType in serviceTypeNames.Distinct())
            {
                var extServiceKey = $"{serviceType}ExternalService";
                var extServiceValue = "";
                if (taskConfigs.Exists(tc => tc.Key == extServiceKey))
                    extServiceValue = taskConfigs.First(tc => tc.Key == extServiceKey).Value;

                if (string.IsNullOrEmpty(extServiceValue))
                {
                    notExistServices.Add($"(type = {serviceType})");

                    if (Verbose)
                        Console.WriteLine($"- (type = {serviceType}): NOT FOUND");
                }
                else
                {
                    var extService = _externalServiceService.GetExternalServiceByName(extServiceValue).Result;
                    if (extService == null)
                        notExistServices.Add($"{extServiceValue} (type = {serviceType})");

                    if (Verbose)
                    {
                        var status = extService != null ? "OK" : "NOT FOUND";
                        Console.WriteLine($"- {extServiceValue} (type = {serviceType}): {status}");
                    }
                }
            }

            if (notExistServices.Count == 0)
            {
                Logger.LogInformation("All required external services are found.");
                return true;
            }

            var message = $"The following external services need to be added before continuing to create the project: {string.Join(", ", notExistServices)}";
            Logger.LogInformation(message);
            Console.WriteLine(message);
                
            return false;
        }

        private string PromptTaskConfig(string propertyName, string[] allowedValues = null, string configType = null)
        {
            string input;
            bool validInput;

            string prompt = $"{propertyName} (Required):";

            do
            {
                input = configType == ConfigType.Boolean ? Console.GetYesNoNullable(prompt)?.ToString() : Console.GetString(prompt);
                if (allowedValues != null && allowedValues.Length > 0 && !string.IsNullOrEmpty(input) && !allowedValues.Contains(input))
                {
                    Console.WriteLine($"Input is not valid. Please enter the allowed values: {string.Join(',', allowedValues)}");
                    validInput = false;
                }
                else
                {
                    validInput = true;
                }
            } while (!validInput || string.IsNullOrEmpty(input));

            return input;
        }
    }
}
