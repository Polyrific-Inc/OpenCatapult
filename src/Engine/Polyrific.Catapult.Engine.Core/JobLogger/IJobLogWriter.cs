// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;

namespace Polyrific.Catapult.Engine.Core.JobLogger
{
    public interface IJobLogWriter
    {
        Task WriteLog(int projectId, int jobQueueId, string taskName, string message);
        Task EndJobLog(int jobQueueId);
    }
}
