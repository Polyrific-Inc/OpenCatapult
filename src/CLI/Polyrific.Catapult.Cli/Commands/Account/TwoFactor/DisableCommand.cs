using System;
using System.Collections.Generic;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Account.TwoFactor
{
    [Command("disable", Description = "Disable 2fa command")]
    public class DisableCommand : BaseCommand
    {
        private readonly IAccountService _accountService;

        public DisableCommand(IConsole console, ILogger<DisableCommand> logger, IAccountService accountService) : base(console, logger)
        {
            _accountService = accountService;
        }

        public override string Execute()
        {
            Console.WriteLine($"Trying to disable 2fa for current user...");

            string message;
            _accountService.DisableTwoFactor().Wait();

            message = $"2fa has been disable";
            Logger.LogInformation(message);

            return message;
        }
    }
}
