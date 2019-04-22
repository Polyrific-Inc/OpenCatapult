// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Provider
{
    [Command("get", Description = "Get a single task provider details")]
    public class GetCommand : BaseCommand
    {
        private readonly IProviderService _providerService;

        public GetCommand(IProviderService providerService, IConsole console, ILogger<GetCommand> logger) : base(console, logger)
        {
            _providerService = providerService;
        }

        [Option("-n|--name", "Name of the task provider", CommandOptionType.SingleValue)]
        [Required]
        public string ProviderName { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to get task provider {ProviderName}...");
            var provider = _providerService.GetProviderByName(ProviderName).Result;
            return provider == null ? $"Task provider {ProviderName} was not found." : provider.ToCliString($"Task provider {ProviderName}:");
        }
    }
}
