// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Shared.Common.Notification
{
    public interface INotificationProvider
    {
        /// <summary>
        /// Send the notification
        /// </summary>
        /// <param name="request"></param>
        /// <param name="messageParameters"></param>
        /// <returns></returns>
        Task SendNotification(SendNotificationRequest request, Dictionary<string, string> messageParameters);
    }
}
