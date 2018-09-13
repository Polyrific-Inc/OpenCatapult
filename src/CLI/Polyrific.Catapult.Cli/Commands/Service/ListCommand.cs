// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Service
{
    [Command(Description = "List external service names the user has access to")]
    public class ListCommand : BaseCommand
    {
        private readonly IExternalServiceService _externalServiceService;

        public ListCommand(IConsole console, ILogger<ListCommand> logger, IExternalServiceService externalServiceService) : base(console, logger)
        {
            _externalServiceService = externalServiceService;
        }

        public override string Execute()
        {
            string message = string.Empty;

            var services = _externalServiceService.GetExternalServices().Result;

            message = services.ToListCliString($"Your external services:");

            return message;
        }
    }
}
