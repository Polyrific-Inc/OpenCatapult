// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class DefaultJobDefinitionNotFoundException : Exception
    {
        public int ProjectId { get; set; }

        public DefaultJobDefinitionNotFoundException(int projectId)
            : base($"Project {projectId} does not have a default job definition")
        {
            ProjectId = projectId;
        }
    }
}
