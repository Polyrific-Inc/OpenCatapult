﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Polyrific.Catapult.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var isService = args.Contains("--service");

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(GetConfiguration(isService))
                .CreateLogger();

            try
            {
                var webhost = CreateWebHostBuilder(args.Where(a => a != "--service").ToArray(), isService).Build();

                if (isService)
                {
                    webhost.RunAsCustomService();
                } 
                else
                {
                    Console.Title = "OpenCatapult API";
                    Log.Information("Starting Catapult API host..");
                    Console.WriteLine($"Process ID: {Process.GetCurrentProcess().Id}.");
                    webhost.Run();
                }                
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Catapult API host crashed unexpectedly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, bool isService) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseConfiguration(GetConfiguration(isService))
                .UseSerilog();
        
        public static IConfiguration GetConfiguration(bool isService) {
            var basePath = Directory.GetCurrentDirectory();
            if (isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                basePath = Path.GetDirectoryName(pathToExe);
            }

            var config = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile("notificationconfig.json", optional: false, reloadOnChange: true)
                    .AddJsonFile(
                        $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                        optional: true)
                    .AddEnvironmentVariables()
                    .Build();
            
            return config;
        }
    }
}
