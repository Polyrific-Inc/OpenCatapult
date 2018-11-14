// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Engine.Commands.Task;

namespace Polyrific.Catapult.Engine.Commands
{
    [Command(Description = "Job task commands")]
    [Subcommand("run", typeof(RunCommand))]
    public class TaskCommand : BaseCommand
    {
        public TaskCommand(IConsole console, ILogger<TaskCommand> logger) : base(console, logger)
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
