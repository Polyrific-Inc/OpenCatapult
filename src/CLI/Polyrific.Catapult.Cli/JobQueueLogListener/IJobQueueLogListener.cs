// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Cli
{
    public interface IJobQueueLogListener
    {
        Task Listen(int jobQueueId, Action<string> onLogReceived, Action<string> onError);
    }
}
