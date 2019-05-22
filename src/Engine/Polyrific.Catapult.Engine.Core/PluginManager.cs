// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polyrific.Catapult.TaskProviders.Core;
using Polyrific.Catapult.Shared.Common;

namespace Polyrific.Catapult.Engine.Core
{
    public class PluginManager : IPluginManager
    {
        private Dictionary<string, List<PluginItem>> _plugins;

        private readonly IPluginProcess _pluginProcess;

        private readonly ILogger _logger;

        public List<string> TaskProviderLocations { get; }

        public PluginManager(Dictionary<string, List<PluginItem>> plugins, ICatapultEngineConfig engineConfig, IPluginProcess pluginProcess, ILogger<PluginManager> logger)
        {
            _plugins = plugins;

            TaskProviderLocations = new List<string>()
            {
                engineConfig.TaskProvidersLocation
            };

            _pluginProcess = pluginProcess;

            _logger = logger;
        }

        public PluginManager(ICatapultEngineConfig engineConfig, IPluginProcess pluginProcess, ILogger<PluginManager> logger)
        {
            TaskProviderLocations = new List<string>()
            {
                engineConfig.TaskProvidersLocation
            };

            _pluginProcess = pluginProcess;

            _logger = logger;
        }
        
        public void AddPluginLocation(string location)
        {
            TaskProviderLocations.Add(location);
        }

        public PluginItem GetPlugin(string providerType, string name)
        {
            if (_plugins != null && _plugins.ContainsKey(providerType))
            {
                return _plugins[providerType].FirstOrDefault(p => p.Name == name);
            }

            return null;
        }

        public List<PluginItem> GetPlugins(string providerType)
        {
            if (_plugins != null && _plugins.ContainsKey(providerType))
            {
                return _plugins[providerType];
            }

            return new List<PluginItem>();
        }

        public void RefreshPlugins()
        {
            _plugins = new Dictionary<string, List<PluginItem>>();

            foreach (var location in TaskProviderLocations)
            {
                var files = new List<string>();
                
                var dllFiles = Directory.GetFiles(location, "*.dll", SearchOption.AllDirectories);
                if (dllFiles.Length > 0)
                    files.AddRange(dllFiles);
                
                var exeFiles = Directory.GetFiles(location, "*.exe", SearchOption.AllDirectories);
                if (exeFiles.Length > 0)
                    files.AddRange(exeFiles);

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
                                var instance = (TaskProvider)info.CreateInstance(type, false, BindingFlags.ExactBinding, null, new object[] { new string[0] }, null, null);
                                if (instance != null)
                                {
                                    var startFile = file;

                                    // if .dll, check if .exe file exists
                                    if (file.EndsWith(".dll") && files.Contains(file.Replace(".dll", ".exe")))
                                    {
                                        startFile = file.Replace(".dll", ".exe");
                                    }

                                    if (!_plugins.ContainsKey(instance.Type))
                                        _plugins.Add(instance.Type,
                                            new List<PluginItem>
                                                {new PluginItem(instance.Name, startFile, instance.RequiredServices)});
                                    else
                                        _plugins[instance.Type]
                                            .Add(new PluginItem(instance.Name, startFile, instance.RequiredServices));
                                }
                            }
                        }
                    }
                    catch (BadImageFormatException)
                    {
                        // skip loading file if this happen
                        _logger.LogDebug("Failed loading {file} as plugin file.", file);
                    }
                }
            }
        }

        public async Task<Dictionary<string, object>> InvokeTaskProvider(string pluginStartFile, string pluginArgs, string securedPluginArgs = null)
        {
            var result = new Dictionary<string, object>();
            
            var isExeFile = pluginStartFile.EndsWith(".exe", StringComparison.InvariantCultureIgnoreCase);

            var fileName = "dotnet";
            if (isExeFile)
                fileName = pluginStartFile;
            
            var temporaryFile = SaveArgumentFile(fileName, pluginArgs);

            var argumentFile = $"--file \"{temporaryFile}\"";
            argumentFile = $"{argumentFile} {(Debugger.IsAttached ? "--attach" : "")}";

            var securedArguments = $"\"\"{securedPluginArgs}\" {(Debugger.IsAttached ? "--attach" : "")}";
            if (!isExeFile)
            {
                argumentFile = $"\"{pluginStartFile}\" " + argumentFile;
                securedArguments = $"\"{pluginStartFile}\" " + securedArguments;
            }

            var startInfo = new ProcessStartInfo()
            {
                FileName = fileName,
                Arguments = argumentFile,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (var process = _pluginProcess.Start(startInfo))
            {
                if (process != null)
                {
                    if (!string.IsNullOrEmpty(securedPluginArgs))
                    {
                        _logger.LogDebug($"[PluginManager] Command: {fileName} {securedArguments}");
                    }                        

                    var reader = _pluginProcess.GetStandardOutput(process);
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

                    var error = await _pluginProcess.GetStandardError(process)?.ReadToEndAsync();
                    if (!string.IsNullOrEmpty(error))
                    {
                        result["errorMessage"] = error;
                    }                    
                }
            }

            File.Delete(temporaryFile);

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

        private string SaveArgumentFile(string pluginFile, string arguments)
        {
            var directory = Path.GetDirectoryName(pluginFile);
            var tempFile = Path.Combine(directory, "temp.json");

            File.WriteAllText(tempFile, arguments);

            return tempFile;
        }
    }
}
