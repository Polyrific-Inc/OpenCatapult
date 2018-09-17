// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Polyrific.Catapult.Shared.Common.Notification
{
    public class NotificationConfig
    {
        private static readonly string NotificationConfigFile = Path.Combine(AppContext.BaseDirectory, "notificationconfig.json");
        private const char _arraySplitChar = ',';

        private Dictionary<string, string> _configs;

        public NotificationConfig()
        {
            _configs = new Dictionary<string, string>();

            InitConfigFile(false).Wait();

            Load().Wait();
        }

        public const string SendRegistrationNotificationProvidersKey = "SendRegistrationNotificationProviders";
        public string[] SendRegistrationNotificationProviders => GetConfigArrayValue(SendRegistrationNotificationProvidersKey, new string[0]);

        public async Task Load()
        {
            var obj = JObject.Parse(await File.ReadAllTextAsync(NotificationConfigFile));
            _configs = obj["NotificationConfig"].ToObject<Dictionary<string, string>>();

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
            await File.WriteAllTextAsync(NotificationConfigFile, JsonConvert.SerializeObject(new { NotificationConfig = _configs }));
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

        public static async Task InitConfigFile(bool reset = false)
        {
            if (reset && File.Exists(NotificationConfigFile))
            {
                File.Delete(NotificationConfigFile);
            }

            if (!File.Exists(NotificationConfigFile))
            {
                await File.WriteAllTextAsync(NotificationConfigFile, JsonConvert.SerializeObject(new { NotificationConfig = GetDefaultConfigs() }));
            }
        }

        private string[] GetConfigArrayValue(string key, string[] defaultValue)
        {
            return _configs.TryGetValue(key, out var sValue) ? sValue.Split(_arraySplitChar) : defaultValue;
        }

        private void SetConfigValue(string key, object value)
        {
            if (_configs.ContainsKey(key))
                _configs[key] = value.ToString();
            else
                _configs.Add(key, value.ToString());
        }

        private static Dictionary<string, string> GetDefaultConfigs()
        {
            var configs = new Dictionary<string, string>
            {
                {SendRegistrationNotificationProvidersKey, "email"}
            };

            return configs;
        }
    }
}
