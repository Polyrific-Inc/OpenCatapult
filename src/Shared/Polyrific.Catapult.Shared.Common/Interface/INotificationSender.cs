// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Shared.Common.Notification;

namespace Polyrific.Catapult.Shared.Common.Interface
{
    public interface INotificationSender
    {
        string Name { get; }
        void SendNotification(SendNotificationRequest request, string subject, string body);
        bool ValidateRequest(SendNotificationRequest request);
    }
}
