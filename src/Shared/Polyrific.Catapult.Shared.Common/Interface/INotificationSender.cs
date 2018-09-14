// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Shared.Common.Interface
{
    public interface INotificationSender
    {
        void SendRegisterEmail(SendNotificationRequest request, string confirmUrl);
        bool ValidateRequest(SendNotificationRequest request);
    }
}
