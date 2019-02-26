// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Polyrific.Catapult.Cli.Commands.Config
{
    [Command("get", Description = "Get config value")]
    public class GetCommand : BaseCommand
    {
        private readonly ICliConfig _cliConfig;

        public GetCommand(ICliConfig cliConfig, IConsole console, ILogger<GetCommand> logger)
            : base(console, logger)
        {
            _cliConfig = cliConfig;
        }

        [Option("-n|--name <NAME>", "Name of the config", CommandOptionType.SingleValue)]
        public string ConfigName { get; set; }

        public override string Execute()
        {
            var message = "";

            _cliConfig.Load().Wait();

            if (!string.IsNullOrEmpty(ConfigName))
            {
                var value = _cliConfig.GetValue(ConfigName);
                if (!string.IsNullOrEmpty(value))
                    message = $"{ConfigName}: {value}";
            }
            else
            {
                var sb = new StringBuilder();
                sb.AppendLine("Available CLI configs:");
                foreach (var key in _cliConfig.Configs.Keys)
                {
                    sb.AppendLine($"- {key}: {_cliConfig.Configs[key]}");
                }

                message = sb.ToString();
            }

            return message;
        }
    }
}
