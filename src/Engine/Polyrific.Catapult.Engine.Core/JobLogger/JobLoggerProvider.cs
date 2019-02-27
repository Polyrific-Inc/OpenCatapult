// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using Microsoft.Extensions.Logging;

namespace Polyrific.Catapult.Engine.Core.JobLogger
{
    public class JobLoggerProvider : ILoggerProvider
    {
        private readonly IJobLogWriter _jobLogWriter;

        internal JobLoggerScope CurrentScope { get; set; }

        public JobLoggerProvider(IJobLogWriter signalRClient)
        {
            _jobLogWriter = signalRClient;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new JobLogger(this, categoryName);
        }

        public IDisposable BeginScope<T>(T state)
        {
            return new JobLoggerScope(this, state);
        }

        public void Dispose()
        {
            CurrentScope.Dispose();
        }

        public void WriteLog(string message)
        {
            int projectId = 0;
            int jobQueueId = 0;
            string taskName = null;

            if (CurrentScope.State is JobScope jobScope)
            {
                projectId = jobScope.ProjectId;
                jobQueueId = jobScope.JobQueueId;
                taskName = null;
            }
            else if (CurrentScope.State is TaskScope taskScope && CurrentScope.Parent?.State is JobScope parentJobScope)
            {
                projectId = parentJobScope.ProjectId;
                jobQueueId = parentJobScope.JobQueueId;
                taskName = taskScope.TaskName;
            }

            if (jobQueueId != 0)
                _jobLogWriter.WriteLog(projectId, jobQueueId, taskName, message).Wait();
        }
    }
}
