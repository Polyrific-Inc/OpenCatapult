// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class CancelCompletedJobException : Exception
    {
        public int JobQueueId { get; set; }

        public CancelCompletedJobException(int jobQueueId)
            : base($"The job \"{jobQueueId}\" was already completed.")
        {
            JobQueueId = jobQueueId;
        }
    }
}