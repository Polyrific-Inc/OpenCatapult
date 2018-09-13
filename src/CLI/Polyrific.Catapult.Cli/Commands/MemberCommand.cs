// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Commands.Member;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "Project member related command")]
    [Subcommand("add", typeof(AddCommand))]
    [Subcommand("list", typeof(ListCommand))]
    [Subcommand("remove", typeof(RemoveCommand))]
    [Subcommand("update", typeof(UpdateCommand))]
    public class MemberCommand : BaseCommand
    {
        public MemberCommand(IConsole console, ILogger<MemberCommand> logger) : base(console, logger)
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