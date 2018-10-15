// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Polyrific.Catapult.Shared.Common.Interface;
using Polyrific.Catapult.Shared.Common.Notification;

namespace Polyrific.Catapult.Shared.SmtpEmailNotification
{
    public class SmtpEmailSender : INotificationSender
    {
        private readonly SmtpSetting _smtpSetting;

        public string Name => NotificationProvider.SmtpEmail;

        public SmtpEmailSender(SmtpSetting smtpSetting)
        {
            _smtpSetting = smtpSetting;
        }
        
        public async Task SendNotification(SendNotificationRequest request, string subject, string body)
        {
            await SendEmail(request.Emails, subject, body);
        }

        private async Task SendEmail(List<string> toAddresses, string subject, string body)
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
                await client.ConnectAsync(_smtpSetting.Server, _smtpSetting.Port, SecureSocketOptions.Auto);
                await client.AuthenticateAsync(_smtpSetting.Username, _smtpSetting.Password);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        public bool ValidateRequest(SendNotificationRequest request)
        {
            return request.Emails?.Count > 0;
        }
    }
}
