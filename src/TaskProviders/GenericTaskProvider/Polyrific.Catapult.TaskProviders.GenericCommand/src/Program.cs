// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.IO;
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

        public async override Task<(string successMessage, Dictionary<string, string> outputValues, string errorMessage)> GenericExecution()
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

            var commands = new List<string>();
            if (AdditionalConfigs.ContainsKey("CommandText") && !string.IsNullOrEmpty(AdditionalConfigs["CommandText"]))
            {
                commands.Add(AdditionalConfigs["CommandText"]);
            }
            
            string commandScript = null;
            if (AdditionalConfigs.ContainsKey("CommandScriptPath") && !string.IsNullOrEmpty(AdditionalConfigs["CommandScriptPath"]))
            {
                commandScript = AdditionalConfigs["CommandScriptPath"];
                commands.AddRange(commandScript.SplitOnNewLine());
            }                

            if (commands.Count == 0)
            {
                return (null, null, "Either Command text or Command script path should be provided.");
            }

            if (!Directory.Exists(Config.WorkingLocation))
                Directory.CreateDirectory(Config.WorkingLocation);
            
            var result = await CommandHelper.Execute(commandTool, commands.ToArray(), Config.WorkingLocation, Logger);

            string successMessage = null;
            if (string.IsNullOrEmpty(result.error))
                successMessage = "The commands has been run successfully";
            
            return (successMessage, null, result.error);
        }

        private static async Task Main(string[] args)
        {
            var app = new Program(args);

            var result = await app.Execute();
            app.ReturnOutput(result);
        }
    }
}
