// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Service
{
    [Command(Description = "Display a single external service")]
    public class GetCommand : BaseCommand
    {
        private readonly IExternalServiceService _externalServiceService;

        public GetCommand(IConsole console, ILogger<GetCommand> logger, IExternalServiceService externalServiceService) : base(console, logger)
        {
            _externalServiceService = externalServiceService;
        }

        [Required]
        [Option("-n|--name <NAME>", "Name of the external service", CommandOptionType.SingleValue)]
        public string Name { get; set; }
        
        public override string Execute()
        {
            string message = string.Empty;

            var service = _externalServiceService.GetExternalServiceByName(Name).Result;
            
            if (service != null)
            {
                message = service.ToCliString($"External Service {Name}:");
            }
            else
            {
                message = $"External Service {Name} is not found";
            }

            return message;
        }
    }
}
