// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polyrific.Catapult.Cli.Extensions;

namespace Polyrific.Catapult.Cli.Commands.Config
{
    [Command(Description = "Set config value")]
    public class SetCommand : BaseCommand
    {
        private readonly ICliConfig _cliConfig;

        public SetCommand(ICliConfig cliConfig, IConsole console, ILogger<SetCommand> logger)
            : base(console, logger)
        {
            _cliConfig = cliConfig;
        }

        [Option("-n|--name <NAME>", "Name of the config", CommandOptionType.SingleValue)]
        public string ConfigName { get; set; }

        [Option("-v|--value <VALUE>", "Value of the config", CommandOptionType.SingleValue)]
        public string ConfigValue { get; set; }

        public override string Execute()
        {
            var message = "";

            _cliConfig.Load().Wait();

            if (!string.IsNullOrEmpty(ConfigName) && !string.IsNullOrEmpty(ConfigValue))
            {

                _cliConfig.SetValue(ConfigName, ConfigValue);
                _cliConfig.Save().Wait();

                message = $"The value of \"{ConfigName}\" has been set to \"{ConfigValue}\".";
                Logger.LogInformation(message);
            }
            else if (!string.IsNullOrEmpty(ConfigName) || !string.IsNullOrEmpty(ConfigValue))
            {
                return message;
            }
            else
            {
                Console.WriteLine("Please enter the value for each config item below, or press ENTER if no changes needed:");

                var configKeys = _cliConfig.Configs.Keys.ToList();
                var modifiedConfigs = new Dictionary<string, string>();
                foreach (var key in configKeys)
                {
                    var isValueNeeded = true;
                    while (isValueNeeded)
                    {
                        System.Console.SetIn(new StreamReader(System.Console.OpenStandardInput(8192)));
                        var value = Console.GetString($"- {key} ({_cliConfig.GetValueOrDefault(key, "")}): ");

                        try
                        {
                            if (!string.IsNullOrEmpty(value))
                            {
                                _cliConfig.SetValue(key, value);
                                modifiedConfigs.Add(key, value);
                            }

                            isValueNeeded = false;
                        }
                        catch (Exception)
                        {
                            // do nothing, just repeat the input
                        }
                    }
                }

                _cliConfig.Save().Wait();

                message = "Config values have been saved successfully.";
                Logger.LogInformation($"Config values have been modified: {JsonConvert.SerializeObject(modifiedConfigs)}");
            }

            return message;
        }
    }
}
