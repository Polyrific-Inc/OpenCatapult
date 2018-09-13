// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class ProjectNotFoundException : Exception
    {
        public int ProjectId { get; set; }

        public ProjectNotFoundException(int projectId) 
            : base($"Project \"{projectId}\" was not found.")
        {
            ProjectId = projectId;
        }
    }
}