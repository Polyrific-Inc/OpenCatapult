// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Infrastructure;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Cli
{
    class Program
    {
        private static async Task<int> Main(string[] args)
        {
            await CliConfig.InitConfigFile();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile("cliconfig.json", false)
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

        private static void ConfigureServices(ServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(conf =>
            {
                conf.AddConfiguration(configuration.GetSection("Logging"));
                conf.AddDebug();
                conf.AddSerilog();
            });

            // init serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            services.AddTokenStore(configuration);
            services.AddJobQueueLogListener(configuration);

            services.AddTransient<ITemplateWriter, TemplateWriter>();
            services.AddTransient<ICliConfig, CliConfig>();
            services.AddTransient<IConsoleReader, ConsoleReader>();

            services.AddCatapultApi(configuration);
        }

        private static void ConfigureApplication(CommandLineApplication<Program> app, ServiceProvider serviceProvider)
        {
            app.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(serviceProvider);

            app.HelpTextGenerator = new CatapultHelpTextGenerator();

            app.ValueParsers.Add(new CatapultOptionParser());

            app.RegisterCommands();
        }

        private void OnExecute(CommandLineApplication app)
        {
            Console.WriteLine("--------------------------");
            Console.WriteLine("= Polyrific Catapult CLI =");
            Console.WriteLine("--------------------------");
            Console.WriteLine();

            app.ShowHelp();
        }
    }
}
