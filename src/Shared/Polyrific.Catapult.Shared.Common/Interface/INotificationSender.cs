// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Common.Notification;

namespace Polyrific.Catapult.Shared.Common.Interface
{
    public interface INotificationSender
    {
        /// <summary>
        /// Name of the notification sender
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Send the notification
        /// </summary>
        /// <param name="request">The generic send notification request object</param>
        /// <param name="subject">Subject of the notification</param>
        /// <param name="body">Body of the notification</param>
        Task SendNotification(SendNotificationRequest request, string subject, string body);

        /// <summary>
        /// Validate the notification request whether it can be sent using the current sender
        /// </summary>
        /// <param name="request">The generic send notification request object</param>
        /// <returns>True if the request is valid; otherwise, returns false</returns>
        bool ValidateRequest(SendNotificationRequest request);
    }
}
