// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Service;
using System.ComponentModel.DataAnnotations;

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
        [Option("-u|--user <USER>", "Username (email) of the user", CommandOptionType.SingleValue)]
        public string User { get; set; }

        public override string Execute()
        {
            string message = string.Empty;
            var user = _accountService.GetUserByEmail(User).Result;

            if (user != null)
            {
                _accountService.RemoveUser(int.Parse(user.Id)).Wait();
                message = $"User {User} has been removed";
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
