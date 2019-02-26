// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Engine.Core;

namespace Polyrific.Catapult.Engine.Commands.Config
{
    [Command("get", Description = "Get config value")]
    public class GetCommand : BaseCommand
    {
        private readonly ICatapultEngineConfig _engineConfig;
        
        public GetCommand(ICatapultEngineConfig engineConfig, IConsole console, ILogger<GetCommand> logger)
            : base(console, logger)
        {
            _engineConfig = engineConfig;
        }
        
        [Option("-n|--name <NAME>", "Name of the config", CommandOptionType.SingleValue)]
        public string ConfigName { get; set; }
        
        public override string Execute()
        {
            var message = "";
            
            _engineConfig.Load().Wait();

            if (!string.IsNullOrEmpty(ConfigName))
            {
                var value = _engineConfig.GetValue(ConfigName);
                if (!string.IsNullOrEmpty(value))
                    message = $"{ConfigName}: {value}";
            }
            else
            {
                var sb = new StringBuilder();
                sb.AppendLine("Available Engine configs:");
                foreach (var key in _engineConfig.Configs.Keys)
                {
                    sb.AppendLine($"- {key}: {_engineConfig.Configs[key]}");
                }

                message = sb.ToString();
            }

            return message;
        }
    }
}
