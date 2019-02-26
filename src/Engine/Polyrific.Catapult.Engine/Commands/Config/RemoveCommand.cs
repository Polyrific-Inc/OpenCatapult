// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polyrific.Catapult.Engine.Core;
using System.Collections.Generic;
using System.Linq;

namespace Polyrific.Catapult.Engine.Commands.Config
{
    [Command("remove", Description = "Remove configurations")]
    public class RemoveCommand : BaseCommand
    {
        private readonly ICatapultEngineConfig _engineConfig;

        public RemoveCommand(ICatapultEngineConfig engineConfig, IConsole console, ILogger<RemoveCommand> logger) : base(console, logger)
        {
            _engineConfig = engineConfig;
        }

        [Option("-a|--all", "Remove all configuration items", CommandOptionType.NoValue)]
        public bool RemoveAll { get; set; }

        [Option("-n|--name", "Name of the configuration item", CommandOptionType.SingleValue)]
        public string ConfigName { get; set; }

        public override string Execute()
        {
            _engineConfig.Load().Wait();

            if (RemoveAll)
            {
                var configKeys = _engineConfig.Configs.Keys.ToList();
                var removedConfigs = new List<string>();
                foreach (var key in configKeys)
                {
                    _engineConfig.RemoveValue(key);
                    removedConfigs.Add(key);
                }

                _engineConfig.Save().Wait();
                
                Logger.LogInformation($"Config values have been removed: {JsonConvert.SerializeObject(removedConfigs)}");

                return "Config values have been removed successfully.";
            }

            if (string.IsNullOrEmpty(ConfigName)) 
                return "";

            _engineConfig.RemoveValue(ConfigName);
            _engineConfig.Save().Wait();

            var message = $"Config \"{ConfigName}\" has been removed.";
            Logger.LogInformation(message);

            return message;
        }
    }
}
