﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.Plugin;
using Polyrific.Catapult.Shared.Service;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Polyrific.Catapult.Cli.Commands.Provider
{
    [Command(Description = "Register a new task provider")]
    public class RegisterCommand : BaseCommand
    {
        private readonly IPluginService _pluginService;

        public RegisterCommand(IPluginService pluginService, IConsole console, ILogger<RegisterCommand> logger) : base(console, logger)
        {
            _pluginService = pluginService;
        }

        [Option("-f|--file", "Task provider metadata yaml file", CommandOptionType.SingleValue)]
        public string MetadataFile { get; set; }

        public override string Execute()
        {
            Console.WriteLine("Trying to register the task provider...");

            if (!File.Exists(MetadataFile))
                return $"Could not find \"{MetadataFile}\".";

            var metadataContent = File.ReadAllText(MetadataFile);
            var plugin = DeserializeYaml<NewPluginDto>(metadataContent);
            if (plugin == null)
                return "Task provider metadata could not be parsed from the file content.";
            
            var _ = _pluginService.AddPlugin(plugin).Result;

            var message = $"Task provider {plugin.Name} (v{plugin.Version}) by {plugin.Author} has been registered successfully.";
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
