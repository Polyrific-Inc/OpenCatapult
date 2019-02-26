// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.User;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Account.Password
{
    [Command("update", Description = "Update current user's password")]
    public class UpdateCommand : BaseCommand
    {
        private readonly IAccountService _accountService;
        private readonly IConsoleReader _consoleReader;

        public UpdateCommand(IConsole console, ILogger<UpdateCommand> logger, IAccountService accountService, IConsoleReader consoleReader) : base(console, logger)
        {
            _accountService = accountService;
            _consoleReader = consoleReader;
        }
        
        public override string Execute()
        {
            Console.WriteLine($"Trying to update password for current user...");

            string message;

            _accountService.UpdatePassword(new UpdatePasswordDto
            {
                OldPassword = _consoleReader.GetPassword("Enter old password:"),
                NewPassword = _consoleReader.GetPassword("Enter new password:"),
                ConfirmNewPassword = _consoleReader.GetPassword("Re-enter new password:")
            }).Wait();
            message = $"Password for current user has been updated";
            Logger.LogInformation(message);

            return message;
        }
    }
}
