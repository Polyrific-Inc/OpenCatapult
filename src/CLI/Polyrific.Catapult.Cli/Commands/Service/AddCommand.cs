// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.ExternalService;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Service
{
    [Command(Description = "Add new external service")]
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
        public string Type { get; set; }

        [Option("-d|--description <DESCRIPTION>", "Description of the external service", CommandOptionType.SingleValue)]
        public string Description { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to add external service \"{Name}\"...");

            string message;

            var serviceType = _externalServiceTypeService.GetExternalServiceTypeByName(Type).Result;

            if (serviceType != null)
            {
                if (serviceType.ExternalServiceProperties != null && serviceType.ExternalServiceProperties.Count > 0)
                {
                    Console.WriteLine("Please enter the service properties:");
                    var config = new Dictionary<string, string>();

                    var secretProperties = new List<string>();
                    foreach (var property in serviceType.ExternalServiceProperties)
                    {
                        string input = string.Empty;
                        string prompt = $"{(!string.IsNullOrEmpty(property.Description) ? property.Description : property.Name)}{(property.IsRequired ? " (Required):" : ":")}";

                        bool validInput;
                        do
                        {
                            if (property.IsSecret)
                                input = _consoleReader.GetPassword(prompt);
                            else
                                input = Console.GetString(prompt);

                            if (property.AllowedValues != null && property.AllowedValues.Length > 0 && !string.IsNullOrEmpty(input) && !property.AllowedValues.Contains(input))
                            {
                                Console.WriteLine($"Input is not valid. Please enter the allowed values: {string.Join(',', property.AllowedValues)}");
                                validInput = false;
                            }
                            else
                            {
                                validInput = true;
                            }

                        } while (!validInput || (property.IsRequired && string.IsNullOrEmpty(input)));

                        config.Add(property.Name, input);

                        if (property.IsSecret)
                            secretProperties.Add(property.Name);
                    }

                    var service = _externalServiceService.CreateExternalService(new CreateExternalServiceDto
                    {
                        Name = Name,
                        Description = Description,
                        ExternalServiceTypeId = serviceType.Id,
                        Config = config
                    }).Result;

                    message = service.ToCliString($"External service has been added:", secretProperties.ToArray());
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
    }
}
