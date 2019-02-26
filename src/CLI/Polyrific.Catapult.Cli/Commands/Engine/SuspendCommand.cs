// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Engine
{
    [Command("suspend", Description = "Suspend an engine")]
    public class SuspendCommand : BaseCommand
    {
        private readonly ICatapultEngineService _engineService;

        public SuspendCommand(IConsole console, ILogger<SuspendCommand> logger, ICatapultEngineService engineService) : base(console, logger)
        {
            _engineService = engineService;
        }

        [Required]
        [Option("-n|--name <NAME>", "Name of the engine", CommandOptionType.SingleValue)]
        public string Name { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to suspend engine {Name}...");

            string message;
            var engine = _engineService.GetCatapultEngineByName(Name).Result;

            if (engine != null)
            {
                _engineService.Suspend(engine.Id).Wait();
                message = $"Engine {Name} has been suspended successfully";
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
