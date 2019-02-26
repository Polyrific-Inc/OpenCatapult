﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Commands.Engine;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command("engine", Description = "Catapult Engine registration commands")]
    [Subcommand(typeof(ListCommand))]
    [Subcommand(typeof(GetCommand))]
    [Subcommand(typeof(RegisterCommand))]
    [Subcommand(typeof(TokenCommand))]
    [Subcommand(typeof(SuspendCommand))]
    [Subcommand(typeof(ActivateCommand))]
    [Subcommand(typeof(RemoveCommand))]
    public class EngineCommand : BaseCommand
    {
        public EngineCommand(IConsole console, ILogger<EngineCommand> logger) : base(console, logger)
        {
        }

        public override string Execute()
        {
            return string.Empty;
        }

        protected override int OnExecute(CommandLineApplication app)
        {
            base.OnExecute(app);
            app.ShowHelp();
            return 0;
        }
    }
}
