﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Polyrific.Catapult.Shared.Common.Interface;
using Polyrific.Catapult.Shared.Common.Notification;

namespace Polyrific.Catapult.Shared.EmailNotification
{
    public class EmailSender : INotificationSender
    {
        private readonly SmtpSetting _smtpSetting;

        public string Name => "email";

        public EmailSender(SmtpSetting smtpSetting)
        {
            _smtpSetting = smtpSetting;
        }

        public void SendRegisterNotification(SendNotificationRequest request, string confirmUrl)
        {
            // TODO: use razor html
            SendEmail(request.Emails, "Catapult - Please confirm your account", $"Click this link confirm: {confirmUrl}");
        }

        private void SendEmail(List<string> toAddresses, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpSetting.SenderEmail));
            foreach (var to in toAddresses)
            {
                message.To.Add(new MailboxAddress(to));
            }
            message.Subject = subject;

            message.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                client.Connect(_smtpSetting.Server, _smtpSetting.Port, SecureSocketOptions.Auto);
                client.Authenticate(_smtpSetting.Username, _smtpSetting.Password);

                client.Send(message);
                client.Disconnect(true);
            }
        }

        public bool ValidateRequest(SendNotificationRequest request)
        {
            return request.Emails?.Count > 0;
        }
    }
}
