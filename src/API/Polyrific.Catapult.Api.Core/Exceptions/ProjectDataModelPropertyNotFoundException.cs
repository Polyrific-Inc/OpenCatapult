// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class ProjectDataModelPropertyNotFoundException : Exception
    {
        public string ProjectDataModelPropertyName { get; set; }

        public ProjectDataModelPropertyNotFoundException(string projectDataModelPropertyName)
            : base($"Project Data Model Property \"{projectDataModelPropertyName}\" was not found.")
        {
            ProjectDataModelPropertyName = projectDataModelPropertyName;
        }
    }
}
