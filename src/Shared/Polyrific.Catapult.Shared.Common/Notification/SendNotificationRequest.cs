// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;

namespace Polyrific.Catapult.Shared.Common.Notification
{
    public class SendNotificationRequest
    {
        public string MessageType { get; set; }

        public List<string> Emails { get; set; }
    }
}
