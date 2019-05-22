// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polyrific.Catapult.TaskProviders.GenericCommand.CommandProviders
{
    public static class CommandHelper
    {
        public static async Task<(string output, string error)> Execute(string commandTool, string[] commands, string workingDirectory, ILogger logger = null)
        {
            var returnValue = new StringBuilder();
            var error = "";

            var info = new ProcessStartInfo(commandTool)
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (var process = Process.Start(info))
            {
                if (process != null)
                {
                    using (var sw = process.StandardInput)
                    {
                        if (sw.BaseStream.CanWrite)
                        {
                            sw.WriteLine($"cd {workingDirectory}");

                            foreach (var command in commands)
                                sw.WriteLine(command);
                        }
                    }

                    var reader = process.StandardOutput;
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();

                        logger?.LogDebug(line);

                        returnValue.AppendLine(line);
                    }

                    error = await process.StandardError.ReadToEndAsync();
                }
            }

            return (returnValue.ToString(), error);
        }
    }
}
