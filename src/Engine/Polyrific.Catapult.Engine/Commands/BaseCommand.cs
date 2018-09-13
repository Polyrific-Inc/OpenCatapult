// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Polyrific.Catapult.Engine.Commands
{
    [HelpOption]
    public abstract class BaseCommand
    {
        protected readonly IConsole Console;
        protected readonly ILogger<BaseCommand> Logger;

        protected BaseCommand(IConsole console, ILogger<BaseCommand> logger)
        {
            Console = console;
            Logger = logger;
        }

        #if DEBUG
        [Option("--debug", "Enter debug mode", CommandOptionType.NoValue)]
        public bool IsDebugMode { get; set; }
        #endif

        [Option("--verbose", "Verbose messages", CommandOptionType.NoValue)]
        public bool Verbose { get; set; }

        /// <summary>
        /// Execute the command, and return a message
        /// </summary>
        /// <returns>Message that will be return back to user</returns>
        public abstract string Execute();

        protected virtual int OnExecute(CommandLineApplication app)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("= Polyrific Catapult Engine =");
            Console.WriteLine("-----------------------------");
            Console.WriteLine();
            
            try
            {
                #if DEBUG
                DebugPreprocessing();
                #endif

                var message = Execute();

                if (!string.IsNullOrEmpty(message))
                    Console.WriteLine(message);

                return 1;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                Console.Error.WriteLine(ex.Message);

                return 0;
            }
        }

        #if DEBUG
        private void DebugPreprocessing()
        {
            if (IsDebugMode)
            {
                Console.WriteLine($"Ready for debugger to attach. Process ID: {Process.GetCurrentProcess().Id}.");
                Console.WriteLine("Press ENTER to continue.");
                System.Console.ReadLine();
            }
        }
        #endif
    }
}