// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "Get current user")]
    public class CurrentUserCommand : BaseCommand
    {
        private readonly IAccountService _accountService;

        public CurrentUserCommand(IConsole console, ILogger<CurrentUserCommand> logger, IAccountService accountService) : base(console, logger)
        {
            _accountService = accountService;
        }
        
        public override string Execute()
        {
            try
            {
                Console.WriteLine("Checking current logged-in user...");

                var currentUser = _accountService.GetCurrentUser().Result;

                return $"Logged in as {currentUser.Email}";
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Unauthorized"))
                    return "Not logged in";

                throw;
            }
        }
    }
}
