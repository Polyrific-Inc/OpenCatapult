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
            Shared.Dto.Constants.TaskProviderType.BuildProvider,
            Shared.Dto.Constants.TaskProviderType.HostingProvider,
            Shared.Dto.Constants.TaskProviderType.GeneratorProvider,
            Shared.Dto.Constants.TaskProviderType.RepositoryProvider,
            Shared.Dto.Constants.TaskProviderType.DatabaseProvider,
            Shared.Dto.Constants.TaskProviderType.StorageProvider,
            Shared.Dto.Constants.TaskProviderType.TestProvider)]
        public string TaskProviderType { get; set; }

        public override string Execute()
        {
            Console.WriteLine("Trying to get list of task providers...");

            if (string.IsNullOrEmpty(TaskProviderType))
                TaskProviderType = "all";

            var providers = _providerService.GetProviders(TaskProviderType).Result;
            if (!providers.Any())
                return TaskProviderType == "all" ? "No registered task providers found." : $"No registered task providers with type {TaskProviderType} found.";
            
            return providers.ToListCliString($"Found {providers.Count} task provider(s):", excludedFields: new string[]
                {
                    "AdditionalConfigs"
                });
        }
    }
}
