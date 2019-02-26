// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Account
{
    [Command("list", Description = "List users")]
    public class ListCommand : BaseCommand
    {
        private readonly IAccountService _accountService;

        public ListCommand(IConsole console, ILogger<ListCommand> logger, IAccountService accountService) : base(console, logger)
        {
            _accountService = accountService;
        }

        [Option("-s|--status <STATUS>", "Status of the users", CommandOptionType.SingleValue)]
        [AllowedValues(UserStatus.All, UserStatus.Active, UserStatus.Suspended, IgnoreCase = true)]
        public string Status { get; set; } = UserStatus.All;

        [Option("-r|--role <ROLE>", "Role of the users", CommandOptionType.SingleValue)]
        [AllowedValues(UserRole.All, UserRole.Administrator, UserRole.Basic, UserRole.Guest, IgnoreCase = true)]
        public string Role { get; set; } = UserRole.All;

        public override string Execute()
        {
            Console.WriteLine("Trying to get list of users...");

            var users = _accountService.GetUsers(Status, Role).Result;

            var message = users.ToListCliString($"Found {users.Count} user(s):");

            return message;
        }
    }
}
