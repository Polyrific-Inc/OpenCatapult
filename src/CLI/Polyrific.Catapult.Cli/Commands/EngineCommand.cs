// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Commands.Engine;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "Catapult Engine registration commands")]
    [Subcommand("list", typeof(ListCommand))]
    [Subcommand("get", typeof(GetCommand))]
    [Subcommand("register", typeof(RegisterCommand))]
    [Subcommand("token", typeof(TokenCommand))]
    [Subcommand("suspend", typeof(SuspendCommand))]
    [Subcommand("activate", typeof(ActivateCommand))]
    [Subcommand("remove", typeof(RemoveCommand))]
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