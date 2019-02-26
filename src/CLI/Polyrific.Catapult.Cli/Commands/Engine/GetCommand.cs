// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Engine
{
    [Command("get", Description = "Get a single engine record")]
    public class GetCommand : BaseCommand
    {
        private readonly ICatapultEngineService _engineService;

        public GetCommand(IConsole console, ILogger<GetCommand> logger, ICatapultEngineService engineService) : base(console, logger)
        {
            _engineService = engineService;
        }

        [Required]
        [Option("-n|--name <NAME>", "Name of the engine", CommandOptionType.SingleValue)]
        public string Name { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to get engine {Name}...");

            string message;
            var engine = _engineService.GetCatapultEngineByName(Name).Result;

            if (engine != null)
            {
                message = engine.ToCliString($"Engine {Name}");
            }
            else
            {
                message = $"Engine {Name} was not found";
            }

            return message;
        }
    }
}
