// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polyrific.Catapult.Engine.Core.Exceptions;
using Polyrific.Catapult.Shared.Common;

namespace Polyrific.Catapult.Engine.Core
{
    public class CatapultEngineConfig : ICatapultEngineConfig
    {
        private static readonly string EngineConfigFile = Path.Combine(AppContext.BaseDirectory, "engineconfig.json");
        private readonly ILogger<CatapultEngineConfig> _logger;

        private Dictionary<string, string> _configs;

        public CatapultEngineConfig(ILogger<CatapultEngineConfig> logger)
        {
            _configs = new Dictionary<string, string>();
            _logger = logger;

            InitConfigFile(false, logger).Wait();

            Load().Wait();
        }

        public const string ApiUrlKey = "ApiUrl";
        public string ApiUrl => GetConfigValue(ApiUrlKey, "");

        public const string ApiRequestTimeoutKey = "ApiRequestTimeout";
        public TimeSpan ApiRequestTimeout => GetConfigTimespanValue(ApiRequestTimeoutKey, TimeSpan.FromMinutes(1));

        public const string AuthorizationTokenKey = "AuthorizationToken";
        public string AuthorizationToken => GetConfigValue(AuthorizationTokenKey, "");

        public const string JobCheckingIntervalKey = "JobCheckingInterval";
        public int JobCheckingInterval => GetConfigIntegerValue(JobCheckingIntervalKey, 30);

        public const string PluginsLocationKey = "PluginsLocation";
        public string PluginsLocation => GetConfigValue(PluginsLocationKey, Path.Combine(AppContext.BaseDirectory, "plugins"));

        public const string WorkingLocationKey = "WorkingLocation";
        public string WorkingLocation => GetConfigValue(WorkingLocationKey, Path.Combine(AppContext.BaseDirectory, "working"));
        
        public Dictionary<string, string> Configs => _configs;

        public async Task Load()
        {
            var obj = JObject.Parse(await FileHelper.ReadAllTextAsync(EngineConfigFile));
            _configs = obj["EngineConfig"].ToObject<Dictionary<string, string>>();

            // check against default config
            var defaultConfigs = GetDefaultConfigs();
            foreach (var conf in defaultConfigs)
            {
                if (!_configs.ContainsKey(conf.Key))
                    _configs.Add(conf.Key, conf.Value);
            }
        }

        public async Task Save()
        {
            _logger.LogInformation("Saving configs into config file..");
            await FileHelper.WriteAllTextAsync(EngineConfigFile, JsonConvert.SerializeObject(new { EngineConfig = _configs}));
        }

        public string GetValue(string configName)
        {
            return GetConfigValue(configName);
        }

        public string GetValueOrDefault(string configName, string defaultValue)
        {
            return GetConfigValue(configName, defaultValue);
        }

        public void SetValue(string configName, string configValue)
        {
            SetConfigValue(configName, configValue);
        }

        public void RemoveValue(string configName)
        {
            if (_configs.ContainsKey(configName))
            {
                var defaultConfigs = GetDefaultConfigs();
                if (defaultConfigs.ContainsKey(configName))
                {
                    _configs[configName] = defaultConfigs[configName];
                }
                else
                {
                    _configs.Remove(configName);
                }
            }
        }

        /// <summary>
        /// Initiate engine config file 
        /// </summary>
        /// <param name="reset">If <value>true</value>, the existing config file will be reset with the default values</param>
        /// <param name="logger">Logger provider</param>
        /// <returns></returns>
        public static async Task InitConfigFile(bool reset = false, ILogger<CatapultEngineConfig> logger = null)
        {
            if (reset && File.Exists(EngineConfigFile))
            {
                logger?.LogInformation($"Deleting config file \"{EngineConfigFile}\"");
                File.Delete(EngineConfigFile);
            }

            if (!File.Exists(EngineConfigFile))
            {
                logger?.LogInformation($"The config file \"{EngineConfigFile}\" doesn't exist. Creating one..");
                await FileHelper.WriteAllTextAsync(EngineConfigFile, JsonConvert.SerializeObject(new { EngineConfig = GetDefaultConfigs() }));
            }
        }

        /// <summary>
        /// Get config value, and throw <see cref="InvalidEngineConfigNameException"/> if not found.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetConfigValue(string key)
        {
            if (_configs.ContainsKey(key))
                return _configs[key];

            throw new InvalidEngineConfigNameException(key);
        }

        /// <summary>
        /// Get config value, and return default value if not found
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private string GetConfigValue(string key, string defaultValue)
        {
            return _configs.TryGetValue(key, out var sValue) ? sValue : defaultValue;
        }

        /// <summary>
        /// Get integer value from config, and return default value if not found
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private int GetConfigIntegerValue(string key, int defaultValue)
        {
            return int.TryParse(GetConfigValue(key, ""), out var result) ? result : defaultValue;
        }

        /// <summary>
        /// Get timespan value from config, and return default value if not found
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private TimeSpan GetConfigTimespanValue(string key, TimeSpan defaultValue)
        {
            return TimeSpan.TryParse(GetConfigValue(key, ""), out var result) ? result : defaultValue;
        }

        /// <summary>
        /// Update config value, or add new config if not found
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void SetConfigValue(string key, object value)
        {
            if (_configs.ContainsKey(key))
                _configs[key] = value.ToString();
            else
                _configs.Add(key, value.ToString());
        }
        
        /// <summary>
        /// Get engine config default values
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> GetDefaultConfigs()
        {
            var configs = new Dictionary<string, string>
            {
                {ApiUrlKey, "https://localhost:44305"},
                {ApiRequestTimeoutKey, "00:01:00"},
                {AuthorizationTokenKey, ""},
                {JobCheckingIntervalKey, "30"},
                {PluginsLocationKey, Path.Combine(AppContext.BaseDirectory, "plugins")},
                {WorkingLocationKey, Path.Combine(AppContext.BaseDirectory, "working")}
            };

            return configs;
        }
    }
}
