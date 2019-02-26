// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.User;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Account
{
    [Command("setrole", Description = "Set the role of a user")]
    public class SetRoleCommand : BaseCommand
    {
        private readonly IAccountService _accountService;

        public SetRoleCommand(IConsole console, ILogger<SetRoleCommand> logger, IAccountService accountService) : base(console, logger)
        {
            _accountService = accountService;
        }

        [Required]
        [Option("-u|--user <USER>", "Username (email) of the user", CommandOptionType.SingleValue)]
        public string User { get; set; }

        [Required]
        [Option("-r|--role <ROLE>", "Role of user", CommandOptionType.SingleValue)]
        [AllowedValues(UserRole.Administrator, UserRole.Basic, UserRole.Guest, IgnoreCase = true)]
        public string Role { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to update role of user {User}...");

            string message;

            var user = _accountService.GetUserByEmail(User).Result;
            if (user != null)
            {
                var userId = int.Parse(user.Id);
                _accountService.SetUserRole(userId, new SetUserRoleDto
                {
                    UserId = userId,
                    RoleName = Role,
                }).Wait();

                message = $"User {User} has been assigned to role \"{Role}\"";
                Logger.LogInformation(message);
            }
            else
            {
                message = $"User {User} was not found";
            }

            return message;
        }
    }
}
