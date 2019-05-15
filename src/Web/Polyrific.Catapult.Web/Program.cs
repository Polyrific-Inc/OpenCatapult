// Copyright (c) Polyrific, Inc 2019. All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Polyrific.Catapult.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var isService = args.Contains("--service");

            var webhost = CreateWebHostBuilder(args.Where(a => a != "--service").ToArray(), isService).Build();

            try
            {
                if (isService)
                {
                    var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                    var basePath = Path.GetDirectoryName(pathToExe);
                    Directory.SetCurrentDirectory(basePath);

                    Log.Logger = new LoggerConfiguration()
                        .WriteTo.EventLog("Catapult-Web", manageEventSource: true)
                        .CreateLogger();

                    Log.Information("OpenCatapult Web is starting as Windows Service. Process ID: {@pid}.", Process.GetCurrentProcess().Id);

                    webhost.RunAsCustomService();
                }
                else
                {
                    Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(GetConfiguration(isService))
                        .CreateLogger();

                    Log.Information("Starting OpenCatapult Web host..");

                    Console.Title = "OpenCatapult Web";
                    Console.WriteLine($"Process ID: {Process.GetCurrentProcess().Id}.");

                    webhost.Run();
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "OpenCatapult Web host crashed unexpectedly.");
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
                    .AddJsonFile(
                        $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                        optional: true)
                    .AddEnvironmentVariables()
                    .Build();
            
            return config;
        }
    }
}
