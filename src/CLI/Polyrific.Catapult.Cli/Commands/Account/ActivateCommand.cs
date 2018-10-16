// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Account
{
    [Command(Description = "Activate a suspended user")]
    public class ActivateCommand : BaseCommand
    {
        private readonly IAccountService _accountService;

        public ActivateCommand(IConsole console, ILogger<ActivateCommand> logger, IAccountService accountService) : base(console, logger)
        {
            _accountService = accountService;
        }

        [Required]
        [Option("-e|--email <EMAIL>", "Email of the user", CommandOptionType.SingleValue)]
        public string Email { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to activate user {Email}...");

            string message;

            var user = _accountService.GetUserByEmail(Email).Result;
            if (user != null)
            {
                _accountService.ReactivateUser(int.Parse(user.Id)).Wait();

                message = $"User {Email} has been activated";
                Logger.LogInformation(message);
            }
            else
            {
                message = $"User {Email} was not found";
            }

            return message;
        }
    }
}
