// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.User;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Account.Password
{
    [Command("reset", Description = "Reset user password")]
    public class ResetCommand : BaseCommand
    {
        private readonly IAccountService _accountService;
        private readonly IConsoleReader _consoleReader;

        public ResetCommand(IConsole console, ILogger<ResetCommand> logger, IAccountService accountService, IConsoleReader consoleReader) : base(console, logger)
        {
            _accountService = accountService;
            _consoleReader = consoleReader;
        }

        [Required]
        [Option("-u|--user <USER>", "Username (email) of the user", CommandOptionType.SingleValue)]
        public string User { get; set; }

        [Required]
        [Option("-t|--token <TOKEN>", "Reset password token", CommandOptionType.SingleValue)]
        public string Token { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to reset password for user {User}...");

            string message;
            _accountService.ResetPassword(User, new ResetPasswordDto
            {
                Token = Token,
                NewPassword = _consoleReader.GetPassword("Enter new password:"),
                ConfirmNewPassword = _consoleReader.GetPassword("Re-enter new password:")
            }).Wait();

            message = $"Password for user {User} has been reset";
            Logger.LogInformation(message);

            return message;
        }
    }
}
