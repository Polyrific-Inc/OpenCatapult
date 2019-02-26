// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Reflection;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "Check version of the components")]
    public class VersionCommand : BaseCommand
    {
        private readonly IVersionService _versionService;

        public VersionCommand(IConsole console, ILogger<VersionCommand> logger, IVersionService versionService) : base(console, logger)
        {
            _versionService = versionService;
        }

        public override string Execute()
        {
            Console.WriteLine("Checking version of the components...");

            var versionResult = _versionService.GetApiVersion().Result;

            var sb = new StringBuilder();
            sb.AppendLine($"API Version: {versionResult.ApiVersion}");
            sb.AppendLine($"CLI Version: {GetCliVersion()}");

            if (versionResult.Engines?.Count > 0)
            {
                sb.AppendLine($"Registered Engines:");
                foreach (var engine in versionResult.Engines)
                    sb.AppendLine($"  - {engine.Name}: {engine.Version ?? "n/a"}");
            }

            if (versionResult.Providers?.Count > 0)
            {
                sb.AppendLine($"Registered Task Providers:");
                foreach (var provider in versionResult.Providers)
                    sb.AppendLine($"  - {provider.Name}: {provider.Version}");
            }

            return sb.ToString();
        }

        private string GetCliVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        }
    }
}
