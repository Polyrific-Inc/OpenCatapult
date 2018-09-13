// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Polyrific.Catapult.Api
{
    public static class GlobalExceptionHandler
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger, bool isDevelopment)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError(contextFeature.Error, "An error was catched by global exception handler.");

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            context.Response.StatusCode,
                            Message = isDevelopment ? contextFeature.Error.Message : "Internal Server Error"
                        }));
                    }
                });
            });
        }
    }
}