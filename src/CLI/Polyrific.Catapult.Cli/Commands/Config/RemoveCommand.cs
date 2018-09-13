// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

        public override string Execute()
        {
            _cliConfig.Load();

            if (RemoveAll)
            {
                var configKeys = _cliConfig.Configs.Keys.ToList();
                var removedConfigs = new List<string>();
                foreach (var key in configKeys)
                {
                    _cliConfig.RemoveValue(key);
                    removedConfigs.Add(key);
                }

                _cliConfig.Save();

                Logger.LogInformation($"Config values have been removed: {JsonConvert.SerializeObject(removedConfigs)}");

                return "Config values have been removed successfully.";
            }

            if (string.IsNullOrEmpty(ConfigName))
                return "";

            _cliConfig.RemoveValue(ConfigName);
            _cliConfig.Save();

            var message = $"Config \"{ConfigName}\" has been removed.";
            Logger.LogInformation(message);

            return message;
        }
    }
}
