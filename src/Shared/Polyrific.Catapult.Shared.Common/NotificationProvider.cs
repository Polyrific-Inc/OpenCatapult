// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using Polyrific.Catapult.Shared.Common.Interface;

namespace Polyrific.Catapult.Shared.Common
{
    public class NotificationProvider
    {
        private readonly IEnumerable<INotificationSender> _notificationSenders;

        public NotificationProvider(IEnumerable<INotificationSender> notificationSenders)
        {
            _notificationSenders = notificationSenders;
        }

        public void SendRegisterEmail(SendNotificationRequest request, string confirmUrl)
        {
            foreach (var sender in _notificationSenders)
                if (sender.ValidateRequest(request))
                    sender.SendRegisterEmail(request, confirmUrl);
        }
    }
}
