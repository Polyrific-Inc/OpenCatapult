// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.User;
using Polyrific.Catapult.Shared.Service;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Cli.Commands.Account
{
    [Command("register", Description = "Register a new user")]
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
        
        [Option("-fn|--firstname <FIRSTNAME>", "First name  of the user", CommandOptionType.SingleValue)]
        public string FirstName { get; set; }

        [Option("-ln|--lastname <LASTNAME>", "Last name of the user", CommandOptionType.SingleValue)]
        public string LastName { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to register new user {Email}...");

            var externalAccountTypes = _accountService.GetExternalAccountTypes().Result;

            Console.WriteLine("Please enter the following additional user info if it is available");
            
            var newUser = new RegisterUserDto
            {
                Email = Email,
                FirstName = FirstName,
                LastName = LastName
            };

            var externalAccountIds = new Dictionary<string, string>();
            foreach (var externalAccountType in externalAccountTypes)
            {
                var input = Console.GetString($"{externalAccountType.Label} (Optional):");
                if (!string.IsNullOrEmpty(input))
                {
                    externalAccountIds.Add(externalAccountType.Key, input);
                }
            }

            if (externalAccountIds.Count > 0)
                newUser.ExternalAccountIds = externalAccountIds;
            
            var user = _accountService.RegisterUser(newUser).Result;

            var message = user.ToCliString($"User {Email} has been registered, but he/she needs to confirm the email first before being able to login.");
            Logger.LogInformation(message);

            return message;
        }
    }
}
