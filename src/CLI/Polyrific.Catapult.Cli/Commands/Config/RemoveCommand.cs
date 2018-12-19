// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polyrific.Catapult.Cli.Extensions;

namespace Polyrific.Catapult.Cli.Commands.Config
{
    [Command(Description = "Remove configurations")]
    public class RemoveCommand : BaseCommand
    {
        private readonly ICliConfig _cliConfig;

        public RemoveCommand(ICliConfig cliConfig, IConsole console, ILogger<RemoveCommand> logger) : base(console, logger)
        {
            _cliConfig = cliConfig;
        }

        [Option("-a|--all", "Remove all configuration items", CommandOptionType.NoValue)]
        public bool RemoveAll { get; set; }

        [Option("-n|--name", "Name of the configuration item", CommandOptionType.SingleValue)]
        public string ConfigName { get; set; }

        [Option("-ac|--autoconfirm", "Execute the command without the need of confirmation prompt", CommandOptionType.NoValue)]
        public bool AutoConfirm { get; set; }

        public override string Execute()
        {
            _cliConfig.Load().Wait();

            if (RemoveAll && (AutoConfirm || Console.GetYesNo($"Are you sure you want to remove all configs?", false)))
            {
                var configKeys = _cliConfig.Configs.Keys.ToList();
                var removedConfigs = new List<string>();
                foreach (var key in configKeys)
                {
                    _cliConfig.RemoveValue(key);
                    removedConfigs.Add(key);
                }

                _cliConfig.Save().Wait();

                Logger.LogInformation($"Config values have been removed: {JsonConvert.SerializeObject(removedConfigs)}");

                return "Config values have been removed successfully.";
            }

            if (string.IsNullOrEmpty(ConfigName))
                return "";

            string message = string.Empty;
            if (AutoConfirm || Console.GetYesNo($"Are you sure you want to remove config \"{ConfigName}\"?", false))
            {
                _cliConfig.RemoveValue(ConfigName);
                _cliConfig.Save().Wait();

                message = $"Config \"{ConfigName}\" has been removed.";
                Logger.LogInformation(message);
            }

            return message;
        }
    }
}
