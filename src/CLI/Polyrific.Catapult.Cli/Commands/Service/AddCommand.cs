// Copyright (c) Polyrific, Inc 2018. All rights reserved.

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
        private readonly IExternalServiceService _externalServiceService;

        public AddCommand(IConsole console, ILogger<AddCommand> logger, IExternalServiceService externalServiceService) : base(console, logger)
        {
            _externalServiceService = externalServiceService;
        }

        [Required]
        [Option("-n|--name <NAME>", "Name of the external service", CommandOptionType.SingleValue)]
        public string Name { get; set; }
        
        [Option("-t|--type <TYPE>", "Type of the external service", CommandOptionType.SingleValue)]
        public string Type { get; set; }

        [Option("-d|--description <DESCRIPTION>", "Description of the external service", CommandOptionType.SingleValue)]
        public string Description { get; set; }

        [Required]
        [Option("-prop|--property <KEY>:<PROPERTY>", "Property of the external service", CommandOptionType.MultipleValue)]
        public (string, string)[] Property { get; set; }

        public override string Execute()
        {
            string message = string.Empty;

            var service = _externalServiceService.CreateExternalService(new CreateExternalServiceDto
            {
                Name = Name,
                Description = Description,
                Type = Type,
                Config = Property?.ToDictionary(x => x.Item1, x => x.Item2)
            }).Result;

            message = service.ToCliString($"External service {Name} created:");
            Logger.LogInformation(message);

            return message;
        }
    }
}
