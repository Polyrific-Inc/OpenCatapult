// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Plugin
{
    [Command(Description = "Remove a plugin registration")]
    public class RemoveCommand : BaseCommand
    {
        private readonly IPluginService _pluginService;

        public RemoveCommand(IPluginService pluginService, IConsole console, ILogger<RemoveCommand> logger) : base(console, logger)
        {
            _pluginService = pluginService;
        }

        [Option("-n|--name", "Name of the plugin", CommandOptionType.SingleValue)]
        public string PluginName { get; set; }

        [Option("-ac|--autoconfirm", "Execute the command without the need of confirmation prompt", CommandOptionType.NoValue)]
        public bool AutoConfirm { get; set; }

        public override string Execute()
        {
            if (!(AutoConfirm || Console.GetYesNo($"Are you sure you want to remove plugin {PluginName}?", false)))
                return string.Empty;

            Console.WriteLine($"Trying to remove plugin \"{PluginName}\"...");

            var plugin = _pluginService.GetPluginByName(PluginName).Result;
            if (plugin == null)
                return $"Plugin {PluginName} was not found.";

            _pluginService.DeletePlugin(plugin.Id).Wait();

            var message = $"Plugin {PluginName} has been removed.";
            Logger.LogInformation(message);

            return message;
        }
    }
}
