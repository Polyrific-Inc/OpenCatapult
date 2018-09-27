// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DotNetCoreTest.Helpers
{
    public class CommandHelper
    {
        public static async Task<(string output, string error, int exitCode)> Execute(string fileName, string args, ILogger logger = null)
        {
            var outputBuilder = new StringBuilder();
            var error = "";
            int exitCode = 0;

            var info = new ProcessStartInfo(fileName)
            {
                UseShellExecute = false,
                Arguments = args,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (var process = Process.Start(info))
            {
                if (process != null)
                {
                    var reader = process.StandardOutput;
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();

                        logger?.LogDebug(line);

                        outputBuilder.AppendLine(line);
                    }

                    error = await process.StandardError.ReadToEndAsync();
                    exitCode = process.ExitCode;
                }
            }

            return (outputBuilder.ToString(), error, exitCode);
        }
    }
}
