// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Engine.Core
{
    public interface ICatapultEngineConfig
    {
        /// <summary>
        /// Get API URL value
        /// </summary>
        string ApiUrl { get; }

        /// <summary>
        /// Get API request timeout value
        /// </summary>
        TimeSpan ApiRequestTimeout { get; }

        /// <summary>
        /// Get Authorization Token value
        /// </summary>
        string AuthorizationToken { get; }

        /// <summary>
        /// Get Job Checking Interval value
        /// </summary>
        int JobCheckingInterval { get; }

        /// <summary>
        /// Location of the plugins folder
        /// </summary>
        string PluginsLocation { get; }

        /// <summary>
        /// Get available configs
        /// </summary>
        Dictionary<string, string> Configs { get; }

        /// <summary>
        /// Load config values
        /// </summary>
        Task Load();

        /// <summary>
        /// Save config values
        /// </summary>
        Task Save();

        /// <summary>
        /// Get a config value
        /// </summary>
        /// <param name="configName">Name of the config item</param>
        /// <returns>The config value</returns>
        string GetValue(string configName);

        /// <summary>
        /// Get a config value, or return default value if not exist
        /// </summary>
        /// <param name="configName">Name of the config item</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>The config value</returns>
        string GetValueOrDefault(string configName, string defaultValue);

        /// <summary>
        /// Set value of a config item
        /// </summary>
        /// <param name="configName">Name of the config item</param>
        /// <param name="configValue">Value of the config item</param>
        void SetValue(string configName, string configValue);
        
        /// <summary>
        /// Remove a config value
        /// </summary>
        /// <param name="configName">Name of the config item</param>
        void RemoveValue(string configName);
    }
}