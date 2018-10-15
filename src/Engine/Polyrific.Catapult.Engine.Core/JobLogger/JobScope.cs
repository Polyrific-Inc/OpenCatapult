// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Engine.Core.JobLogger
{
    public class JobScope
    {
        public JobScope(int jobQueueId)
        {
            JobQueueId = jobQueueId;
        }

        public int JobQueueId { get; set; }
    }
}
