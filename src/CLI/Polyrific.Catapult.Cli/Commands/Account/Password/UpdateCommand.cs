﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.User;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Account.Password
{
    [Command(Description = "Update user password")]
    public class UpdateCommand : BaseCommand
    {
        private readonly IAccountService _accountService;
        private readonly IConsoleReader _consoleReader;

        public UpdateCommand(IConsole console, ILogger<UpdateCommand> logger, IAccountService accountService, IConsoleReader consoleReader) : base(console, logger)
        {
            _accountService = accountService;
            _consoleReader = consoleReader;
        }

        [Required]
        [Option("-u|--user <USER>", "Username (email) of the user", CommandOptionType.SingleValue)]
        public string User { get; set; }
        
        public override string Execute()
        {
            string message = string.Empty;
            var user = _accountService.GetUserByEmail(User).Result;

            if (user != null)
            {
                int userId = int.Parse(user.Id);
                _accountService.UpdatePassword(userId, new UpdatePasswordDto
                {
                    Id = userId,
                    OldPassword = _consoleReader.GetPassword("Enter old password:"),
                    NewPassword = _consoleReader.GetPassword("Enter new password:"),
                    ConfirmNewPassword = _consoleReader.GetPassword("Re-enter new password:")
                }).Wait();
                message = $"Password for user {User} has been updated";
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
