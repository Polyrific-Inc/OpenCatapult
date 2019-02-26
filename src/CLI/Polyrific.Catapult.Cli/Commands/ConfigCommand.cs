// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Commands.Config;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "Configure the CLI")]
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

        protected override int OnExecute(CommandLineApplication app)
        {
            base.OnExecute(app);
            app.ShowHelp();
            return 0;
        }
    }
}
