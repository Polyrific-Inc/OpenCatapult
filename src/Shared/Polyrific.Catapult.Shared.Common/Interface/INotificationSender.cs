// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Shared.Common.Notification;

namespace Polyrific.Catapult.Shared.Common.Interface
{
    public interface INotificationSender
    {
        string Name { get; }
        void SendRegisterNotification(SendNotificationRequest request, string confirmUrl);
        bool ValidateRequest(SendNotificationRequest request);
    }
}
