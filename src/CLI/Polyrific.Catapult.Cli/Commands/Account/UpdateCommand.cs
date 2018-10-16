// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.User;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Account
{
    [Command(Description = "Update user profile")]
    public class UpdateCommand : BaseCommand
    {
        private readonly IAccountService _accountService;

        public UpdateCommand(IConsole console, ILogger<UpdateCommand> logger, IAccountService accountService) : base(console, logger)
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
            Console.WriteLine($"Trying to update user {Email}...");

            string message;
            
            var user = _accountService.GetUserByEmail(Email).Result;
            if (user != null)
            {
                var userId = int.Parse(user.Id);
                _accountService.UpdateUser(userId, new UpdateUserDto
                {
                    Id = userId,
                    FirstName = FirstName ?? user.FirstName,
                    LastName = LastName ?? user.LastName
                }).Wait();

                message = $"User {Email} has been updated";
                Logger.LogInformation(message);
            }
            else
            {
                message = $"User {Email} was not found";
            }

            return message;
        }
    }
}
