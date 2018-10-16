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

            string message = $"Reset password token has been sent to {User}";

            var user = _accountService.GetUserByEmail(User).Result;
            if (user != null)
            {
                var userId = int.Parse(user.Id);
                _accountService.RequestResetPassword(userId).Wait();
            }
            else
            {
                Logger.LogWarning($"A reset password token was requested for user {User} that doesn't exist");
            }

            return message;
        }
    }
}
