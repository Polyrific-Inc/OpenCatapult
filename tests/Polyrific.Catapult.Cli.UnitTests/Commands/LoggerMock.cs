// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Logging;
using Moq;

namespace Polyrific.Catapult.Cli.UnitTests.Commands
{
    public static class LoggerMock
    {
        public static Mock<ILogger<T>> GetLogger<T>()
        {
            var logger = new Mock<ILogger<T>>();

            return logger;
        }
    }
}