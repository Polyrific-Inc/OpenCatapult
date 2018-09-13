// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.User;
using Polyrific.Catapult.Shared.Service;
using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Cli.Commands.Account
{
    [Command(Description = "Register a new user")]
    public class RegisterCommand : BaseCommand
    {
        private readonly IAccountService _accountService;

        public RegisterCommand(IConsole console, ILogger<RegisterCommand> logger, IAccountService accountService) : base(console, logger)
        {
            _accountService = accountService;
        }

        [Required]
        [Option("-e|--email <EMAIL>", "Email of the user", CommandOptionType.SingleValue)]
        public string Email { get; set; }

        [Required]
        [Option("-p|--password <PASSWORD>", "Password  of the user", CommandOptionType.SingleValue)]
        public string Password { get; set; }

        [Option("-fn|--firstname <FIRSTNAME>", "First name  of the user", CommandOptionType.SingleValue)]
        public string FirstName { get; set; }

        [Option("-ln|--lastname <LASTNAME>", "Last name of the user", CommandOptionType.SingleValue)]
        public string LastName { get; set; }

        public override string Execute()
        {
            string message = string.Empty;
            var user = _accountService.RegisterUser(new RegisterUserDto
            {
                Email = Email,
                Password = Password,
                ConfirmPassword = Password,
                FirstName = FirstName,
                LastName = LastName
            }).Result;

            message = user.ToCliString("User registered");
            Logger.LogInformation(message);

            return message;
        }
    }
}