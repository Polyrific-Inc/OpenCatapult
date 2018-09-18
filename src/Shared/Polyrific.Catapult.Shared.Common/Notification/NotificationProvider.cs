// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Polyrific.Catapult.Shared.Common.Interface;

namespace Polyrific.Catapult.Shared.Common.Notification
{
    public class NotificationProvider
    {
        private readonly IEnumerable<INotificationSender> _notificationSenders;
        private readonly NotificationConfig _notificationConfig;

        public const string SmtpEmail = "SmtpEmail";

        public NotificationProvider(IEnumerable<INotificationSender> notificationSenders, NotificationConfig notificationConfig)
        {
            _notificationSenders = notificationSenders;
            _notificationConfig = notificationConfig;
        }

        public void SendNotification(SendNotificationRequest request, Dictionary<string, string> messageParameters)
        {
            foreach (var sender in _notificationSenders)
                if (ValidateSenderPreference(sender, request.MessageType) && sender.ValidateRequest(request))
                    sender.SendNotification(request, GetSubject(request.MessageType, messageParameters), GetBody(sender, request.MessageType, messageParameters));
        }

        private bool ValidateSenderPreference(INotificationSender sender, string messageType)
        {
            return _notificationConfig.GetNotificationProviders(messageType).Contains(sender.Name);
        }

        private string GetBody(INotificationSender sender, string messageType, Dictionary<string, string> messageParameters)
        {
            var templatePath = Path.Combine(AppContext.BaseDirectory, "Notification", "Template", $"{messageType}.html");

            var customTemplatePath = Path.Combine(AppContext.BaseDirectory, "Notification", "Template", sender.Name, $"{messageType}.html");
            if (File.Exists(customTemplatePath))
            {
                templatePath = customTemplatePath;
            }

            var bodyTemplate = File.ReadAllText(templatePath);

            return FillInHandlebars(bodyTemplate, messageParameters);
        }

        private string GetSubject(string messageType, Dictionary<string, string> messageParameters)
        {
            var subjectTemplate = _notificationConfig.GetNotificationSubject(messageType);

            return FillInHandlebars(subjectTemplate, messageParameters);
        }

        private string FillInHandlebars(string content, Dictionary<string, string> messageParameters)
        {
            var handlebarPattern = @"\{{2}\w+\}{2}";
            var matches = Regex.Matches(content, handlebarPattern);

            // Attempt to fill in each handlebar field using the values passed in.
            foreach (Match match in matches)
            {
                var handlebar = match.Value;
                var key = handlebar.Replace("{", "").Replace("}", "");
                var keyValue = "";
                if (messageParameters.ContainsKey(key))
                {
                    keyValue = messageParameters[key];
                }

                content = content.Replace(handlebar, keyValue);
            }

            return content;
        }
    }
}
