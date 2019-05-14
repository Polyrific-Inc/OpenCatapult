// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.ExternalService;
using Polyrific.Catapult.Shared.Dto.ExternalServiceType;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Service
{
    [Command("add", Description = "Add new external service")]
    public class AddCommand : BaseCommand
    {
        private readonly IConsoleReader _consoleReader;
        private readonly IExternalServiceService _externalServiceService;
        private readonly IExternalServiceTypeService _externalServiceTypeService;

        public AddCommand(IConsole console, ILogger<AddCommand> logger, IConsoleReader consoleReader, IExternalServiceService externalServiceService, IExternalServiceTypeService externalServiceTypeService) : base(console, logger)
        {
            _consoleReader = consoleReader;
            _externalServiceService = externalServiceService;
            _externalServiceTypeService = externalServiceTypeService;
        }

        [Required]
        [Option("-n|--name <NAME>", "Name of the external service", CommandOptionType.SingleValue)]
        public string Name { get; set; }
        
        [Option("-t|--type <TYPE>", "Type of the external service", CommandOptionType.SingleValue)]
        [Required]
        [AllowedValues(ExternalServiceTypeName.Generic, ExternalServiceTypeName.GitHub, ExternalServiceTypeName.Azure, IgnoreCase = true)]
        public string Type { get; set; }

        [Option("-d|--description <DESCRIPTION>", "Description of the external service", CommandOptionType.SingleValue)]
        public string Description { get; set; }

        [Option("-g|--global", "Indicates whether the external service can be accessed globally?", CommandOptionType.NoValue)]
        public bool Global { get; set; }

        [Option("-prop|--property <KEY>:<PROPERTY>", "Property of the external service", CommandOptionType.MultipleValue)]
        public (string, string)[] Property { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to add external service \"{Name}\"...");

            string message;

            var serviceType = _externalServiceTypeService.GetExternalServiceTypeByName(Type).Result;

            if (serviceType != null)
            {
                if ((serviceType.ExternalServiceProperties != null && serviceType.ExternalServiceProperties.Count > 0) || Type.ToLower() == ExternalServiceTypeName.Generic.ToLower())
                {
                    Console.WriteLine("Please enter the service properties:");
                    var config = Property?.ToDictionary(x => x.Item1, x => x.Item2) ?? new Dictionary<string, string>();

                    var secretProperties = new List<string>();
                    foreach (var property in serviceType.ExternalServiceProperties)
                    {
                        if (IsPropertySet(property.Name) || CheckPropertyCondition(property.AdditionalLogic?.HideCondition, config))
                            continue;

                        var isRequired = property.IsRequired || CheckPropertyCondition(property.AdditionalLogic?.RequiredCondition, config);
                        string input = string.Empty;
                        string prompt = $"{(!string.IsNullOrEmpty(property.Description) ? property.Description : property.Name)}{(isRequired ? " (Required):" : ":")}";

                        bool validInput = true;
                        do
                        {
                            if (property.IsSecret)
                                input = _consoleReader.GetPassword(prompt);
                            else
                                input = Console.GetString(prompt);
                            
                            if (property.AllowedValues != null && property.AllowedValues.Length > 0)
                            {
                                if (!string.IsNullOrEmpty(input) && !property.AllowedValues.Contains(input))
                                {
                                    Console.WriteLine($"Input is not valid. Please enter the allowed values: {string.Join(',', property.AllowedValues)}");
                                    validInput = false;
                                }
                                else if (string.IsNullOrEmpty(input) && isRequired)
                                {
                                    input = property.AllowedValues[0];
                                    validInput = true;
                                }
                                else
                                {
                                    validInput = true;
                                }
                            }

                        } while (!validInput || (isRequired && string.IsNullOrEmpty(input)));

                        config.Add(property.Name, input);

                        if (property.IsSecret)
                            secretProperties.Add(property.Name);
                    }

                    var service = _externalServiceService.CreateExternalService(new CreateExternalServiceDto
                    {
                        Name = Name,
                        Description = Description,
                        ExternalServiceTypeId = serviceType.Id,
                        IsGlobal = Global,
                        Config = config
                    }).Result;

                    message = service.ToCliString($"External service has been added:", secretProperties.ToArray(), excludedFields: new string[]
                    {
                        "ExternalServiceTypeId"
                    }, nameDictionary: new Dictionary<string, string>
                    {
                        {"Config", "Properties"}
                    });

                    Logger.LogInformation(message);
                }
                else
                {
                    message = $"Service type {Type} does not have properties set";
                }
            }
            else
            {
                message = $"Service type {Type} was not found";
            }            

            return message;
        }

        public override string GetHelpFooter()
        {
            Console.WriteLine("Trying to get available external services...");

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Available external services:");

            try
            {
                var serviceTypes = _externalServiceTypeService.GetExternalServiceTypes(true).Result;
                foreach (var serviceType in serviceTypes)
                {
                    sb.AppendLine($"  - Type: {serviceType.Name}");
                    if (serviceType.ExternalServiceProperties != null && serviceType.ExternalServiceProperties.Count > 0)
                    {
                        sb.AppendLine("    Properties:");
                        foreach (var property in serviceType.ExternalServiceProperties)
                            sb.AppendLine($"      - {property.Name} {(property.IsRequired ? "(required)" : "")}");
                    }
                }
            }
            catch
            {
                sb.AppendLine("Failed to retrieve external service types. Please try to login into application");
            }

            return sb.ToString();
        }

        private bool CheckPropertyCondition(PropertyConditionDto condition, Dictionary<string, string> properties)
        {
            if (condition == null || properties == null)
                return false;

            var propertyValue = properties.GetValueOrDefault(condition.PropertyName);
            return propertyValue == condition.PropertyValue;
        }

        private bool IsPropertySet(string propertyName)
        {
            return Property?.Any(p => p.Item1 == propertyName) ?? false;
        }
    }
}
