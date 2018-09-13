// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class JobDefinitionNotFoundException : Exception
    {
        public int JobDefinitionId { get; set; }

        public JobDefinitionNotFoundException(int jobDefinitionId)
            : base($"Job definition \"{jobDefinitionId}\" was not found.")
        {
            JobDefinitionId = jobDefinitionId;
        }
    }
}