// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Polyrific.Catapult.TaskProviders.Core;
using Polyrific.Catapult.TaskProviders.GenericCommand.CommandProviders;

namespace Polyrific.Catapult.TaskProviders.GenericCommand
{
    public class Program : GenericTaskProvider
    {
        public override string Name => "Polyrific.Catapult.TaskProviders.GenericCommand";

        public Program(string[] args) : base(args)
        {
        }

        public async override Task<(Dictionary<string, string> outputValues, string errorMessage)> GenericExecution()
        {
            string commandTool = null;
            if (AdditionalConfigs.ContainsKey("CommandTool") && !string.IsNullOrEmpty(AdditionalConfigs["CommandTool"]))
                commandTool = AdditionalConfigs["CommandTool"];
            
            // set default value for command tool
            if (string.IsNullOrEmpty(commandTool))
            {
                bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

                if (isWindows)
                    commandTool = "cmd.exe";
                else
                    commandTool = "/bin/bash";
            }
            
            string commands = null;
            if (AdditionalConfigs.ContainsKey("Commands") && !string.IsNullOrEmpty(AdditionalConfigs["Commands"]))
                commands = AdditionalConfigs["Commands"];
            else
                return (null, "Commands additional config is required");

            var result = await CommandHelper.Execute(commandTool, commands.SplitOnNewLine(), Config.WorkingLocation, Logger);

            return (null, result.error);
        }

        private static async Task Main(string[] args)
        {
            var app = new Program(args);

            var result = await app.Execute();
            app.ReturnOutput(result);
        }
    }
}
