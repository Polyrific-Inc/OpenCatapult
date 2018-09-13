﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polyrific.Catapult.Engine.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Polyrific.Catapult.Engine.Extensions;

namespace Polyrific.Catapult.Engine.Commands.Config
{
    [Command(Description = "Set config value")]
    public class SetCommand : BaseCommand
    {
        private readonly ICatapultEngineConfig _engineConfig;
        
        public SetCommand(ICatapultEngineConfig engineConfig, IConsole console, ILogger<SetCommand> logger)
            : base(console, logger)
        {
            _engineConfig = engineConfig;
        }

        [Option("-a|--all", "Set all configurations", CommandOptionType.NoValue)]
        public bool SetAll { get; set; }
        
        [Option("-n|--name <NAME>", "Name of the config", CommandOptionType.SingleValue)]
        public string ConfigName { get; set; }
        
        [Option("-v|--value <VALUE>", "Value of the config", CommandOptionType.SingleValue)]
        public string ConfigValue { get; set; }

        public override string Execute()
        {
            var message = "";

            _engineConfig.Load();

            if (SetAll)
            {
                Console.WriteLine("Please enter the value for each config item below, or press ENTER if no changes needed:");

                var configKeys = _engineConfig.Configs.Keys.ToList();
                var modifiedConfigs = new Dictionary<string, string>();
                foreach (var key in configKeys)
                {
                    var isValueNeeded = true;
                    while (isValueNeeded)
                    {
                        System.Console.SetIn(new StreamReader(System.Console.OpenStandardInput(8192)));
                        var value = Console.GetString($"- {key} ({_engineConfig.GetValueOrDefault(key, "")}): ");

                        try
                        {
                            if (!string.IsNullOrEmpty(value))
                            {
                                _engineConfig.SetValue(key, value);
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

                _engineConfig.Save();

                message = "Config values have been saved successfully.";
                Logger.LogInformation($"Config values have been modified: {JsonConvert.SerializeObject(modifiedConfigs)}");
            }
            else if (!string.IsNullOrEmpty(ConfigName) && !string.IsNullOrEmpty(ConfigValue))
            {
            
                _engineConfig.SetValue(ConfigName, ConfigValue);
                _engineConfig.Save();

                message = $"The value of \"{ConfigName}\" has been set to \"{ConfigValue}\".";
                Logger.LogInformation(message);
            }

            return message;
        }
    }
}