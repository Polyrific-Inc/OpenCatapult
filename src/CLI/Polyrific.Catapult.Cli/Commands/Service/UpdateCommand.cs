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
    [Command(Description = "Update an external service")]
    public class UpdateCommand : BaseCommand
    {
        private readonly IConsoleReader _consoleReader;
        private readonly IExternalServiceService _externalServiceService;
        private readonly IExternalServiceTypeService _externalServiceTypeService;

        public UpdateCommand(IConsole console, ILogger<UpdateCommand> logger, IConsoleReader consoleReader, IExternalServiceService externalServiceService, IExternalServiceTypeService externalServiceTypeService) : base(console, logger)
        {
            _consoleReader = consoleReader;
            _externalServiceService = externalServiceService;
            _externalServiceTypeService = externalServiceTypeService;
        }

        [Required]
        [Option("-n|--name <NAME>", "Name of the external service", CommandOptionType.SingleValue)]
        public string Name { get; set; }

        [Option("-d|--description <DESCRIPTION>", "Description of the external service", CommandOptionType.SingleValue)]
        public string Description { get; set; }

        public override string Execute()
        {
            string message = string.Empty;

            var service = _externalServiceService.GetExternalServiceByName(Name).Result;

            if (service != null)
            {
                Console.WriteLine("Please enter the updated service properties (leave blank if it's unchanged):");
                var serviceType = _externalServiceTypeService.GetExternalServiceType(service.ExternalServiceTypeId).Result;
                foreach (var property in serviceType.ExternalServiceProperties)
                {
                    string input = null;
                    string prompt = $"{(!string.IsNullOrEmpty(property.Description) ? property.Description : property.Name)}:";


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

                    } while (!validInput);

                    if (!string.IsNullOrEmpty(input))
                        service.Config[property.Name] = input;
                }

                _externalServiceService.UpdateExternalService(service.Id, new UpdateExternalServiceDto
                {
                    Description = Description ?? service.Description,
                    Config = service.Config
                }).Wait();
                message = $"External Service {Name} was updated";
                Logger.LogInformation(message);
            }
            else
            {
                message = $"External Service {Name} is not found";
            }

            return message;
        }
    }
}
