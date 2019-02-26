// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Commands.Queue;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "Job queue related command")]
    [Subcommand(typeof(AddCommand))]
    [Subcommand(typeof(GetCommand))]
    [Subcommand(typeof(LogCommand))]
    [Subcommand(typeof(ListCommand))]
    [Subcommand(typeof(RestartCommand))]
    [Subcommand(typeof(CancelCommand))]
    public class QueueCommand : BaseCommand
    {
        public QueueCommand(IConsole console, ILogger<QueueCommand> logger) : base(console, logger)
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
