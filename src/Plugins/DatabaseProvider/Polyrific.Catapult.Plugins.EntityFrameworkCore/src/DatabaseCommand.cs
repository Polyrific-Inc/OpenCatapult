// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.EntityFrameworkCore.Helpers;

namespace Polyrific.Catapult.Plugins.EntityFrameworkCore
{
    public class DatabaseCommand : IDatabaseCommand
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;

        public DatabaseCommand(ILogger logger, string connectionString)
        {
            _logger = logger;
            _connectionString = connectionString;
        }

        public async Task<string> Update(string dataProject, string startupProject)
        {
            var pathToEfDll = GetPathToEfDll();
            if (string.IsNullOrEmpty(pathToEfDll))
                return "Failed getting ef.dll for migration";

            var efMigrationsDllDepsJson = startupProject.Replace("dll", "deps.json");
            if (!File.Exists(efMigrationsDllDepsJson))
                return "Failed getting .deps.json file";

            var efMigrationsDllRuntimeConfig = startupProject.Replace("dll", "runtimeconfig.json");
            if (!File.Exists(efMigrationsDllRuntimeConfig))
                return "Failed getting .runtimeconfig.json file";

            var args = $"exec --depsfile \"{efMigrationsDllDepsJson}\" --runtimeconfig \"{efMigrationsDllRuntimeConfig}\" \"{pathToEfDll}\" database update --assembly \"{dataProject}\" --startup-assembly \"{startupProject}\" --verbose";
            var result = await RunDotnet(args);

            if (!string.IsNullOrEmpty(result.error))
                return result.error;
            else if (!result.output.EndsWith("Done.\r\n"))
                return "Failed updating database";
            else
                return "";
        }

        private async Task<(string output, string error)> RunDotnet(string args)
        {
            var result = await CommandHelper.Execute("dotnet", args, new System.Collections.Generic.Dictionary<string, string>
            {
                { "ConnectionStrings__DefaultConnection", _connectionString }
            }, _logger);

            return result;
        }

        private string GetPathToEfDll()
        {
            // option 1
            string pathToNuget = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget", "packages");
            var dotnetEfFolder = Path.Combine(pathToNuget, "microsoft.entityframeworkcore.tools.dotnet");
            var latestEf = Directory.Exists(dotnetEfFolder) ? Directory.EnumerateDirectories(dotnetEfFolder).LastOrDefault() : null;
            var pathToEf = latestEf != null ? Path.Combine(latestEf, "tools\\netcoreapp2.0", "ef.dll") : null;
            if (pathToEf != null && File.Exists(pathToEf))
            {
                return pathToEf;
            }

            string pathToDotnet = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "dotnet", "sdk");
            var latestDotnet = Directory.Exists(pathToDotnet) ? Directory.EnumerateDirectories(pathToDotnet, "2.1*").LastOrDefault() : null;

            if (latestDotnet != null)
            {
                // option 2
                dotnetEfFolder = Path.Combine(latestDotnet, "DotnetTools", "dotnet-ef");
                latestEf = Directory.Exists(dotnetEfFolder) ? Directory.EnumerateDirectories(dotnetEfFolder).LastOrDefault() : null;
                pathToEf = latestEf != null ? Path.Combine(latestEf, "tools\\netcoreapp2.1\\any\\tools\\netcoreapp2.0\\any", "ef.dll") : null;

                if (pathToEf != null && File.Exists(pathToEf))
                {
                    return pathToEf;
                }
            }

            // option 3
            dotnetEfFolder = Path.Combine(pathToDotnet, "NuGetFallbackFolder", "microsoft.entityframeworkcore.tools.dotnet");
            latestEf = Directory.Exists(dotnetEfFolder) ? Directory.EnumerateDirectories(dotnetEfFolder).LastOrDefault() : null;
            pathToEf = latestEf != null ? Path.Combine(latestEf, "tools\\netcoreapp2.0", "ef.dll") : null;

            if (pathToEf != null && File.Exists(pathToEf))
            {
                return pathToEf;
            }

            return null;
        }
    }
}
