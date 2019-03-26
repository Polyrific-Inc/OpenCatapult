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

        public const string RegistrationCompleted = "RegistrationCompleted";

        public const string ResetPassword = "ResetPassword";

        public const string ResetPasswordWeb = "ResetPasswordWeb";

        public const string ProjectDeleted = "ProjectDeleted";

        public string[] GetNotificationProviders(string messageType)
        {
            return GetConfigArrayValue(messageType, new string[0]);
        }

        public string GetNotificationSubject(string messageType)
        {
            return GetConfigValue($"{messageType}Subject", "");
        }

        public async Task Load()
        {
            var obj = JObject.Parse(await FileHelper.ReadAllTextAsync(NotificationConfigFile));
            _configs = obj["NotificationConfig"].ToObject<Dictionary<string, string>>();

            // check against default config
            var defaultConfigs = GetDefaultConfigs();
            foreach (var conf in defaultConfigs)
            {
                if (!_configs.ContainsKey(conf.Key))
                    _configs.Add(conf.Key, conf.Value);
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
                await FileHelper.WriteAllTextAsync(NotificationConfigFile, JsonConvert.SerializeObject(new { NotificationConfig = GetDefaultConfigs() }));
            }
        }

        private string[] GetConfigArrayValue(string key, string[] defaultValue)
        {
            return _configs.TryGetValue(key, out var sValue) ? sValue.Split(_arraySplitChar) : defaultValue;
        }

        private string GetConfigValue(string key, string defaultValue)
        {
            return _configs.TryGetValue(key, out var sValue) ? sValue : defaultValue;
        }

        private static Dictionary<string, string> GetDefaultConfigs()
        {
            var configs = new Dictionary<string, string>
            {
                {RegistrationCompleted, $"{NotificationProvider.SmtpEmail}"},
                {ResetPassword, $"{NotificationProvider.SmtpEmail}"},
                {ResetPasswordWeb, $"{NotificationProvider.SmtpEmail}"},
                {ProjectDeleted, $"{NotificationProvider.SmtpEmail}"},
                {$"{RegistrationCompleted}Subject", "Catapult - Please confirm your account"},
                {$"{ResetPassword}Subject", "Catapult - Reset password token"},
                {$"{ResetPasswordWeb}Subject", "Catapult - Reset password token"},
                {$"{ProjectDeleted}Subject", "Catapult - Your project has been deleted"}
            };

            return configs;
        }
    }
}
