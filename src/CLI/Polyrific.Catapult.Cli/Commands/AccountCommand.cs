// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Commands.Account;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "User account related command")]
    [Subcommand(typeof(ListCommand))]
    [Subcommand(typeof(ActivateCommand))]
    [Subcommand(typeof(RegisterCommand))]
    [Subcommand(typeof(RemoveCommand))]
    [Subcommand(typeof(SuspendCommand))]
    [Subcommand(typeof(UpdateCommand))]
    [Subcommand(typeof(PasswordCommand))]
    [Subcommand(typeof(SetRoleCommand))]
    public class AccountCommand : BaseCommand
    {
        public AccountCommand(IConsole console, ILogger<AccountCommand> logger) : base(console, logger)
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
