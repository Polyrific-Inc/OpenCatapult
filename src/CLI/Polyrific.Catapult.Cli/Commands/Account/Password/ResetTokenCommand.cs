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
        [Option("-u|--user <USER>", "Username (email) of the user", CommandOptionType.SingleValue)]
        public string User { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Requesting reset password token for user {User}...");

            string message;

            var user = _accountService.GetUserByEmail(User).Result;
            if (user != null)
            {
                var userId = int.Parse(user.Id);
                _accountService.RequestResetPassword(userId).Wait();
                message = $"Reset password token has been sent to {User}";
            }
            else
            {
                message = $"User {User} is not found";
            }

            return message;
        }
    }
}
