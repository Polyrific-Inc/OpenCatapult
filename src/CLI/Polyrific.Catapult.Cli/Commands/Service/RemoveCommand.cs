// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Service
{
    [Command("remove", Description = "Remove an external service")]
    public class RemoveCommand : BaseCommand
    {
        private readonly IExternalServiceService _externalServiceService;

        public RemoveCommand(IConsole console, ILogger<RemoveCommand> logger, IExternalServiceService externalServiceService) : base(console, logger)
        {
            _externalServiceService = externalServiceService;
        }

        [Required]
        [Option("-n|--name <NAME>", "Name of the external service", CommandOptionType.SingleValue)]
        public string Name { get; set; }

        [Option("-ac|--autoconfirm", "Execute the command without the need of confirmation prompt", CommandOptionType.NoValue)]
        public bool AutoConfirm { get; set; }

        public override string Execute()
        {
            if (!(AutoConfirm || Console.GetYesNo($"Are you sure you want to remove external service {Name}?", false)))
                return string.Empty;

            Console.WriteLine($"Trying to remove external service \"{Name}\"...");

            string message;

            var service = _externalServiceService.GetExternalServiceByName(Name).Result;

            if (service != null)
            {
                _externalServiceService.DeleteExternalService(service.Id).Wait();
                message = $"External Service {Name} has been removed successfully";
                Logger.LogInformation(message);
            }
            else
            {
                message = $"External Service {Name} was not found";
            }

            return message;
        }
    }
}
