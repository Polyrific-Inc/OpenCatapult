// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Engine.Core;
using Polyrific.Catapult.Engine.Extensions;

namespace Polyrific.Catapult.Engine.Commands
{
    [Command(Description = "Check for any available job in queue")]
    public class CheckCommand : BaseCommand
    {
        private readonly ICatapultEngine _engine;

        public CheckCommand(ICatapultEngine engine, IConsole console, ILogger<CheckCommand> logger) : base(console, logger)
        {
            _engine = engine;
        }

        [Option("--auto", "Automatically execute the job without asking for confirmation", CommandOptionType.NoValue)]
        public bool AutoExecute { get; set; }

        public override string Execute()
        {
            var jobQueue = _engine.GetJobInQueue().Result;
            if (jobQueue == null)
                return "No job was found in queue.";
            
            Console.WriteLine($"A job queue \"{jobQueue.Code}\" was found.");

            var confirmExecute = AutoExecute || Console.GetYesNo("Do you want to proceed with the execution?", false);
            if (!confirmExecute)
                return $"Job queue \"{jobQueue.Code}\" has not been executed.";
            
            Console.WriteLine($"Executing job queue {jobQueue.Code}...");
            _engine.ExecuteJob(jobQueue).Wait();
            
            var message = $"Job queue \"{jobQueue.Code}\" execution has completed.";
            Logger.LogInformation(message);

            return message;
        }
    }
}
