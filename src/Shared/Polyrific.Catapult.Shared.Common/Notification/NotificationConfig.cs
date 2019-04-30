// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;

namespace Polyrific.Catapult.Shared.Common.Notification
{
    public class NotificationConfig
    {
        private const char _arraySplitChar = ',';

        public NotificationConfig()
        {
        }

        public const string RegistrationCompleted = "RegistrationCompleted";

        public const string JobQueueCompleted = "JobQueueCompleted";

        public const string ResetPassword = "ResetPassword";

        public const string ResetPasswordWeb = "ResetPasswordWeb";

        public const string ProjectDeleted = "ProjectDeleted";

        public Dictionary<string, string> NotificationProviders { get; set; }

        public Dictionary<string, string> NotificationSubject { get; set; }
        

        public string[] GetNotificationProviders(string messageType)
        {
            if (NotificationProviders != null && NotificationProviders.ContainsKey(messageType))
            {
                return NotificationProviders[messageType].Split(_arraySplitChar);
            }

            return new string[0];
        }

        public string GetNotificationSubject(string messageType)
        {
            if (NotificationProviders != null && NotificationProviders.ContainsKey(messageType))
            {
                return NotificationSubject[messageType];
            }

            return null;
        }
    }
}
