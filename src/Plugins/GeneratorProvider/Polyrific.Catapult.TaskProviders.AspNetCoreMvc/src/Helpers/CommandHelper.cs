// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polyrific.Catapult.TaskProviders.AspNetCoreMvc.Helpers
{
    public class CommandHelper
    {
        public static Task<string> Execute(string fileName, string args, Dictionary<string, string> environmentVariables = null, ILogger logger = null)
        {
            var returnValue = new StringBuilder();

            var info = new ProcessStartInfo(fileName)
            {
                UseShellExecute = false,
                Arguments = args,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            if (environmentVariables != null)
                foreach (var envVariable in environmentVariables)
                    info.EnvironmentVariables.Add(envVariable.Key, envVariable.Value);

            using (var process = Process.Start(info))
            {
                if (process != null)
                {
                    var reader = process.StandardOutput;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        logger?.LogDebug(line);

                        returnValue.AppendLine(line);
                    }
                }
            }

            return Task.FromResult(returnValue.ToString());
        }
        
        public static Task<string> RunDotnet(string args, Dictionary<string, string> environmentVariables = null, ILogger logger = null)
        {
            return Execute("dotnet", args, environmentVariables, logger);
        }
    }
}
