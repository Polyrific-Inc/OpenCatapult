﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.User;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Account.Password
{
    [Command(Description = "Reset user password")]
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

            var user = _accountService.GetUserByEmail(User).Result;
            if (user != null)
            {
                var userId = int.Parse(user.Id);
                _accountService.ResetPassword(userId, new ResetPasswordDto
                {
                    Id = userId,
                    Token = Token,
                    NewPassword = _consoleReader.GetPassword("Enter new password:"),
                    ConfirmNewPassword = _consoleReader.GetPassword("Re-enter new password:")
                }).Wait();

                message = $"Password for user {User} has been reset";
                Logger.LogInformation(message);
            }
            else
            {
                message = $"User {User} is not found";
            }

            return message;
        }
    }
}
