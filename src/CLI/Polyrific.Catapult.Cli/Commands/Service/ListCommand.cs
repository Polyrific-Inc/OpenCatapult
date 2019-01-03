// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
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
            Console.WriteLine("Trying to get list of external services...");

            string message;

            var services = _externalServiceService.GetExternalServices().Result;

            message = services.ToListCliString($"Found {services.Count} external service(s):", excludedFields: new string[]
                {
                    "Properties",
                    "ExternalServiceTypeId"
                }, nameDictionary: new Dictionary<string, string>
                {
                    {"Config", "Properties"}
                });

            return message;
        }
    }
}
