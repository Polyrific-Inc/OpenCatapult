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
    public class TaskProviderManager : ITaskProviderManager
    {
        private Dictionary<string, List<TaskProviderItem>> _taskProviders;

        private readonly ITaskProviderProcess _taskProviderProcess;

        private readonly ILogger _logger;

        public List<string> TaskProviderLocations { get; }

        public TaskProviderManager(Dictionary<string, List<TaskProviderItem>> taskProviders, ICatapultEngineConfig engineConfig, ITaskProviderProcess taskProviderProcess, ILogger<TaskProviderManager> logger)
        {
            _taskProviders = taskProviders;

            TaskProviderLocations = new List<string>()
            {
                engineConfig.TaskProvidersLocation
            };

            _taskProviderProcess = taskProviderProcess;

            _logger = logger;
        }

        public TaskProviderManager(ICatapultEngineConfig engineConfig, ITaskProviderProcess taskProviderProcess, ILogger<TaskProviderManager> logger)
        {
            TaskProviderLocations = new List<string>()
            {
                engineConfig.TaskProvidersLocation
            };

            _taskProviderProcess = taskProviderProcess;

            _logger = logger;
        }
        
        public void AddTaskProviderLocation(string location)
        {
            TaskProviderLocations.Add(location);
        }

        public TaskProviderItem GetTaskProvider(string providerType, string name)
        {
            if (_taskProviders != null && _taskProviders.ContainsKey(providerType))
            {
                return _taskProviders[providerType].FirstOrDefault(p => p.Name == name);
            }

            return null;
        }

        public List<TaskProviderItem> GetTaskProviders(string providerType)
        {
            if (_taskProviders != null && _taskProviders.ContainsKey(providerType))
            {
                return _taskProviders[providerType];
            }

            return new List<TaskProviderItem>();
        }

        public void RefreshTaskProviders()
        {
            _taskProviders = new Dictionary<string, List<TaskProviderItem>>();

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

                                    if (!_taskProviders.ContainsKey(instance.Type))
                                        _taskProviders.Add(instance.Type,
                                            new List<TaskProviderItem>
                                                {new TaskProviderItem(instance.Name, startFile, instance.RequiredServices)});
                                    else
                                        _taskProviders[instance.Type]
                                            .Add(new TaskProviderItem(instance.Name, startFile, instance.RequiredServices));
                                }
                            }
                        }
                    }
                    catch (BadImageFormatException)
                    {
                        // skip loading file if this happen
                        _logger.LogDebug("Failed loading {file} as task provider file.", file);
                    }
                }
            }
        }

        public async Task<Dictionary<string, object>> InvokeTaskProvider(string taskProviderStartFile, string taskProviderArgs, string securedTaskProviderArgs = null)
        {
            var result = new Dictionary<string, object>();
            
            var isExeFile = taskProviderStartFile.EndsWith(".exe", StringComparison.InvariantCultureIgnoreCase);

            var fileName = "dotnet";
            if (isExeFile)
                fileName = taskProviderStartFile;
            
            var temporaryFile = SaveArgumentFile(fileName, taskProviderArgs);

            var argumentFile = $"--file \"{temporaryFile}\"";
            argumentFile = $"{argumentFile} {(Debugger.IsAttached ? "--attach" : "")}";

            var securedArguments = $"\"\"{securedTaskProviderArgs}\" {(Debugger.IsAttached ? "--attach" : "")}";
            if (!isExeFile)
            {
                argumentFile = $"\"{taskProviderStartFile}\" " + argumentFile;
                securedArguments = $"\"{taskProviderStartFile}\" " + securedArguments;
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

            using (var process = _taskProviderProcess.Start(startInfo))
            {
                if (process != null)
                {
                    if (!string.IsNullOrEmpty(securedTaskProviderArgs))
                    {
                        _logger.LogDebug($"[TaskProviderManager] Command: {fileName} {securedArguments}");
                    }                        

                    var reader = _taskProviderProcess.GetStandardOutput(process);
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

                    var error = await _taskProviderProcess.GetStandardError(process)?.ReadToEndAsync();
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

        private string SaveArgumentFile(string taskProviderFile, string arguments)
        {
            var directory = Path.GetDirectoryName(taskProviderFile);
            var tempFile = Path.Combine(directory, "temp.json");

            File.WriteAllText(tempFile, arguments);

            return tempFile;
        }
    }
}
