// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Commands.Account.TwoFactor;

namespace Polyrific.Catapult.Cli.Commands.Account
{
    [Command("twofactor", Description = "Two factor authentication related command")]
    [Subcommand(typeof(DisableCommand))]
    [Subcommand(typeof(EnableCommand))]
    [Subcommand(typeof(ResetAuthenticatorCommand))]
    [Subcommand(typeof(ResetRecoveryCommand))]
    public class TwoFactorCommand : BaseCommand
    {
        public TwoFactorCommand(IConsole console, ILogger<TwoFactorCommand> logger) : base(console, logger)
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
