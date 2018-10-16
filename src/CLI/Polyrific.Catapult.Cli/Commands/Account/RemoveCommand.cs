// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Account
{
    [Command(Description = "Remove a user")]
    public class RemoveCommand : BaseCommand
    {
        private readonly IAccountService _accountService;

        public RemoveCommand(IConsole console, ILogger<RemoveCommand> logger, IAccountService accountService) : base(console, logger)
        {
            _accountService = accountService;
        }

        [Required]
        [Option("-e|--email <EMAIL>", "Email of the user", CommandOptionType.SingleValue)]
        public string Email { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to remove user {Email}...");

            string message;
            
            var user = _accountService.GetUserByEmail(Email).Result;
            if (user != null)
            {
                _accountService.RemoveUser(int.Parse(user.Id)).Wait();
                
                message = $"User {Email} has been removed";
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
