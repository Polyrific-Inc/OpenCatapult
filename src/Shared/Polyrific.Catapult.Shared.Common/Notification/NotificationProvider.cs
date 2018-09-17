// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using Polyrific.Catapult.Shared.Common.Interface;

namespace Polyrific.Catapult.Shared.Common.Notification
{
    public class NotificationProvider
    {
        private readonly IEnumerable<INotificationSender> _notificationSenders;
        private readonly NotificationConfig _notificationConfig;

        public NotificationProvider(IEnumerable<INotificationSender> notificationSenders, NotificationConfig notificationConfig)
        {
            _notificationSenders = notificationSenders;
            _notificationConfig = notificationConfig;
        }

        public void SendRegisterNotification(SendNotificationRequest request, string confirmUrl)
        {
            foreach (var sender in _notificationSenders)
                if (ValidateSenderPreference(sender) && sender.ValidateRequest(request))
                    sender.SendRegisterNotification(request, confirmUrl);
        }

        private bool ValidateSenderPreference(INotificationSender sender)
        {
            return _notificationConfig.SendRegistrationNotificationProviders.Contains(sender.Name);
        }
    }
}
