// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Account.Password
{
    [Command(Description = "Request reset password token")]
    public class ResetTokenCommand : BaseCommand
    {
        private readonly IAccountService _accountService;

        public ResetTokenCommand(IConsole console, ILogger<ResetTokenCommand> logger, IAccountService accountService) : base(console, logger)
        {
            _accountService = accountService;
        }

        [Required]
        [Option("-e|--email <EMAIL>", "Email of the user", CommandOptionType.SingleValue)]
        public string Email { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Requesting reset password token for user {Email}...");

            string message;
            
            var user = _accountService.GetUserByEmail(Email).Result;
            if (user != null)
            {
                var userId = int.Parse(user.Id);
                _accountService.RequestResetPassword(userId).Wait();
                
                message = $"Reset password token has been sent to {Email}";
            }
            else
            {
                message = $"User {Email} is not found";
            }

            return message;
        }
    }
}
