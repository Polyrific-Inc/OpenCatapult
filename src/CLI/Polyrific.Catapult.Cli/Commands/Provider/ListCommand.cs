// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Provider
{
    [Command("list", Description = "List registered task providers")]
    public class ListCommand : BaseCommand
    {
        private readonly IProviderService _providerService;

        public ListCommand(IProviderService providerService, IConsole console, ILogger<ListCommand> logger) : base(console, logger)
        {
            _providerService = providerService;
        }

        [Option("-t|--type", "Type of the task provider", CommandOptionType.SingleOrNoValue)]
        [AllowedValues(
            "all",
            Shared.Dto.Constants.ProviderType.BuildProvider,
            Shared.Dto.Constants.ProviderType.HostingProvider,
            Shared.Dto.Constants.ProviderType.GeneratorProvider,
            Shared.Dto.Constants.ProviderType.RepositoryProvider,
            Shared.Dto.Constants.ProviderType.DatabaseProvider,
            Shared.Dto.Constants.ProviderType.StorageProvider,
            Shared.Dto.Constants.ProviderType.TestProvider)]
        public string ProviderType { get; set; }

        public override string Execute()
        {
            Console.WriteLine("Trying to get list of task providers...");

            if (string.IsNullOrEmpty(ProviderType))
                ProviderType = "all";

            var providers = _providerService.GetProviders(ProviderType).Result;
            if (!providers.Any())
                return ProviderType == "all" ? "No registered task providers found." : $"No registered task providers with type {ProviderType} found.";
            
            return providers.ToListCliString($"Found {providers.Count} task provider(s):", excludedFields: new string[]
                {
                    "AdditionalConfigs"
                });
        }
    }
}
