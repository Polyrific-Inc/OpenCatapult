// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class JobProcessedByOtherEngineException : Exception
    {
        public int JobQueueId { get; set; }

        public JobProcessedByOtherEngineException(int jobQueueId)
            : base($"The job \"{jobQueueId}\" was already processed by another engine.")
        {
            JobQueueId = jobQueueId;
        }
    }
}