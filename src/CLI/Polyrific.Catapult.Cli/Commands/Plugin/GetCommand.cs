// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Plugin
{
    [Command(Description = "Get a single plugin details")]
    public class GetCommand : BaseCommand
    {
        private readonly IPluginService _pluginService;

        public GetCommand(IPluginService pluginService, IConsole console, ILogger<GetCommand> logger) : base(console, logger)
        {
            _pluginService = pluginService;
        }

        [Option("-n|--name", "Name of the plugin", CommandOptionType.SingleValue)]
        public string PluginName { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to get plugin {PluginName}...");
            var plugin = _pluginService.GetPluginByName(PluginName).Result;
            return plugin == null ? $"Plugin {PluginName} was not found." : plugin.ToCliString($"Plugin {PluginName}:");
        }
    }
}
