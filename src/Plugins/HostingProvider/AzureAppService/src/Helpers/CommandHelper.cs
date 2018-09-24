// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace AzureAppService.Helpers
{
    public class CommandHelper
    {
        public static Task<string> Execute(string fileName, string args, ILogger logger = null)
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
    }
}
