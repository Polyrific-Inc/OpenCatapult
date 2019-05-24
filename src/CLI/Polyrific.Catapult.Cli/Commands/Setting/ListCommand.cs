// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Setting
{
    [Command("list", Description = "List of application settings")]
    public class ListCommand : BaseCommand
    {
        private readonly IApplicationSettingService _applicationSettingService;

        public ListCommand(IConsole console, ILogger<ListCommand> logger, IApplicationSettingService applicationSettingService) : base(console, logger)
        {
            _applicationSettingService = applicationSettingService;
        }

        public override string Execute()
        {
            Console.WriteLine("Trying to get list of settings...");

            string message;

            var settings = _applicationSettingService.GetApplicationSettingValue().Result;

            message = settings.ToCliString($"Application setting(s):");

            return message;
        }
    }
}
