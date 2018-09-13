// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polyrific.Catapult.Engine.Core;

namespace Polyrific.Catapult.Engine.Commands.Config
{
    [Command(Description = "Import configuration from a config file")]
    public class ImportCommand : BaseCommand
    {
        private readonly ICatapultEngineConfig _engineConfig;

        public ImportCommand(ICatapultEngineConfig engineConfig, IConsole console, ILogger<ImportCommand> logger) : base(console, logger)
        {
            _engineConfig = engineConfig;
        }

        [Required]
        [Option("-f|--file", "Full path of the file", CommandOptionType.SingleValue)]
        public string InputFile { get; set; }

        public override string Execute()
        {
            if (!File.Exists(InputFile))
                return $"File \"{InputFile}\" could not be found.";
                
            _engineConfig.Load();

            var obj = JObject.Parse(File.ReadAllText(InputFile));
            var configs = obj.ToObject<Dictionary<string, string>>();

            var addedConfigs = new Dictionary<string, string>();
            var modifiedConfigs = new Dictionary<string, string>();
            foreach (var key in configs.Keys)
            {
                if (_engineConfig.Configs.ContainsKey(key))
                    modifiedConfigs.Add(key, configs[key]);
                else
                    addedConfigs.Add(key, configs[key]);

                _engineConfig.SetValue(key, configs[key]);
            }

            _engineConfig.Save();
            
            Logger.LogInformation($"Modified configs: {JsonConvert.SerializeObject(modifiedConfigs)}. Added configs: {JsonConvert.SerializeObject(addedConfigs)}");

            var sb = new StringBuilder("Configs have been imported.");

            if (modifiedConfigs.Any())
            {
                sb.AppendLine("- Modified:");
                foreach (var key in modifiedConfigs.Keys)
                {
                    sb.AppendLine($"  - {key}: {modifiedConfigs[key]}");
                }
            }

            if (addedConfigs.Any())
            {
                sb.AppendLine("- Added:");
                foreach (var key in addedConfigs.Keys)
                {
                    sb.AppendLine($"  - {key}: {addedConfigs[key]}");
                }
            }

            return sb.ToString();
        }
    }
}