// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Account.TwoFactor
{
    [Command("disable", Description = "Disable 2fa command")]
    public class DisableCommand : BaseCommand
    {
        private readonly IAccountService _accountService;

        [Option("-ac|--autoconfirm", "Execute the command without the need of confirmation prompt", CommandOptionType.NoValue)]
        public bool AutoConfirm { get; set; }

        public DisableCommand(IConsole console, ILogger<DisableCommand> logger, IAccountService accountService) : base(console, logger)
        {
            _accountService = accountService;
        }

        public override string Execute()
        {
            if (!(AutoConfirm || Console.GetYesNo($"Are you sure you want to disable the two factor authentication?", false)))
                return string.Empty;

            Console.WriteLine($"Trying to disable 2fa for current user...");

            string message;
            _accountService.DisableTwoFactor().Wait();

            message = $"2fa has been disabled";
            Logger.LogInformation(message);

            return message;
        }
    }
}
