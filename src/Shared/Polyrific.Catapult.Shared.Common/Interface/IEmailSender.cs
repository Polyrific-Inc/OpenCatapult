// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;

namespace Polyrific.Catapult.Shared.Common.Interface
{
    public interface IEmailSender
    {
        void SendRegisterEmail(string email, string confirmUrl);
        void SendEmail(List<string> toAddresses, string subject, string body);
    }
}
