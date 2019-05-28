// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Account.TwoFactor
{
    [Command("resetauthenticator", Description = "Reset 2fa authenticator command")]
    public class ResetAuthenticatorCommand : BaseCommand
    {
        private readonly IAccountService _accountService;

        public ResetAuthenticatorCommand(IConsole console, ILogger<ResetAuthenticatorCommand> logger, IAccountService accountService) : base(console, logger)
        {
            _accountService = accountService;
        }

        public override string Execute()
        {
            Console.WriteLine($"Trying to reset 2FA authenticator for current user...");

            string message;
            _accountService.ResetAuthenticatorKey().Wait();

            message = $"2FA authenticator has been reset";
            Logger.LogInformation(message);

            return message;
        }
    }
}
