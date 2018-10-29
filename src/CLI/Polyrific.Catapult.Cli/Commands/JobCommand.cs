// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Commands.Job;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "Job definition related command")]
    [Subcommand("add", typeof(AddCommand))]
    [Subcommand("list", typeof(ListCommand))]
    [Subcommand("remove", typeof(RemoveCommand))]
    [Subcommand("update", typeof(UpdateCommand))]
    public class JobCommand : BaseCommand
    {
        public JobCommand(IConsole console, ILogger<JobCommand> logger) : base(console, logger)
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
