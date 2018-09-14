// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Polyrific.Catapult.Shared.Common.Interface;

namespace Polyrific.Catapult.Shared.Common
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpSetting _smtpSetting;

        public EmailSender(SmtpSetting smtpSetting)
        {
            _smtpSetting = smtpSetting;
        }

        public void SendRegisterEmail(string email, string confirmUrl)
        {
            // TODO: use razor html
            SendEmail(new List<string>
            {
                email
            }, "Catapult - Please confirm your account", $"Click this link confirm: {confirmUrl}");
        }

        public void SendEmail(List<string> toAddresses, string subject, string body)
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
                client.Connect(_smtpSetting.Server, _smtpSetting.Port, true);
                client.Authenticate(_smtpSetting.Username, _smtpSetting.Password);

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }    
}
