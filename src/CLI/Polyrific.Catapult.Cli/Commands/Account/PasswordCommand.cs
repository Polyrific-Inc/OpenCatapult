// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Commands.Account.Password;

namespace Polyrific.Catapult.Cli.Commands.Account
{
    [Command(Description = "Account password related command")]
    [Subcommand("update", typeof(Password.UpdateCommand))]
    [Subcommand("resettoken", typeof(ResetTokenCommand))]
    [Subcommand("reset", typeof(ResetCommand))]
    public class PasswordCommand : BaseCommand
    {
        public PasswordCommand(IConsole console, ILogger<PasswordCommand> logger) : base(console, logger)
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
