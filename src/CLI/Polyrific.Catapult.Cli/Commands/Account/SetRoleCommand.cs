﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.User;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Account
{
    [Command(Description = "SetRole user profile")]
    public class SetRoleCommand : BaseCommand
    {
        private readonly IAccountService _accountService;

        public SetRoleCommand(IConsole console, ILogger<SetRoleCommand> logger, IAccountService accountService) : base(console, logger)
        {
            _accountService = accountService;
        }

        [Required]
        [Option("-e|--email <EMAIL>", "Email of the user", CommandOptionType.SingleValue)]
        public string Email { get; set; }

        [Required]
        [Option("-r|--role <ROLE>", "Role of user", CommandOptionType.SingleValue)]
        [AllowedValues(UserRole.Administrator, UserRole.Basic, UserRole.Guest, IgnoreCase = true)]
        public string Role { get; set; }

        public override string Execute()
        {
            string message = string.Empty;
            var user = _accountService.GetUserByEmail(Email).Result;

            if (user != null)
            {
                int userId = int.Parse(user.Id);
                _accountService.SetUserRole(userId, new SetUserRoleDto
                {
                    UserId = userId,
                    RoleName = Role,
                }).Wait();
                message = $"User {Email} has been set to role \"{Role}\"";
                Logger.LogInformation(message);
            }
            else
            {
                message = $"User {Email} is not found";
            }

            return message;
        }
    }
}
