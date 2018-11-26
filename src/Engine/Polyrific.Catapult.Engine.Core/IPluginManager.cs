// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Engine.Core
{
    public interface IPluginManager
    {
        /// <summary>
        /// Refresh plugin collection
        /// </summary>
        void RefreshPlugins();

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
        /// Invoke task provider
        /// </summary>
        /// <param name="pluginDll">Path to the task provider dll file</param>
        /// <param name="pluginArgs">Arguments to be passed when executing task provider</param>
        /// <returns></returns>
        Task<Dictionary<string, object>> InvokeTaskProvider(string pluginDll, string pluginArgs);
    }
}
