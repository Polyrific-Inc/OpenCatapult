// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Commands.Provider;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "Task provider registration commands")]
    [Subcommand(typeof(ListCommand))]
    [Subcommand(typeof(GetCommand))]
    [Subcommand(typeof(RegisterCommand))]
    [Subcommand(typeof(RemoveCommand))]
    public class ProviderCommand : BaseCommand
    {
        public ProviderCommand(IConsole console, ILogger<ProviderCommand> logger) : base(console, logger)
        {
        }

        public override string Execute()
        {
            return string.Empty;
        }
    }
}
