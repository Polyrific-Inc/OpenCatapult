// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Engine.Commands.Config;

namespace Polyrific.Catapult.Engine.Commands
{
    [Command(Description = "Configure the engine")]
    [Subcommand(typeof(GetCommand))]
    [Subcommand(typeof(SetCommand))]
    [Subcommand(typeof(RemoveCommand))]
    [Subcommand(typeof(ImportCommand))]
    public class ConfigCommand : BaseCommand
    {
        public ConfigCommand(IConsole console, ILogger<ConfigCommand> logger) : base(console, logger)
        {
        }
        
        public override string Execute()
        {
            return "";
        }
    }
}
