// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Commands.Task;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "Job task definition related command")]
    [Subcommand("add", typeof(AddCommand))]
    [Subcommand("get", typeof(GetCommand))]
    [Subcommand("list", typeof(ListCommand))]
    [Subcommand("update", typeof(UpdateCommand))]
    [Subcommand("remove", typeof(RemoveCommand))]
    public class TaskCommand : BaseCommand
    {
        public TaskCommand(IConsole console, ILogger<TaskCommand> logger) : base(console, logger)
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
