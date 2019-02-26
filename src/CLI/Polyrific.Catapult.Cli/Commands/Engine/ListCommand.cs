// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Engine
{
    [Command("list", Description = "List registered engines")]
    public class ListCommand : BaseCommand
    {
        private readonly ICatapultEngineService _engineService;

        public ListCommand(IConsole console, ILogger<ListCommand> logger, ICatapultEngineService engineService) : base(console, logger)
        {
            _engineService = engineService;
        }

        [Option("-s|--status <STATUS>", "Filter the engines by their status", CommandOptionType.SingleValue)]
        [AllowedValues(EngineStatus.All, EngineStatus.Active, EngineStatus.Suspended, EngineStatus.Running, IgnoreCase = true)]
        public string Status { get; set; } = EngineStatus.All;

        public override string Execute()
        {
            Console.WriteLine($"Trying to get engines with status = \"{Status}\"...");
            var engines = _engineService.GetCatapultEngines(Status).Result;
            return engines.ToListCliString($"Found {engines.Count} engine(s):");

        }
    }
}
