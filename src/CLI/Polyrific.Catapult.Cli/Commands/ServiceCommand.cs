// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Commands.Service;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "External service related commands")]
    [Subcommand("add", typeof(AddCommand))]
    [Subcommand("get", typeof(GetCommand))]
    [Subcommand("list", typeof(ListCommand))]
    [Subcommand("remove", typeof(RemoveCommand))]
    [Subcommand("update", typeof(UpdateCommand))]
    public class ServiceCommand : BaseCommand
    {
        public ServiceCommand(IConsole console, ILogger<ServiceCommand> logger) : base(console, logger)
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
