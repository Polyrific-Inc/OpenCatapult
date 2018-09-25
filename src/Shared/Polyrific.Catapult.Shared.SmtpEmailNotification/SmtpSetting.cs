// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Shared.SmtpEmailNotification
{
    public class SmtpSetting
    {
        /// <summary>
        /// The smptp server url
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// The smtp port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Username to authenticate to the smtp server
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password to authenticate to the smtp server
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The sender email address used to send the email
        /// </summary>
        public string SenderEmail { get; set; }
    }
}
