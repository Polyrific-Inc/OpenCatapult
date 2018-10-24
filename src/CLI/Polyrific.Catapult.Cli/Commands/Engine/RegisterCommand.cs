// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.CatapultEngine;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Engine
{
    [Command(Description = "Register a new engine")]
    public class RegisterCommand : BaseCommand
    {
        private readonly ICatapultEngineService _engineService;

        public RegisterCommand(IConsole console, ILogger<RegisterCommand> logger, ICatapultEngineService engineService) : base(console, logger)
        {
            _engineService = engineService;
        }

        [Required]
        [Option("-n|--name <NAME>", "Name of the engine", CommandOptionType.SingleValue)]
        public string Name { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to register new engine {Name}...");

            string message;
            var engine = _engineService.RegisterEngine(new RegisterCatapultEngineDto
            {
                Name = Name
            }).Result;

            message = engine.ToCliString($"Engine has been registered:");
            Logger.LogInformation(message);

            return message;
        }
    }
}
