// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Engine.Core;
using System;
using System.Timers;

namespace Polyrific.Catapult.Engine.Commands
{
    [Command(Description = "Start the engine")]
    public class StartCommand : BaseCommand
    {
        private readonly ICatapultEngine _engine;
        private readonly ICatapultEngineConfig _engineConfig;
        private readonly Timer _jobCheckerTimer;

        public StartCommand(ICatapultEngine engine, ICatapultEngineConfig engineConfig, IConsole console,
            ILogger<StartCommand> logger)
            : base(console, logger)
        {
            _engine = engine;
            _engineConfig = engineConfig;

            _jobCheckerTimer = new Timer(_engineConfig.JobCheckingInterval * 1000);
            _jobCheckerTimer.Elapsed += _jobCheckerTimer_Elapsed;
            _jobCheckerTimer.Stop();
        }

        public override string Execute()
        {
            Console.WriteLine($"Checking connection to API \"{_engineConfig.ApiUrl}\"");
            var success = _engine.CheckApiConnection().Result;
            if (!success)
                throw new Exception($"Connection to {_engineConfig.ApiUrl} was failed.");

            Console.WriteLine($"Connection to API {_engineConfig.ApiUrl} is good.");
            Console.WriteLine("Engine is waiting for a job to execute..");
            _jobCheckerTimer_Elapsed(this, null);

            while (true)
            {
                var key = System.Console.ReadKey(false);
                if (key.Key == ConsoleKey.Escape)
                {
                    if (ConfirmExit())
                        break;
                }
            }

            return "Engine stopped.";
        }

        private void _jobCheckerTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _jobCheckerTimer.Stop();

            var jobQueue = _engine.GetJobInQueue().Result;
            if (jobQueue != null)
            {
                Console.WriteLine($"Job queue {jobQueue.Code} is ready to be executed.");
                _engine.ExecuteJob(jobQueue).Wait();
                Console.WriteLine($"Job queue {jobQueue.Code} execution has completed.");
            }
            else
            {
                if (Verbose)
                {
                    Console.WriteLine($"[{e?.SignalTime.ToLongTimeString() ?? DateTime.Now.ToLongTimeString()}] No job in queue.");
                }
            }

            _jobCheckerTimer.Start();
        }
        
        private bool ConfirmExit()
        {
            _jobCheckerTimer.Stop();

            var confirmed = Prompt.GetYesNo("Do you want to exit?", false);
            if (!confirmed)
                _jobCheckerTimer.Start();

            return confirmed;
        }
    }
}
