// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class DuplicateProjectException : Exception
    {
        public string ProjectName { get; set; }

        public DuplicateProjectException(string projectName) 
            : base($"Project \"{projectName}\" already exists.")
        {
            ProjectName = projectName;
        }
    }
}