// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Commands.Queue;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "Job queue related command")]
    [Subcommand("add", typeof(AddCommand))]
    [Subcommand("get", typeof(GetCommand))]
    [Subcommand("log", typeof(LogCommand))]
    [Subcommand("list", typeof(ListCommand))]
    [Subcommand("restart", typeof(RestartCommand))]
    [Subcommand("cancel", typeof(CancelCommand))]
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
