// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polyrific.Catapult.Shared.Dto.Constants;

namespace Polyrific.Catapult.Plugins.Core
{
    public abstract class TaskProvider
    {
        protected Dictionary<string, object> ParsedArguments;
        protected ILogger Logger;

        protected TaskProvider(string[] args, string taskProviderName)
        {
            if (args.Contains("--attach") && Debugger.IsAttached == false)
                Debugger.Launch();

            if (args.Contains("--file"))
            {
                var filePathIndex = Array.IndexOf(args, "--file") + 1;

                if (args.Length - 1 >= filePathIndex)
                {
                    var filePath = args[filePathIndex];

                    if (File.Exists(filePath))
                    {
                        ParsedArguments = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(filePath));
                    }
                    else
                    {
                        throw new ArgumentException($"File path {filePath} was not found");
                    }
                }
                else
                {
                    throw new ArgumentException("File path is required when using --file option");
                }
            }
            else
            {
                ParsedArguments = args.Length > 0 ? JsonConvert.DeserializeObject<Dictionary<string, object>>(args[0]) : new Dictionary<string, object>();
            }            
            
            Logger = new TaskLogger(taskProviderName);
        }

        /// <summary>
        /// Name of the provider
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Type of the task. It should be one of <see cref="PluginType"/> constant.
        /// </summary>
        public abstract string Type { get; }

        /// <summary>
        /// Names of the required external service connections
        /// </summary>
        public virtual string[] RequiredServices { get; } = { };

        /// <summary>
        /// Process to execute (could be "all", "pre", "main", or "post"). Default value is "all".
        /// </summary>
        public string ProcessToExecute { get; set; } = "all";

        /// <summary>
        /// Parse arguments into properties
        /// </summary>
        public virtual void ParseArguments()
        {
            var validProcessNames = new[] {"all", "pre", "main", "post"};
            if (ParsedArguments.ContainsKey("process") && validProcessNames.Contains(ParsedArguments["process"]))
                ProcessToExecute = ParsedArguments["process"].ToString();
        }

        /// <summary>
        /// Execute the task
        /// </summary>
        /// <returns></returns>
        public abstract Task<string> Execute();

        /// <summary>
        /// Return execution result as the output of this provider
        /// </summary>
        /// <param name="result"></param>
        public void ReturnOutput(string result)
        {
            Console.WriteLine($"[OUTPUT] {result}");
        }
    }
}
