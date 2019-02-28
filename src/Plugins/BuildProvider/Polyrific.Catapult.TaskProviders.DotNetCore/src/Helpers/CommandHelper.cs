// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polyrific.Catapult.TaskProviders.DotNetCore.Helpers
{
    public class CommandHelper
    {
        public static async Task<(string output, string error)> Execute(string fileName, string args, ILogger logger = null)
        {
            var outputBuilder = new StringBuilder();
            var error = "";

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
                }
            }

            return (outputBuilder.ToString(), error);
        }
    }
}
