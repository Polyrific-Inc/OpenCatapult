﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Service;
using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Cli.Commands.Account
{
    [Command(Description = "Suspend a user")]
    public class SuspendCommand : BaseCommand
    {
        private readonly IAccountService _accountService;

        public SuspendCommand(IConsole console, ILogger<SuspendCommand> logger, IAccountService accountService) : base(console, logger)
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
                _accountService.SuspendUser(int.Parse(user.Id)).Wait();
                message = $"User {User} has been suspended";
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
