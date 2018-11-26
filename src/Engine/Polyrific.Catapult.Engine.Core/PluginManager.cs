﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Core;

namespace Polyrific.Catapult.Engine.Core
{
    public class PluginManager : IPluginManager
    {
        private Dictionary<string, List<PluginItem>> _plugins;

        private readonly ILogger _logger;

        private readonly List<string> _pluginLocations;

        public PluginManager(ICatapultEngineConfig engineConfig, ILogger<PluginManager> logger)
        {
            _pluginLocations = new List<string>()
            {
                engineConfig.PluginsLocation
            };

            _logger = logger;
        }
        
        public void AddPluginLocation(string location)
        {
            _pluginLocations.Add(location);
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

            foreach (var location in _pluginLocations)
            {
                var files = Directory.GetFiles(location, "*.dll", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    try
                    {
                        var info = Assembly.LoadFile(file);
                        if (info.EntryPoint != null && typeof(TaskProvider).IsAssignableFrom(info.EntryPoint.DeclaringType))
                        {
                            var type = info.EntryPoint.DeclaringType.FullName;
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
    }
}