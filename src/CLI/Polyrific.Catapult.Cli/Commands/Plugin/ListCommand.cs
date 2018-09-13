// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Plugin
{
    [Command(Description = "List registered plugins")]
    public class ListCommand : BaseCommand
    {
        private readonly IPluginService _pluginService;

        public ListCommand(IPluginService pluginService, IConsole console, ILogger<ListCommand> logger) : base(console, logger)
        {
            _pluginService = pluginService;
        }

        [Option("-t|--type", "Type of the plugin", CommandOptionType.SingleOrNoValue)]
        [AllowedValues(
            "all",
            Shared.Dto.Constants.PluginType.BuildProvider,
            Shared.Dto.Constants.PluginType.DeployProvider,
            Shared.Dto.Constants.PluginType.GeneratorProvider,
            Shared.Dto.Constants.PluginType.RepositoryProvider)]
        public string PluginType { get; set; }

        public override string Execute()
        {
            if (string.IsNullOrEmpty(PluginType))
                PluginType = "all";

            var plugins = _pluginService.GetPlugins(PluginType).Result;
            if (!plugins.Any())
                return PluginType == "all" ? "No registered plugins found." : $"No registered plugins with type {PluginType} found.";
            
            return plugins.ToListCliString("Registered plugins:");
        }
    }
}
