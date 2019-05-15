// Copyright (c) Polyrific, Inc 2019. All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Polyrific.Catapult.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var isService = args.Contains("--service");

            var webhost = CreateWebHostBuilder(args.Where(a => a != "--service").ToArray(), isService).Build();

            if (isService)
            {
                webhost.RunAsCustomService();
            }
            else
            {
                Console.Title = "OpenCatapult Web";
                Console.WriteLine($"Process ID: {Process.GetCurrentProcess().Id}.");

                webhost.Run();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, bool isService) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseConfiguration(GetConfiguration(isService));
        
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
