// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using Microsoft.Extensions.Logging;

namespace Polyrific.Catapult.Engine.Core.JobLogger
{
    internal class JobLogger : ILogger
    {
        private readonly JobLoggerProvider _provider;
        private readonly string _category;

        public JobLogger(JobLoggerProvider loggerProvider, string categoryName)
        {
            _provider = loggerProvider;
            _category = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return _provider.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= LogLevel.Information;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            // only allow write job log when a scope is defined for job or task
            if (_provider.CurrentScope?.State is JobScope || _provider.CurrentScope?.State is TaskScope)
            {
                var message = formatter(state, exception);
                message = $"[{DateTime.UtcNow.ToString("o")}] {message}";

                _provider.WriteLog(message);
            }
                
        }
    }
}
