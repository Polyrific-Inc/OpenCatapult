// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Service;
using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Cli.Commands.Engine
{
    [Command(Description = "Remove an engine")]
    public class RemoveCommand : BaseCommand
    {
        private readonly ICatapultEngineService _engineService;

        public RemoveCommand(IConsole console, ILogger<RemoveCommand> logger, ICatapultEngineService engineService) : base(console, logger)
        {
            _engineService = engineService;
        }

        [Required]
        [Option("-n|--name <NAME>", "Name of the engine", CommandOptionType.SingleValue)]
        public string Name { get; set; }

        [Option("-ac|--autoconfirm", "Execute the command without the need of confirmation prompt", CommandOptionType.NoValue)]
        public bool AutoConfirm { get; set; }

        public override string Execute()
        {
            if (!(AutoConfirm || Console.GetYesNo($"Are you sure you want to remove engine {Name}?", false)))
                return string.Empty;

            string message;
            var engine = _engineService.GetCatapultEngineByName(Name).Result;

            if (engine != null)
            {
                _engineService.RemoveCatapultEngine(engine.Id).Wait();
                message = $"Engine {Name} has been removed";
                Logger.LogInformation(message);
            }
            else
            {
                message = $"Engine {Name} is not found";
            }

            return message;
        }
    }
}
