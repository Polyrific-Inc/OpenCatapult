// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using Microsoft.Extensions.Logging;

namespace Polyrific.Catapult.TaskProviders.Core
{
    public class TaskLogger : ILogger
    {
        private readonly string _taskProviderName;

        public TaskLogger(string taskProviderName)
        {
            _taskProviderName = taskProviderName;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = formatter?.Invoke(state, exception);

            var logMessage = $"[LOG][{Enum.GetName(typeof(LogLevel), logLevel)}][{_taskProviderName}] {message}";
            if (exception != null)
                logMessage += $" {exception.StackTrace}";

            Console.WriteLine(logMessage);
        }
        
    }
}
