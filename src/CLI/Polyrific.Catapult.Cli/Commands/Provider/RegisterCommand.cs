// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.Provider;
using Polyrific.Catapult.Shared.Service;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Polyrific.Catapult.Cli.Commands.Provider
{
    [Command("register", Description = "Register a new task provider")]
    public class RegisterCommand : BaseCommand
    {
        private readonly IProviderService _providerService;

        public RegisterCommand(IProviderService providerService, IConsole console, ILogger<RegisterCommand> logger) : base(console, logger)
        {
            _providerService = providerService;
        }

        [Option("-f|--file", "Task provider metadata yaml file", CommandOptionType.SingleValue)]
        public string MetadataFile { get; set; }

        public override string Execute()
        {
            Console.WriteLine("Trying to register the task provider...");

            if (!File.Exists(MetadataFile))
                return $"Could not find \"{MetadataFile}\".";

            var metadataContent = File.ReadAllText(MetadataFile);
            var provider = DeserializeYaml<NewProviderDto>(metadataContent);
            if (provider == null)
                return "Task provider metadata could not be parsed from the file content.";
            
            var _ = _providerService.AddProvider(provider).Result;

            var message = $"Task provider {provider.Name} (v{provider.Version}) by {provider.Author} has been registered successfully.";
            Logger.LogInformation(message);

            return message;
        }

        private T DeserializeYaml<T>(string templateYaml)
        {
            var deserializer = new DeserializerBuilder().WithNamingConvention(new HyphenatedNamingConvention()).IgnoreUnmatchedProperties().Build();
            return deserializer.Deserialize<T>(templateYaml);
        }
    }
}
