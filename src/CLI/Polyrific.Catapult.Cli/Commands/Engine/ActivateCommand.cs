// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Service;
using System.ComponentModel.DataAnnotations;

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
            string message = string.Empty;
            var engine = _engineService.GetCatapultEngineByName(Name).Result;

            if (engine != null)
            {
                _engineService.Reactivate(engine.Id).Wait();
                message = $"Engine {Name} has been activated";
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
