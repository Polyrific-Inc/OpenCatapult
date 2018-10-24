// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Diagnostics;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Common;

namespace Polyrific.Catapult.Cli.Commands
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
        /// Execute the command, and return the result message
        /// </summary>
        /// <returns>Execution result message</returns>
        public abstract string Execute();

        public virtual string GetHelpFooter()
        {
            return "";
        }

        protected virtual int OnExecute(CommandLineApplication app)
        {
            Console.WriteLine("----------------------------");
            Console.WriteLine("|     OpenCatapult CLI     |");
            Console.WriteLine("----------------------------");
            Console.WriteLine();

            try
            {
                #if DEBUG
                DebugPreprocessing();
                #endif

                var resultMessage = Execute();

                if (!string.IsNullOrEmpty(resultMessage))
                    Console.WriteLine(resultMessage);

                Console.WriteLine();

                return 0;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetLastInnerExceptionMessage();
                Logger.LogError(ex, errorMessage);

                Console.Error.WriteLine(errorMessage);
                Console.WriteLine();

                return 1;
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
