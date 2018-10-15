// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Engine.Core;
using Polyrific.Catapult.Engine.Core.JobLogger;
using Polyrific.Catapult.Engine.Infrastructure;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Engine
{
    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            await CatapultEngineConfig.InitConfigFile();
            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile("engineconfig.json", false)
                .Build();
            
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, configuration);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var app = new CommandLineApplication<Program>();
            ConfigureApplication(app, serviceProvider);

            try
            {
                return app.Execute(args);
            }
            catch (CommandParsingException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return -1;
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(conf =>
            {
                conf.AddConfiguration(configuration.GetSection("Logging"));
                conf.AddDebug();
                conf.AddSerilog();
            });

            // add custom logger
            services.AddSingleton<ILoggerProvider, JobLoggerProvider>();
            services.AdJobLogWriter(configuration);

            // init serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            services.AddEngineCore();
            services.AddCatapultApi(configuration);

            services.AddOptions();
        }

        private static void ConfigureApplication(CommandLineApplication app, IServiceProvider serviceProvider)
        {
            app.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(serviceProvider);

            app.RegisterCommands();
        }

        private void OnExecute(CommandLineApplication app)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("= Polyrific Catapult Engine =");
            Console.WriteLine("-----------------------------");
            Console.WriteLine();

            app.ShowHelp();
        }
    }
}
