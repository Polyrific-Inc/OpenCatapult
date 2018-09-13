// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Commands.Plugin;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "Plugin registration commands")]
    [Subcommand("list", typeof(ListCommand))]
    [Subcommand("get", typeof(GetCommand))]
    [Subcommand("register", typeof(RegisterCommand))]
    [Subcommand("remove", typeof(RemoveCommand))]
    public class PluginCommand : BaseCommand
    {
        public PluginCommand(IConsole console, ILogger<PluginCommand> logger) : base(console, logger)
        {
        }

        public override string Execute()
        {
            return string.Empty;
        }
    }
}
