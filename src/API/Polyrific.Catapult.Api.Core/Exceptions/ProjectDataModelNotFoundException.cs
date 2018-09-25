// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class ProjectDataModelNotFoundException : Exception
    {
        public int ProjectDataModelId { get; set; }
        public string ProjectDataModelName { get; set; }

        public ProjectDataModelNotFoundException(int projectDataModelId)
            : base($"Project Data Model \"{projectDataModelId}\" was not found.")
        {
            ProjectDataModelId = projectDataModelId;
        }

        public ProjectDataModelNotFoundException(string projectDataModelName)
            : base($"Project Data Model \"{projectDataModelName}\" was not found.")
        {
            ProjectDataModelName = projectDataModelName;
        }
    }
}
