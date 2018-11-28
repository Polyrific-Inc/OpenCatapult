﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polyrific.Catapult.Plugins.Core;
using Polyrific.Catapult.Shared.Common;

namespace Polyrific.Catapult.Engine.Core
{
    public class PluginManager : IPluginManager
    {
        private Dictionary<string, List<PluginItem>> _plugins;

        private readonly ILogger _logger;

        public List<string> PluginLocations { get; }

        public PluginManager(Dictionary<string, List<PluginItem>> plugins, ICatapultEngineConfig engineConfig, ILogger<PluginManager> logger)
        {
            _plugins = plugins;

            PluginLocations = new List<string>()
            {
                engineConfig.PluginsLocation
            };

            _logger = logger;
        }

        public PluginManager(ICatapultEngineConfig engineConfig, ILogger<PluginManager> logger)
        {
            PluginLocations = new List<string>()
            {
                engineConfig.PluginsLocation
            };

            _logger = logger;
        }
        
        public void AddPluginLocation(string location)
        {
            PluginLocations.Add(location);
        }

        public PluginItem GetPlugin(string taskType, string name)
        {
            if (_plugins != null && _plugins.ContainsKey(taskType))
            {
                return _plugins[taskType].FirstOrDefault(p => p.Name == name);
            }

            return null;
        }

        public List<PluginItem> GetPlugins(string taskType)
        {
            if (_plugins != null && _plugins.ContainsKey(taskType))
            {
                return _plugins[taskType];
            }

            return new List<PluginItem>();
        }

        public void RefreshPlugins()
        {
            _plugins = new Dictionary<string, List<PluginItem>>();

            foreach (var location in PluginLocations)
            {
                var files = Directory.GetFiles(location, "*.dll", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    try
                    {
                        var info = Assembly.LoadFile(file);
                        if (info.EntryPoint != null && typeof(TaskProvider).IsAssignableFrom(info.EntryPoint.DeclaringType))
                        {
                            var type = info.EntryPoint.DeclaringType?.FullName;
                            if (type != null)
                            {
                                var instance = (TaskProvider)info.CreateInstance(type, false, BindingFlags.ExactBinding, null, new object[] { }, null, null);
                                if (instance != null)
                                {
                                    if (!_plugins.ContainsKey(instance.Type))
                                        _plugins.Add(instance.Type,
                                            new List<PluginItem>
                                                {new PluginItem(instance.Name, file, instance.RequiredServices)});
                                    else
                                        _plugins[instance.Type]
                                            .Add(new PluginItem(instance.Name, file, instance.RequiredServices));
                                }
                            }
                        }
                    }
                    catch (System.BadImageFormatException ex)
                    {
                        // skip loading file if this happen
                        _logger.LogWarning(ex, "Failed loading plugin file {file}", file);
                    }
                }
            }
        }

        public async Task<Dictionary<string, object>> InvokeTaskProvider(string pluginDll, string pluginArgs)
        {
            var result = new Dictionary<string, object>();

            pluginArgs = pluginArgs.Replace("\"", "\\\"");

            var startInfo = new ProcessStartInfo()
            {
                FileName = "dotnet",
                Arguments = $"\"{pluginDll}\" \"{pluginArgs}\" {(Debugger.IsAttached ? "--attach" : "")}",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (var process = Process.Start(startInfo))
            {
                if (process != null)
                {
                    // TODO: We need to think a way to print the command without exposing secret values
                    //Console.WriteLine($"[Master] Command: {process.StartInfo.FileName} {process.StartInfo.Arguments}");

                    var reader = process.StandardOutput;
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();

                        var tags = line.GetPrefixTags();
                        if (tags.Length > 0 && tags[0] == "OUTPUT")
                        {
                            var outputString = line.Replace("[OUTPUT] ", "");
                            var outputDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(outputString);
                            foreach (var key in outputDict.Keys)
                            {
                                if (!result.ContainsKey(key))
                                    result.Add(key, outputDict[key]);
                            }
                        } else if (tags.Length > 0 && tags[0] == "LOG")
                        {
                            SubmitLog(line.Replace("[LOG]", ""));
                        }
                    }
                }
            }

            return result;
        }

        private void SubmitLog(string logMessage)
        {
            var tags = logMessage.GetPrefixTags();
            if (tags.Length > 0)
            {
                switch (tags[0])
                {
                    case "Critical":
                        _logger.LogCritical(logMessage.Replace("[Critical]", ""));
                        break;
                    case "Error":
                        _logger.LogError(logMessage.Replace("[Error]", ""));
                        break;
                    case "Warning":
                        _logger.LogWarning(logMessage.Replace("[Warning]", ""));
                        break;
                    case "Information":
                        _logger.LogInformation(logMessage.Replace("[Information]", ""));
                        break;
                    case "Debug":
                        _logger.LogDebug(logMessage.Replace("[Debug]", ""));
                        break;
                    case "Trace":
                        _logger.LogTrace(logMessage.Replace("[Trace]", ""));
                        break;
                }
            }
        }
    }
}
