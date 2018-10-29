// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Commands.Model;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "Project data model related command")]
    [Subcommand("add", typeof(AddCommand))]
    [Subcommand("get", typeof(GetCommand))]
    [Subcommand("list", typeof(ListCommand))]
    [Subcommand("remove", typeof(RemoveCommand))]
    [Subcommand("update", typeof(UpdateCommand))]
    public class ModelCommand : BaseCommand
    {
        public ModelCommand(IConsole console, ILogger<ModelCommand> logger) : base(console, logger)
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
