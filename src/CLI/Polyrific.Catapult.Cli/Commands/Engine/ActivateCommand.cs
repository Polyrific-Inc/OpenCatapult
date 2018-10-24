// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Engine
{
    [Command(Description = "Activate a suspended engine")]
    public class ActivateCommand : BaseCommand
    {
        private readonly ICatapultEngineService _engineService;

        public ActivateCommand(IConsole console, ILogger<ActivateCommand> logger, ICatapultEngineService engineService) : base(console, logger)
        {
            _engineService = engineService;
        }

        [Required]
        [Option("-n|--name <NAME>", "Name of the engine", CommandOptionType.SingleValue)]
        public string Name { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to activate engine {Name}...");

            string message;
            var engine = _engineService.GetCatapultEngineByName(Name).Result;

            if (engine != null)
            {
                _engineService.Reactivate(engine.Id).Wait();
                message = $"Engine {Name} has been activated successfully";
                Logger.LogInformation(message);
            }
            else
            {
                message = $"Engine {Name} was not found";
            }

            return message;
        }
    }
}
