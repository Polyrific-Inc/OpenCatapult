// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Engine.Core.JobLogger
{
    public class JobScope
    {
        public JobScope(int projectId, int jobQueueId)
        {
            ProjectId = projectId;
            JobQueueId = jobQueueId;
        }

        public int ProjectId { get; set; }
        
        public int JobQueueId { get; set; }
    }
}
