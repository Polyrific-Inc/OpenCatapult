// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Engine.Core
{
    public interface IPluginManager
    {
        /// <summary>
        /// Plugin locations
        /// </summary>
        List<string> PluginLocations { get; }

        /// <summary>
        /// Add new location of the plugin
        /// </summary>
        /// <param name="location"></param>
        void AddPluginLocation(string location);

        /// <summary>
        /// Get plugins by the task type
        /// </summary>
        /// <param name="taskType"></param>
        /// <returns></returns>
        List<PluginItem> GetPlugins(string taskType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        PluginItem GetPlugin(string taskType, string name);

        /// <summary>
        /// Refresh plugin collection
        /// </summary>
        void RefreshPlugins();

        /// <summary>
        /// Invoke task provider
        /// </summary>
        /// <param name="pluginDll">Path to the task provider dll file</param>
        /// <param name="pluginArgs">Arguments to be passed when executing task provider</param>
        /// <param name="securedPluginArgs">Arguments that has been stripped from secret values, to be shown in UI for traceability</param>
        /// <returns></returns>
        Task<Dictionary<string, object>> InvokeTaskProvider(string pluginDll, string pluginArgs, string securedPluginArgs = null);
    }
}
