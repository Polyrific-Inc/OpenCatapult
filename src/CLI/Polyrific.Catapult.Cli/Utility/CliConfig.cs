// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polyrific.Catapult.Cli.Exceptions;

namespace Polyrific.Catapult.Cli
{
    public class CliConfig : ICliConfig
    {
        private static readonly string CliConfigFile = Path.Combine(AppContext.BaseDirectory, "cliconfig.json");
        private readonly ILogger<CliConfig> _logger;

        private Dictionary<string, string> _configs;

        public CliConfig(ILogger<CliConfig> logger)
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
        
        public const string AppDataFolderPathKey = "AppDataFolderPath";
        public string AppDataFolderPath => GetConfigValue(AppDataFolderPathKey, "");

        public Dictionary<string, string> Configs => _configs;

        public async Task Load()
        {
            var obj = JObject.Parse(await File.ReadAllTextAsync(CliConfigFile));
            _configs = obj["CliConfig"].ToObject<Dictionary<string, string>>();

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
            await File.WriteAllTextAsync(CliConfigFile, JsonConvert.SerializeObject(new { CliConfig = _configs }));
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
        /// Initiate cli config file 
        /// </summary>
        /// <param name="reset">If <value>true</value>, the existing config file will be reset with the default values</param>
        /// <param name="logger">Logger provider</param>
        /// <returns></returns>
        public static async Task InitConfigFile(bool reset = false, ILogger<CliConfig> logger = null)
        {
            if (reset && File.Exists(CliConfigFile))
            {
                logger?.LogInformation($"Deleting config file \"{CliConfigFile}\"");
                File.Delete(CliConfigFile);
            }

            if (!File.Exists(CliConfigFile))
            {
                logger?.LogInformation($"The config file \"{CliConfigFile}\" doesn't exist. Creating one..");
                await File.WriteAllTextAsync(CliConfigFile, JsonConvert.SerializeObject(new { CliConfig = GetDefaultConfigs() }));
            }
        }

        /// <summary>
        /// Get config value, and throw <see cref="InvalidConfigNameException"/> if not found.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetConfigValue(string key)
        {
            if (_configs.ContainsKey(key))
                return _configs.GetValueOrDefault(key);

            throw new InvalidConfigNameException(key);
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
        /// Get cli config default values
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> GetDefaultConfigs()
        {
            var configs = new Dictionary<string, string>
            {
                {ApiUrlKey, "https://localhost:44305"},
                {ApiRequestTimeoutKey, "00:01:00"},
                {AppDataFolderPathKey, "Polyrific/Catapult" }
            };

            return configs;
        }
    }
}
