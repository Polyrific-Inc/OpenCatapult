// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Logging;

namespace Polyrific.Catapult.Api
{
    public static class LoggerExtensions
    {
        public static void LogRequest(this ILogger logger, string message, params object[] args)
        {
            logger.LogInformation($"[Req] {message}", args);
        }
        public static void LogRequestDebug(this ILogger logger, string message, params object[] args)
        {
            logger.LogDebug($"[Req] {message}", args);
        }

        public static void LogResponse(this ILogger logger, string message, params object[] args)
        {
            logger.LogInformation($"[Res] {message}", args);
        }
        public static void LogResponseDebug(this ILogger logger, string message, params object[] args)
        {
            logger.LogDebug($"[Res] {message}", args);
        }
    }
}
