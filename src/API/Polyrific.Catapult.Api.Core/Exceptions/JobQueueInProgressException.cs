// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class JobQueueInProgressException : Exception
    {
        public int JobDefinitionId { get; set; }

        public JobQueueInProgressException(int projectId)
            : base($"There is already a running job in project \"{projectId}\". Please wait for it to complete before queueing a new job.")
        {
            JobDefinitionId = projectId;
        }
    }
}