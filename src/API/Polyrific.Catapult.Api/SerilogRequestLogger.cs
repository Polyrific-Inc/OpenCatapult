// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Polyrific.Catapult.Api
{
    public class SerilogRequestLogger
    {
        private readonly RequestDelegate _next;

        public SerilogRequestLogger(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            // Push the user name into the log context so that it is included in all log entries
            LogContext.PushProperty("User", httpContext.User.Identity.Name);

            await _next(httpContext);
        }
    }
}
