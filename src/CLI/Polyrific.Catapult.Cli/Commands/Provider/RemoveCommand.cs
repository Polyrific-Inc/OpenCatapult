// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Provider
{
    [Command("remove", Description = "Remove a task provider registration")]
    public class RemoveCommand : BaseCommand
    {
        private readonly IProviderService _providerService;

        public RemoveCommand(IProviderService providerService, IConsole console, ILogger<RemoveCommand> logger) : base(console, logger)
        {
            _providerService = providerService;
        }

        [Option("-n|--name", "Name of the task provider", CommandOptionType.SingleValue)]
        public string ProviderName { get; set; }

        [Option("-ac|--autoconfirm", "Execute the command without the need of confirmation prompt", CommandOptionType.NoValue)]
        public bool AutoConfirm { get; set; }

        public override string Execute()
        {
            if (!(AutoConfirm || Console.GetYesNo($"Are you sure you want to remove task provider {ProviderName}?", false)))
                return string.Empty;

            Console.WriteLine($"Trying to remove task provider \"{ProviderName}\"...");

            var provider = _providerService.GetProviderByName(ProviderName).Result;
            if (provider == null)
                return $"Task provider {ProviderName} was not found.";

            _providerService.DeleteProvider(provider.Id).Wait();

            var message = $"Task provider {ProviderName} has been removed.";
            Logger.LogInformation(message);

            return message;
        }
    }
}
