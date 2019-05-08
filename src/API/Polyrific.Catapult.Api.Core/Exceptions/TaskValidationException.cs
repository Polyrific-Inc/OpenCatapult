// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class TaskValidationException : Exception
    {
        public TaskValidationException(string jobName, string message, Exception innerException)
            : base($"Job {jobName} have invalid task(s): {message}", innerException)
        {
        }
    }
}
