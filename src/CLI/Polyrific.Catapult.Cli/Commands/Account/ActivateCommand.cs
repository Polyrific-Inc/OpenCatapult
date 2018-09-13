// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Service;
using System.ComponentModel.DataAnnotations;

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
            string message = string.Empty;
            var user = _accountService.GetUserByEmail(Email).Result;

            if (user != null)
            {
                _accountService.ReactivateUser(int.Parse(user.Id)).Wait();
                message = $"User {Email} has been activated";
                Logger.LogInformation(message);
            }
            else
            {
                message = $"User {Email} is not found";
            }

            return message;
        }
    }
}