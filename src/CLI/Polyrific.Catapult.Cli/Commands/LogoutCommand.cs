// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "Logout from the application")]
    public class LogoutCommand : BaseCommand
    {
        private readonly ITokenStore _tokenStore;

        public LogoutCommand(IConsole console, ILogger<LogoutCommand> logger, ITokenStore tokenStore) : base(console, logger)
        {
            _tokenStore = tokenStore;
        }

        public override string Execute()
        {
            _tokenStore.DeleteToken().Wait();

            return $"Logged out";
        }
    }
}